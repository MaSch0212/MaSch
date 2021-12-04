namespace MaSch.Test;

/// <summary>
/// Provides extension methods for the <see cref="PrivateObject"/> class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class PrivateObjectExtensions
{
    /// <summary>
    /// Invokes the specified method.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the method.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <returns>Result of method call.</returns>
    public static T Invoke<T>(this PrivateObject po, string name, params object[] args)
    {
        return (T)po.Invoke(name, args);
    }

    /// <summary>
    /// Invokes the specified method.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the method.</param>
    /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <returns>Result of method call.</returns>
    public static T Invoke<T>(this PrivateObject po, string name, Type[] parameterTypes, object[] args)
    {
        return (T)po.Invoke(name, parameterTypes, args);
    }

    /// <summary>
    /// Invokes the specified method.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the method.</param>
    /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <param name="typeArguments">An array of types corresponding to the types of the generic arguments.</param>
    /// <returns>Result of method call.</returns>
    public static T Invoke<T>(this PrivateObject po, string name, Type[] parameterTypes, object[] args, Type[] typeArguments)
    {
        return (T)po.Invoke(name, parameterTypes, args, typeArguments);
    }

    /// <summary>
    /// Invokes the specified method.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the method.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <param name="culture">Culture info.</param>
    /// <returns>Result of method call.</returns>
    public static T Invoke<T>(this PrivateObject po, string name, object[] args, CultureInfo culture)
    {
        return (T)po.Invoke(name, args, culture);
    }

    /// <summary>
    /// Invokes the specified method.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the method.</param>
    /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <param name="culture">Culture info.</param>
    /// <returns>Result of method call.</returns>
    public static T Invoke<T>(this PrivateObject po, string name, Type[] parameterTypes, object[] args, CultureInfo culture)
    {
        return (T)po.Invoke(name, parameterTypes, args, culture);
    }

    /// <summary>
    /// Invokes the specified method.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the method.</param>
    /// <param name="bindingFlags">A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <returns>Result of method call.</returns>
    public static T Invoke<T>(this PrivateObject po, string name, BindingFlags bindingFlags, params object[] args)
    {
        return (T)po.Invoke(name, bindingFlags, args);
    }

    /// <summary>
    /// Invokes the specified method.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the method.</param>
    /// <param name="bindingFlags">A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
    /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <returns>Result of method call.</returns>
    public static T Invoke<T>(this PrivateObject po, string name, BindingFlags bindingFlags, Type[] parameterTypes, object[] args)
    {
        return (T)po.Invoke(name, bindingFlags, parameterTypes, args);
    }

    /// <summary>
    /// Invokes the specified method.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the method.</param>
    /// <param name="bindingFlags">A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <param name="culture">Culture info.</param>
    /// <returns>Result of method call.</returns>
    public static T Invoke<T>(this PrivateObject po, string name, BindingFlags bindingFlags, object[] args, CultureInfo culture)
    {
        return (T)po.Invoke(name, bindingFlags, args, culture);
    }

    /// <summary>
    /// Invokes the specified method.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the method.</param>
    /// <param name="bindingFlags">A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
    /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <param name="culture">Culture info.</param>
    /// <returns>Result of method call.</returns>
    public static T Invoke<T>(this PrivateObject po, string name, BindingFlags bindingFlags, Type[] parameterTypes, object[] args, CultureInfo culture)
    {
        return (T)po.Invoke(name, bindingFlags, parameterTypes, args, culture);
    }

    /// <summary>
    /// Invokes the specified method.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the method.</param>
    /// <param name="bindingFlags">A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
    /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <param name="culture">Culture info.</param>
    /// <param name="typeArguments">An array of types corresponding to the types of the generic arguments.</param>
    /// <returns>Result of method call.</returns>
    public static T Invoke<T>(this PrivateObject po, string name, BindingFlags bindingFlags, Type[] parameterTypes, object[] args, CultureInfo culture, Type[] typeArguments)
    {
        return (T)po.Invoke(name, bindingFlags, parameterTypes, args, culture, typeArguments);
    }

    /// <summary>
    /// Gets the array element using array of subsrcipts for each dimension.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the member.</param>
    /// <param name="indices">the indices of array.</param>
    /// <returns>An arrya of elements.</returns>
    public static T GetArrayElement<T>(this PrivateObject po, string name, params int[] indices)
    {
        return (T)po.GetArrayElement(name, indices);
    }

    /// <summary>
    /// Gets the array element using array of subsrcipts for each dimension.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the member.</param>
    /// <param name="bindingFlags">A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
    /// <param name="indices">the indices of array.</param>
    /// <returns>An arrya of elements.</returns>
    public static T GetArrayElement<T>(this PrivateObject po, string name, BindingFlags bindingFlags, params int[] indices)
    {
        return (T)po.GetArrayElement(name, bindingFlags, indices);
    }

    /// <summary>
    /// Get the field.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the field.</param>
    /// <returns>The field.</returns>
    public static T GetField<T>(this PrivateObject po, string name)
    {
        return (T)po.GetField(name);
    }

    /// <summary>
    /// Gets the field.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the field.</param>
    /// <param name="bindingFlags">A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
    /// <returns>The field.</returns>
    public static T GetField<T>(this PrivateObject po, string name, BindingFlags bindingFlags)
    {
        return (T)po.GetField(name, bindingFlags);
    }

    /// <summary>
    /// Get the field or property.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the field or property.</param>
    /// <returns>The field or property.</returns>
    public static T GetFieldOrProperty<T>(this PrivateObject po, string name)
    {
        return (T)po.GetFieldOrProperty(name);
    }

    /// <summary>
    /// Gets the field or property.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the field or property.</param>
    /// <param name="bindingFlags">A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
    /// <returns>The field or property.</returns>
    public static T GetFieldOrProperty<T>(this PrivateObject po, string name, BindingFlags bindingFlags)
    {
        return (T)po.GetFieldOrProperty(name, bindingFlags);
    }

    /// <summary>
    /// Gets the property.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the property.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <returns>The property.</returns>
    public static T GetProperty<T>(this PrivateObject po, string name, params object[] args)
    {
        return (T)po.GetProperty(name, args);
    }

    /// <summary>
    /// Gets the property.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the property.</param>
    /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the indexed property.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <returns>The property.</returns>
    public static T GetProperty<T>(this PrivateObject po, string name, Type[] parameterTypes, object[] args)
    {
        return (T)po.GetProperty(name, parameterTypes, args);
    }

    /// <summary>
    /// Gets the property.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the property.</param>
    /// <param name="bindingFlags">A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <returns>The property.</returns>
    public static T GetProperty<T>(this PrivateObject po, string name, BindingFlags bindingFlags, params object[] args)
    {
        return (T)po.GetProperty(name, bindingFlags, args);
    }

    /// <summary>
    /// Gets the property.
    /// </summary>
    /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
    /// <param name="po">The private object.</param>
    /// <param name="name">Name of the property.</param>
    /// <param name="bindingFlags">A bitmask comprised of one or more <see cref="T:System.Reflection.BindingFlags"/> that specify how the search is conducted.</param>
    /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the indexed property.</param>
    /// <param name="args">Arguments to pass to the member to invoke.</param>
    /// <returns>The property.</returns>
    public static T GetProperty<T>(this PrivateObject po, string name, BindingFlags bindingFlags, Type[] parameterTypes, object[] args)
    {
        return (T)po.GetProperty(name, bindingFlags, parameterTypes, args);
    }
}
