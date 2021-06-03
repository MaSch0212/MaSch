using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace MaSch.Core.Converters
{
    /// <summary>
    /// A <see cref="IObjectConverter"/> that converts any enumerable to another enumerable type.
    /// </summary>
    public class EnumerableConverter : IObjectConverter
    {
        private static readonly Dictionary<Type, Type> InterfaceToClassMap = new()
        {
            [typeof(IEnumerable)] = typeof(object[]),
            [typeof(IEnumerable<>)] = typeof(List<>),
            [typeof(ICollection)] = typeof(object[]),
            [typeof(ICollection<>)] = typeof(List<>),
            [typeof(IList)] = typeof(List<object>),
            [typeof(IList<>)] = typeof(List<>),
            [typeof(IReadOnlyList<>)] = typeof(ReadOnlyCollection<>),
            [typeof(IReadOnlyCollection<>)] = typeof(ReadOnlyCollection<>),
        };

        private readonly int _priority;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerableConverter"/> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        public EnumerableConverter(int priority = 0)
        {
            _priority = priority;
        }

        /// <inheritdoc />
        public bool CanConvert(Type? sourceType, Type targetType, IObjectConvertManager convertManager)
        {
            if (targetType.IsInterface)
                return InterfaceToClassMap.ContainsKey(targetType.IsGenericType ? targetType.GetGenericTypeDefinition() : targetType);

            if (targetType.IsArray)
                return true;

            var ctors = targetType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Select(x => x.GetParameters().Select(y => y.ParameterType).ToArray()).ToArray();
            if (ctors.Any(x => x.Length == 1 && InterfaceToClassMap.ContainsKey(x[0].IsGenericType ? x[0].GetGenericTypeDefinition() : x[0])))
                return true;

            if ((ctors.Any(x => x.Length == 0) || ctors.Any(x => x.Length == 1 && x[0].In(typeof(short), typeof(int), typeof(long)))) &&
                (targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance).Any(x => x.Name == "Add" && x.GetParameters().Length == 1) ||
                 (from p in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                  let idxp = p.GetIndexParameters()
                  where idxp.Length == 1 && idxp[0].ParameterType.In(typeof(short), typeof(int), typeof(long)) && p.SetMethod != null
                  select p).Any()))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public object? Convert(object? obj, Type? sourceType, Type targetType, IObjectConvertManager convertManager, IFormatProvider formatProvider)
        {
            var originalSourceType = sourceType;

            var exceptions = new List<Exception>();
            try
            {
                if (obj is IEnumerable e && ConvertImpl(e, out var res))
                    return res;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }

            try
            {
                sourceType = (obj?.GetType() ?? sourceType)?.MakeArrayType();
                IEnumerable e;
                if (sourceType != null)
                {
                    Array arr = (Array)Activator.CreateInstance(sourceType, new object[] { 1 })!;
                    arr.SetValue(obj, 0);
                    e = arr;
                }
                else
                {
                    e = new[] { obj };
                }

                if (ConvertImpl(e, out var res))
                    return res;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }

            throw new AggregateException($"Could not convert \"{originalSourceType?.FullName ?? "(null)"}\" to \"{targetType.FullName}\".", exceptions);

            bool ConvertImpl(IEnumerable obj, out object? r)
            {
                var actualSourceType = obj?.GetType() ?? sourceType;
                if (actualSourceType?.IsCastableTo(targetType) == true)
                {
                    r = obj.CastTo(targetType);
                    return true;
                }

                var actualTargetType = targetType;
                if (targetType.IsInterface)
                {
                    actualTargetType = InterfaceToClassMap[targetType.IsGenericType ? targetType.GetGenericTypeDefinition() : targetType];
                    if (actualTargetType.IsGenericType)
                        actualTargetType = actualTargetType.MakeGenericType(targetType.GenericTypeArguments);
                }

                var ctors = actualTargetType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Select(x => (Ctor: x, Params: x.GetParameters().Select(y => y.ParameterType).ToArray())).ToArray();

                var (ctor, paramTypes) = ctors.FirstOrDefault(x => x.Params.Length == 1 && x.Params[0].In(typeof(short), typeof(int), typeof(long)));
                if (ctor != null)
                {
                    object? result = null;
                    ICollection? collection = null;
                    ICollection GetCollection()
                    {
                        if (collection != null)
                            return collection;
                        if (obj is ICollection c)
                            return collection = c;

                        var c2 = new List<object?>();
                        if (obj != null)
                        {
                            foreach (var item in obj)
                                c2.Add(item);
                        }

                        return collection = c2;
                    }

                    if (paramTypes.Length == 0)
                        result = ctor.Invoke(null);
                    else if (paramTypes.Length == 1)
                        result = ctor.Invoke(new object?[] { convertManager.Convert(GetCollection().Count, paramTypes[0], formatProvider) });

                    if (result != null)
                    {
                        if (result is Array array)
                        {
                            var itemType = array.GetType().GetElementType()!;
                            var i = 0;
                            foreach (var item in GetCollection())
                                array.SetValue(convertManager.Convert(item, itemType, formatProvider), i++);
                            r = array;
                            return true;
                        }

                        var resulType = result.GetType();
                        var addMethod = resulType.GetMethods(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(x => x.Name == "Add" && x.GetParameters().Length == 1);
                        if (addMethod != null)
                        {
                            var itemType = addMethod.GetParameters()[0];
                            foreach (var item in GetCollection())
                                addMethod.Invoke(result, new object?[] { convertManager.Convert(item, itemType.ParameterType, formatProvider) });
                            r = result;
                            return true;
                        }

                        var indexProperty = (from p in resulType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                             let idxp = p.GetIndexParameters()
                                             where idxp.Length == 1 && idxp[0].ParameterType.In(typeof(short), typeof(int), typeof(long)) && p.SetMethod != null
                                             select p).FirstOrDefault();
                        if (indexProperty != null)
                        {
                            var i = 0;
                            foreach (var item in GetCollection())
                                indexProperty.SetValue(result, convertManager.Convert(item, indexProperty.PropertyType, formatProvider), new object?[] { convertManager.Convert(i, indexProperty.GetIndexParameters()[0].ParameterType, formatProvider) });
                            r = result;
                            return true;
                        }
                    }
                }

                (ctor, paramTypes) = ctors.FirstOrDefault(x => x.Params.Length == 1 && InterfaceToClassMap.ContainsKey(x.Params[0].IsGenericType ? x.Params[0].GetGenericTypeDefinition() : x.Params[0]));
                if (ctor != null)
                {
                    if (actualSourceType?.IsCastableTo(paramTypes[0]) == true)
                        r = ctor.Invoke(new object?[] { obj.CastTo(paramTypes[0]) });
                    else
                        r = ctor.Invoke(new object?[] { convertManager.Convert(obj, actualSourceType, paramTypes[0], formatProvider) });
                    return true;
                }

                r = null;
                return false;
            }
        }

        /// <inheritdoc />
        public int GetPriority(Type? sourceType, Type targetType) => _priority;
    }
}
