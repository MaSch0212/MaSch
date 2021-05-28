using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MaSch.Console.Cli.Runtime
{
    public abstract class CliCommandMemberInfo : ICliCommandMemberInfo
    {
        private readonly Dictionary<object, bool> _optionsToHasValue = new(new ObjectHashComparer());

        protected PropertyInfo Property { get; }

        public ICliCommandInfo Command { get; }
        public string PropertyName => Property.Name;
        public Type PropertyType => Property.PropertyType;

        public abstract bool IsRequired { get; }
        public abstract object? DefaultValue { get; }
        public abstract string? HelpText { get; }

        protected CliCommandMemberInfo(ICliCommandInfo command, PropertyInfo property)
        {
            Command = Guard.NotNull(command, nameof(command));
            Property = Guard.NotNull(property, nameof(property));

            if (Property.GetIndexParameters()?.Any() == true)
                throw new ArgumentException($"The property cannot be an indexer.", nameof(property));
            if (Property.GetAccessors(true).Any(x => x.IsStatic))
                throw new ArgumentException($"The property \"{property.Name}\" cannot be static.", nameof(property));
            if (Property.GetAccessors(true).Any(x => x.IsAbstract))
                throw new ArgumentException($"The property \"{property.Name}\" cannot be abstract.", nameof(property));
            if (!Property.CanRead || !Property.CanWrite)
                throw new ArgumentException($"The property \"{property.Name}\" needs to have a setter and getter.", nameof(property));
        }

        public virtual object? GetValue(object options)
        {
            Guard.NotNull(options, nameof(options));
            return Property.GetValue(options);
        }

        public virtual bool HasValue(object options)
        {
            Guard.NotNull(options, nameof(options));
            return _optionsToHasValue.TryGetValue(options, out bool hasValue) && hasValue;
        }

        public virtual void SetDefaultValue(object options)
        {
            Guard.NotNull(options, nameof(options));
            if (typeof(IEnumerable).IsAssignableFrom(PropertyType) && PropertyType != typeof(string))
                SetValue(options, Array.Empty<object?>(), true);
            else
                SetValue(options, DefaultValue ?? PropertyType.GetDefault(), true);
        }

        public virtual void SetValue(object options, object? value)
        {
            Guard.NotNull(options, nameof(options));
            SetValue(options, value, false);
        }

        private void SetValue(object options, object? value, bool isDefault)
        {
            if (typeof(IEnumerable).IsAssignableFrom(Property.PropertyType) && Property.PropertyType != typeof(string) && !isDefault)
            {
                var currentValue = Property.GetValue(options);
                if (currentValue is IEnumerable e1 && value is IEnumerable e2)
                    value = e1.ToGeneric().Concat(e2.ToGeneric());
            }

            Property.SetValue(options, value.ConvertTo(Property.PropertyType, CultureInfo.InvariantCulture));

            if (_optionsToHasValue.ContainsKey(options))
                _optionsToHasValue[options] = !isDefault;
            else
                _optionsToHasValue.Add(new EquatableWeakReference(options), !isDefault);
        }

        [ExcludeFromCodeCoverage]
        private class ObjectHashComparer : IEqualityComparer<object>
        {
            public new bool Equals(object? x, object? y)
            {
                if (x is EquatableWeakReference r1)
                    return r1.Equals(y);
                else if (y is EquatableWeakReference r2)
                    return r2.Equals(x);
                else
                    return object.Equals(x, y);
            }

            public int GetHashCode([DisallowNull] object obj)
                => obj is EquatableWeakReference r ? r.GetHashCode() : obj.GetInitialHashCode();
        }
    }
}
