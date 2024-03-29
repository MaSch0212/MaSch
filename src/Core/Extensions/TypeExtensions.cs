﻿namespace MaSch.Core.Extensions;

/// <summary>
/// This class contains extension methods for the Type-type.
/// </summary>
[SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "Accessing non-public members in this extension class is necessary.")]
public static class TypeExtensions
{
    private static readonly Dictionary<Type, string> _specialTypeCSharpRepresentations = new()
    {
        [typeof(short)] = "short",
        [typeof(int)] = "int",
        [typeof(long)] = "long",
        [typeof(string)] = "string",
        [typeof(object)] = "object",
        [typeof(bool)] = "bool",
        [typeof(void)] = "void",
        [typeof(char)] = "char",
        [typeof(byte)] = "byte",
        [typeof(ushort)] = "ushort",
        [typeof(uint)] = "uint",
        [typeof(ulong)] = "ulong",
        [typeof(sbyte)] = "sbyte",
        [typeof(float)] = "float",
        [typeof(double)] = "double",
        [typeof(decimal)] = "decimal",
    };

    /// <summary>
    /// Checks if this type can be casted implicitly or explicitly to the given type. See <see href="http://stackoverflow.com/questions/2119441/check-if-types-are-castable-subclasses"/>.
    /// </summary>
    /// <param name="from">The type which is casted.</param>
    /// <param name="to">The type to which the object should be casted.</param>
    /// <param name="implicitly">if true only implicitly casts are checked, otherwise also explicit casts are checked.</param>
    /// <returns>true if this type is castable to the given type, otherwise false.</returns>
    public static bool IsCastableTo(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        this Type from,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        Type to,
        bool implicitly = false)
    {
        _ = Guard.NotNull(from);
        _ = Guard.NotNull(to);
        return to.IsAssignableFrom(from) || from.HasCastDefined(to, implicitly);
    }

    /// <summary>
    /// Determines whether this method is overriding some method from a base class. See <see href="https://stackoverflow.com/questions/5746447/determine-whether-a-c-sharp-method-has-keyword-override-using-reflection"/>.
    /// </summary>
    /// <param name="methodInfo">The method to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified method is overriding a method from a base class; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOverriding(this MethodInfo methodInfo)
    {
        _ = Guard.NotNull(methodInfo);
        return methodInfo.DeclaringType != methodInfo.GetBaseDefinition().DeclaringType;
    }

    /// <summary>
    /// Determines whether this property is overriding some property from a base class. See <see href="https://stackoverflow.com/questions/5746447/determine-whether-a-c-sharp-method-has-keyword-override-using-reflection"/>.
    /// </summary>
    /// <param name="propertyInfo">The property to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified property is overriding a property from a base class; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">The property does not have a setter nor a getter. - <paramref name="propertyInfo"/>.</exception>
    public static bool IsOverriding(this PropertyInfo propertyInfo)
    {
        _ = Guard.NotNull(propertyInfo);
        return IsOverriding(propertyInfo.GetMethod ?? propertyInfo.SetMethod ?? throw new ArgumentException($"The property does not have a setter nor a getter.", nameof(propertyInfo)));
    }

    /// <summary>
    /// Determines whether this event is overriding some event from a base class. See <see href="https://stackoverflow.com/questions/5746447/determine-whether-a-c-sharp-method-has-keyword-override-using-reflection"/>.
    /// </summary>
    /// <param name="eventInfo">The event to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified event is overriding an event from a base class; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">The event does not have an add method nor a remove method. - <paramref name="eventInfo"/>.</exception>
    public static bool IsOverriding(this EventInfo eventInfo)
    {
        _ = Guard.NotNull(eventInfo);
        return IsOverriding(eventInfo.AddMethod ?? eventInfo.RemoveMethod ?? throw new ArgumentException($"The event does not have an add method nor a remove method.", nameof(eventInfo)));
    }

    /// <summary>
    /// Determines whether this method is hiding some method from a base class. See <see href="https://stackoverflow.com/questions/5746447/determine-whether-a-c-sharp-method-has-keyword-override-using-reflection"/>.
    /// </summary>
    /// <param name="methodInfo">The method to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified method is hiding a method from a base class; otherwise, <c>false</c>.
    /// </returns>
    [RequiresUnreferencedCode("Accesses all methods from the base type of the type this method is declared on.")]
    public static bool IsHiding(this MethodInfo methodInfo)
    {
        _ = Guard.NotNull(methodInfo);
        if (methodInfo.DeclaringType != methodInfo.GetBaseDefinition().DeclaringType)
            return false;

        var baseType = methodInfo.DeclaringType?.BaseType;
        if (baseType == null)
            return false;

        MethodInfo? hiddenBaseMethodInfo = null;
        var methods = baseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Static);
        foreach (var method in methods)
        {
            if (method.Name == methodInfo.Name)
            {
                var methodParams = method.GetParameters();
                var methodInfoParams = methodInfo.GetParameters();
                if (methodParams.Length == methodInfoParams.Length)
                {
                    var i = 0;
                    for (; i < methodParams.Length; i++)
                    {
                        if (methodParams[i].ParameterType != methodInfoParams[i].ParameterType
                            || (methodParams[i].Attributes ^ methodInfoParams[i].Attributes).HasFlag(ParameterAttributes.Out))
                        {
                            break;
                        }
                    }

                    if (i == methodParams.Length)
                    {
                        hiddenBaseMethodInfo = method;
                        break;
                    }
                }
            }
        }

        return hiddenBaseMethodInfo != null && !hiddenBaseMethodInfo.IsPrivate;
    }

    /// <summary>
    /// Determines whether this property is hiding some property from a base class. See <see href="https://stackoverflow.com/questions/5746447/determine-whether-a-c-sharp-method-has-keyword-override-using-reflection"/>.
    /// </summary>
    /// <param name="propertyInfo">The property to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified property is hiding a property from a base class; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">The property does not have a setter nor a getter. - <paramref name="propertyInfo"/>.</exception>
    [RequiresUnreferencedCode("Calls IsHiding(MethodInfo) for either the SetMethod or GetMethod of this property.")]
    public static bool IsHiding(this PropertyInfo propertyInfo)
    {
        _ = Guard.NotNull(propertyInfo);
        return IsHiding(propertyInfo.GetMethod ?? propertyInfo.SetMethod ?? throw new ArgumentException($"The property does not have a setter nor a getter.", nameof(propertyInfo)));
    }

    /// <summary>
    /// Determines whether this event is hiding some event from a base class. See <see href="https://stackoverflow.com/questions/5746447/determine-whether-a-c-sharp-method-has-keyword-override-using-reflection"/>.
    /// </summary>
    /// <param name="eventInfo">The event to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified event is hiding an event from a base class; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">The event does not have an add method nor a remove method. - <paramref name="eventInfo"/>.</exception>
    [RequiresUnreferencedCode("Calls IsHiding(MethodInfo) for either the AddMethod or RemoveMethod of this event.")]
    public static bool IsHiding(this EventInfo eventInfo)
    {
        _ = Guard.NotNull(eventInfo);
        return IsHiding(eventInfo.AddMethod ?? eventInfo.RemoveMethod ?? throw new ArgumentException($"The event does not have an add method nor a remove method.", nameof(eventInfo)));
    }

    /// <summary>
    /// Determines whether this field is hiding some field from a base class. See <see href="https://stackoverflow.com/questions/5746447/determine-whether-a-c-sharp-method-has-keyword-override-using-reflection"/>.
    /// </summary>
    /// <param name="fieldInfo">The field to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified field is hiding a field from a base class; otherwise, <c>false</c>.
    /// </returns>
    [RequiresUnreferencedCode("Accesses all fields of the base type of the type this field is declared on.")]
    public static bool IsHiding(this FieldInfo fieldInfo)
    {
        _ = Guard.NotNull(fieldInfo);
        var baseType = fieldInfo.DeclaringType?.BaseType;
        if (baseType == null)
            return false;

        var hiddenBaseFieldInfo = baseType.GetField(fieldInfo.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Static);
        return hiddenBaseFieldInfo != null && !hiddenBaseFieldInfo.IsPrivate;
    }

    /// <summary>
    /// Determines whether this method is hidden in the specified type.
    /// </summary>
    /// <param name="methodInfo">The method to check.</param>
    /// <param name="type">The type to check if the <paramref name="methodInfo"/> is hidden.</param>
    /// <returns>
    ///   <c>true</c> if the specified method is hidden in the specified type; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsHiddenInType(
        this MethodInfo methodInfo,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        Type type)
    {
        _ = Guard.NotNull(methodInfo);
        _ = Guard.NotNull(type);

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Static);
        foreach (var method in methods)
        {
            if (method.Name == methodInfo.Name && method != methodInfo && methodInfo.DeclaringType?.IsAssignableFrom(method.DeclaringType) == true)
            {
                var methodParams = method.GetParameters();
                var methodInfoParams = methodInfo.GetParameters();
                if (methodParams.Length == methodInfoParams.Length)
                {
                    var i = 0;
                    for (; i < methodParams.Length; i++)
                    {
                        if (methodParams[i].ParameterType != methodInfoParams[i].ParameterType
                            || (methodParams[i].Attributes ^ methodInfoParams[i].Attributes).HasFlag(ParameterAttributes.Out))
                        {
                            break;
                        }
                    }

                    if (i == methodParams.Length)
                        return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Determines whether this property is hidden in the specified type.
    /// </summary>
    /// <param name="propertyInfo">The property to cehck.</param>
    /// <param name="type">The type to check if the <paramref name="propertyInfo"/> is hidden.</param>
    /// <returns>
    ///   <c>true</c> if the specified property is hidden in the specified type; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">The property does not have a setter nor a getter. - <paramref name="propertyInfo"/>.</exception>
    public static bool IsHiddenInType(
        this PropertyInfo propertyInfo,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        Type type)
    {
        _ = Guard.NotNull(propertyInfo);
        _ = Guard.NotNull(type);
        return IsHiddenInType(propertyInfo.GetMethod ?? propertyInfo.SetMethod ?? throw new ArgumentException($"The property does not have a setter nor a getter.", nameof(propertyInfo)), type);
    }

    /// <summary>
    /// Determines whether this event is hidden in the specified type.
    /// </summary>
    /// <param name="eventInfo">The event to check.</param>
    /// <param name="type">The type to check if the <paramref name="eventInfo"/> is hidden.</param>
    /// <returns>
    ///   <c>true</c> if the specified event is hidden in the specified type; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">The event does not have an add method nor a remove method. - <paramref name="eventInfo"/>.</exception>
    public static bool IsHiddenInType(
        this EventInfo eventInfo,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        Type type)
    {
        _ = Guard.NotNull(eventInfo);
        _ = Guard.NotNull(type);
        return IsHiddenInType(eventInfo.AddMethod ?? eventInfo.RemoveMethod ?? throw new ArgumentException($"The event does not have an add method nor a remove method.", nameof(eventInfo)), type);
    }

    /// <summary>
    /// Determines whether this field is hidden in the specified type.
    /// </summary>
    /// <param name="fieldInfo">The field to check.</param>
    /// <param name="type">The type to check if the <paramref name="fieldInfo"/> is hidden.</param>
    /// <returns>
    ///   <c>true</c> if the specified field is hidden in the specified type; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsHiddenInType(
        this FieldInfo fieldInfo,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
        Type type)
    {
        _ = Guard.NotNull(fieldInfo);
        _ = Guard.NotNull(type);

        return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Static)
            .Any(x => x.Name == fieldInfo.Name && x != fieldInfo && fieldInfo.DeclaringType?.IsAssignableFrom(x.DeclaringType) == true);
    }

    /// <summary>
    /// Gets a method recursively though the base types of this type.
    /// </summary>
    /// <param name="t">The type to get the method from.</param>
    /// <param name="name">The name of the method.</param>
    /// <param name="bindingFlags">The binding flags describing the method.</param>
    /// <param name="types">The parameter types of the method.</param>
    /// <returns>The method that matches the specified filters if found in any base types of <paramref name="t"/>; otherwise, <see langword="null"/>.</returns>
    [SuppressMessage("ReflectionAnalyzers.SystemReflection", "REFL045:These flags are insufficient to match any members.", Justification = "False positive.")]
    [RequiresUnreferencedCode("Accesses recursively all base types of parameter t.")]
    public static MethodInfo? GetMethodRecursive(this Type? t, string name, BindingFlags bindingFlags, params Type[] types)
    {
        _ = Guard.NotNullOrEmpty(name);
        _ = Guard.NotNull(types);
        var currentType = t;
        MethodInfo? result = null;
        while (result == null && currentType != null)
        {
            result = currentType.GetMethod(name, bindingFlags, null, types, null);
            currentType = currentType.BaseType;
        }

        return result;
    }

    /// <summary>
    /// Queries all base types recursively.
    /// </summary>
    /// <typeparam name="T">The element type to return.</typeparam>
    /// <param name="t">The type to query.</param>
    /// <param name="func">The function to get results from.</param>
    /// <returns>A combined <see cref="IEnumerable{T}"/> of all results of <paramref name="func"/>.</returns>
    [RequiresUnreferencedCode("Accesses recursively all interfaceas of parameter t.")]
    public static IEnumerable<T> QueryTypesRecursive<T>(this Type t, Func<Type, IEnumerable<T>> func)
    {
        _ = Guard.NotNull(t);
        _ = Guard.NotNull(func);
        var result = func(t);
        if (t.BaseType != null)
            result = result.Concat(QueryTypesRecursive(t.BaseType, func));
        result = result.Concat(t.GetInterfaces().SelectMany(x => QueryTypesRecursive(x, func)));
        return result;
    }

    /// <summary>
    /// Gets the type name without generic artiy.
    /// </summary>
    /// <param name="t">The type to get the name of.</param>
    /// <returns>The name of the type <paramref name="t"/> with any generic information.</returns>
    public static string GetTypeNameWithoutGenericArtiy(this Type t)
    {
        _ = Guard.NotNull(t);
        string name = t.Name;
        int index = name.IndexOf('`');
        return index == -1 ? name : name[..index];
    }

    /// <summary>
    /// Gets the default value of the specified type.
    /// </summary>
    /// <param name="type">The type to get the default of.</param>
    /// <returns>The default value of <paramref name="type"/>.</returns>
    public static object? GetDefault([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] this Type type)
    {
        _ = Guard.NotNull(type);
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }

        return null;
    }

    /// <summary>
    /// Returns the result of the <see cref="Type.GetElementType()"/> method if the <see cref="Type.IsByRef"/> property is set to true; otherwise the type itself.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>If <see cref="Type.IsByRef"/> is <c>true</c> the result of the <see cref="Type.GetElementType()"/> method is returned; otherwise, the value of <paramref name="type"/>.</returns>
    public static Type GetElementTypeOrSelf(this Type type)
    {
        return type.IsByRef ? type.GetElementType() ?? type : type;
    }

    /// <summary>
    /// Gets the C# representation of this type.
    /// </summary>
    /// <param name="type">The type to get the C# representation of.</param>
    /// <returns>The C# representation of <paramref name="type"/>.</returns>
    public static string? GetCSharpRepresentation(this Type? type)
    {
        return GetCSharpRepresentation(type, false, true);
    }

    /// <summary>
    /// Gets the C# representation of this type.
    /// </summary>
    /// <param name="type">The type to get the C# representation of.</param>
    /// <param name="forceGlobal">Determines wether to force the global notation.</param>
    /// <returns>The C# representation of <paramref name="type"/>.</returns>
    public static string? GetCSharpRepresentation(this Type? type, bool forceGlobal)
    {
        return GetCSharpRepresentation(type, forceGlobal, true);
    }

    /// <summary>
    /// Gets the C# representation of this type.
    /// </summary>
    /// <param name="type">The type to get the C# representation of.</param>
    /// <param name="forceGlobal">Determines wether to force the global notation.</param>
    /// <param name="addGenericArgs">Determines wether to add generic arguments.</param>
    /// <returns>The C# representation of <paramref name="type"/>.</returns>
    public static string? GetCSharpRepresentation(this Type? type, bool forceGlobal, bool addGenericArgs)
    {
        if (type == null)
            return null;

        var realType = type.GetElementTypeOrSelf();
        if (realType.IsArray)
            return GetCSharpRepresentation(realType.GetElementType(), forceGlobal) + "[]";
        if (realType.IsPointer)
            return GetCSharpRepresentation(realType.GetElementType(), forceGlobal) + "*";
        if (realType.IsGenericType && realType.GetGenericTypeDefinition() == typeof(Nullable<>))
            return GetCSharpRepresentation(realType.GetGenericArguments()[0], forceGlobal) + "?";
        var genericArgs = realType.GetGenericArguments();
        if (realType.IsGenericType && !realType.IsGenericTypeDefinition)
            realType = realType.GetGenericTypeDefinition();

        string result;
        if (_specialTypeCSharpRepresentations.TryGetValue(realType, out string? csharpRepresentation))
            result = csharpRepresentation;
        else
            result = (forceGlobal && !realType.IsGenericParameter ? "global::" : string.Empty) + realType.FullName;

        if (genericArgs.Length > 0 && addGenericArgs)
            result = Regex.Replace(result, @"\<.*\>", GetCSharpRepresentation(genericArgs, forceGlobal));

        return result.Replace("&", string.Empty);
    }

    /// <summary>
    /// Gets the C# representation of these generic arguments.
    /// </summary>
    /// <param name="genericArguments">The generic arguments to get the C# representation of.</param>
    /// <param name="forceGlobal">Determines wether to force the global notation.</param>
    /// <returns>The C# representation of <paramref name="genericArguments"/>.</returns>
    public static string GetCSharpRepresentation(this IEnumerable<Type>? genericArguments, bool forceGlobal = false)
    {
        var ga = genericArguments?.ToArray();
        if (ga != null && ga.Any())
            return $"<{string.Join(", ", ga.Select(x => GetCSharpRepresentation(x, forceGlobal)))}>";
        return string.Empty;
    }

    /// <summary>
    /// Flattenss the type hierarchy of the given type.
    /// </summary>
    /// <param name="type">The type from which to get the flattened type hierarchy.</param>
    /// <returns>An enumerable that iterates through the flattened type hierarchy.</returns>
    public static IEnumerable<Type> FlattenHierarchy([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] this Type? type)
    {
        if (type == null)
        {
            yield break;
        }

        yield return type;
        foreach (var @interface in type.GetTypeInfo().GetInterfaces())
        {
            yield return @interface;
        }

        foreach (var @interface in FlattenHierarchy(type.GetTypeInfo().BaseType))
        {
            yield return @interface;
        }
    }

    private static bool HasCastDefined(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        this Type from,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        Type to,
        bool implicitly)
    {
        _ = Guard.NotNull(from);
        _ = Guard.NotNull(to);
        if ((from.IsPrimitive || from.IsEnum) && (to.IsPrimitive || to.IsEnum))
        {
            if (!implicitly)
                return from == to || (from != typeof(bool) && to != typeof(bool));

            Type[][] typeHierarchy =
            {
                new[] { typeof(byte),  typeof(sbyte), typeof(char) },
                new[] { typeof(short), typeof(ushort) },
                new[] { typeof(int), typeof(uint) },
                new[] { typeof(long), typeof(ulong) },
                new[] { typeof(float) },
                new[] { typeof(double) },
            };
            var lowerTypes = Enumerable.Empty<Type>();
            foreach (var types in typeHierarchy)
            {
                if (types.Any(t => t == to))
                    return lowerTypes.Any(t => t == from);
                lowerTypes = lowerTypes.Concat(types);
            }

            return false;   // IntPtr, UIntPtr, Enum, Boolean
        }

        return IsCastDefined(to, m => m.GetParameters()[0].ParameterType, _ => from, implicitly, false)
            || IsCastDefined(from, _ => to, m => m.ReturnType, implicitly, true);
    }

    private static bool IsCastDefined(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        IReflect type,
        Func<MethodInfo, Type> baseType,
        Func<MethodInfo, Type> derivedType,
        bool implicitly,
        bool lookInBase)
    {
        _ = Guard.NotNull(type);
        _ = Guard.NotNull(baseType);
        _ = Guard.NotNull(derivedType);
        var bindinFlags = BindingFlags.Public | BindingFlags.Static
                        | (lookInBase ? BindingFlags.FlattenHierarchy : BindingFlags.DeclaredOnly);
        return type.GetMethods(bindinFlags).Any(
            m => (m.Name == "op_Implicit" || (!implicitly && m.Name == "op_Explicit"))
                && baseType(m).IsAssignableFrom(derivedType(m)));
    }
}
