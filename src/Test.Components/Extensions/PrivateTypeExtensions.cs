using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

#if NETFRAMEWORK
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace MaSch.Test
{
    /// <summary>
    /// Provides extension methods for the <see cref="PrivateType"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class PrivateTypeExtensions
    {
        /// <summary>
        /// Invokes static member.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the member to InvokeHelper.</param>
        /// <param name="args">Arguements to the invoction.</param>
        /// <returns>Result of invocation.</returns>
        public static T InvokeStatic<T>(this PrivateType pt, string name, params object[] args)
        {
            return (T)pt.InvokeStatic(name, args);
        }

        /// <summary>
        /// Invokes static member.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the member to InvokeHelper.</param>
        /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the method to invoke.</param>
        /// <param name="args">Arguements to the invoction.</param>
        /// <returns>Result of invocation.</returns>
        public static T InvokeStatic<T>(this PrivateType pt, string name, Type[] parameterTypes, object[] args)
        {
            return (T)pt.InvokeStatic(name, parameterTypes, args);
        }

        /// <summary>
        /// Invokes static member.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the member to InvokeHelper.</param>
        /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the method to invoke.</param>
        /// <param name="args">Arguements to the invoction.</param>
        /// <param name="typeArguments">An array of types corresponding to the types of the generic arguments.</param>
        /// <returns>Result of invocation.</returns>
        public static T InvokeStatic<T>(this PrivateType pt, string name, Type[] parameterTypes, object[] args, Type[] typeArguments)
        {
            return (T)pt.InvokeStatic(name, parameterTypes, args, typeArguments);
        }

        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the member.</param>
        /// <param name="args">Arguements to the invocation.</param>
        /// <param name="culture">Culture.</param>
        /// <returns>Result of invocation.</returns>
        public static T InvokeStatic<T>(this PrivateType pt, string name, object[] args, CultureInfo culture)
        {
            return (T)pt.InvokeStatic(name, args, culture);
        }

        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the member.</param>
        /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the method to invoke.</param>
        /// <param name="args">Arguements to the invocation.</param>
        /// <param name="culture">Culture info.</param>
        /// <returns>Result of invocation.</returns>
        public static T InvokeStatic<T>(this PrivateType pt, string name, Type[] parameterTypes, object[] args, CultureInfo culture)
        {
            return (T)pt.InvokeStatic(name, parameterTypes, args, culture);
        }

        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the member.</param>
        /// <param name="bindingFlags">Additional invocation attributes.</param>
        /// <param name="args">Arguements to the invocation.</param>
        /// <returns>Result of invocation.</returns>
        public static T InvokeStatic<T>(this PrivateType pt, string name, BindingFlags bindingFlags, params object[] args)
        {
            return (T)pt.InvokeStatic(name, bindingFlags, args);
        }

        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the member.</param>
        /// <param name="bindingFlags">Additional invocation attributes.</param>
        /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the method to invoke.</param>
        /// <param name="args">Arguements to the invocation.</param>
        /// <returns>Result of invocation.</returns>
        public static T InvokeStatic<T>(this PrivateType pt, string name, BindingFlags bindingFlags, Type[] parameterTypes, object[] args)
        {
            return (T)pt.InvokeStatic(name, bindingFlags, parameterTypes, args);
        }

        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the member.</param>
        /// <param name="bindingFlags">Additional invocation attributes.</param>
        /// <param name="args">Arguements to the invocation.</param>
        /// <param name="culture">Culture.</param>
        /// <returns>Result of invocation.</returns>
        public static T InvokeStatic<T>(this PrivateType pt, string name, BindingFlags bindingFlags, object[] args, CultureInfo culture)
        {
            return (T)pt.InvokeStatic(name, bindingFlags, args, culture);
        }

        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the member.</param>
        /// <param name="bindingFlags">Additional invocation attributes.</param>
        /// /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the method to invoke.</param>
        /// <param name="args">Arguements to the invocation.</param>
        /// <param name="culture">Culture.</param>
        /// <returns>Result of invocation.</returns>
        public static T InvokeStatic<T>(this PrivateType pt, string name, BindingFlags bindingFlags, Type[] parameterTypes, object[] args, CultureInfo culture)
        {
            return (T)pt.InvokeStatic(name, bindingFlags, parameterTypes, args, culture);
        }

        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the member.</param>
        /// <param name="bindingFlags">Additional invocation attributes.</param>
        /// /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the method to invoke.</param>
        /// <param name="args">Arguements to the invocation.</param>
        /// <param name="culture">Culture.</param>
        /// <param name="typeArguments">An array of types corresponding to the types of the generic arguments.</param>
        /// <returns>Result of invocation.</returns>
        public static T InvokeStatic<T>(this PrivateType pt, string name, BindingFlags bindingFlags, Type[] parameterTypes, object[] args, CultureInfo culture, Type[] typeArguments)
        {
            return (T)pt.InvokeStatic(name, bindingFlags, parameterTypes, args, culture, typeArguments);
        }

        /// <summary>
        /// Gets the element in static array.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the array.</param>
        /// <param name="indices">
        /// A one-dimensional array of 32-bit integers that represent the indexes specifying
        /// the position of the element to get. For instance, to access a[10][11] the indices would be {10,11}.
        /// </param>
        /// <returns>element at the specified location.</returns>
        public static T GetStaticArrayElement<T>(this PrivateType pt, string name, params int[] indices)
        {
            return (T)pt.GetStaticArrayElement(name, indices);
        }

        /// <summary>
        /// Gets the element in satatic array.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the array.</param>
        /// <param name="bindingFlags">Additional InvokeHelper attributes.</param>
        /// <param name="indices">
        /// A one-dimensional array of 32-bit integers that represent the indexes specifying
        /// the position of the element to get. For instance, to access a[10][11] the array would be {10,11}.
        /// </param>
        /// <returns>element at the spcified location.</returns>
        public static T GetStaticArrayElement<T>(this PrivateType pt, string name, BindingFlags bindingFlags, params int[] indices)
        {
            return (T)pt.GetStaticArrayElement(name, bindingFlags, indices);
        }

        /// <summary>
        /// Gets the static field.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the field.</param>
        /// <returns>The static field.</returns>
        public static T GetStaticField<T>(this PrivateType pt, string name)
        {
            return (T)pt.GetStaticField(name);
        }

        /// <summary>
        /// Gets the static field using specified InvokeHelper attributes.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the field.</param>
        /// <param name="bindingFlags">Additional invocation attributes.</param>
        /// <returns>The static field.</returns>
        public static T GetStaticField<T>(this PrivateType pt, string name, BindingFlags bindingFlags)
        {
            return (T)pt.GetStaticField(name, bindingFlags);
        }

        /// <summary>
        /// Gets the static field or property.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the field or property.</param>
        /// <returns>The static field or property.</returns>
        public static T GetStaticFieldOrProperty<T>(this PrivateType pt, string name)
        {
            return (T)pt.GetStaticFieldOrProperty(name);
        }

        /// <summary>
        /// Gets the static field or property using specified InvokeHelper attributes.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the field or property.</param>
        /// <param name="bindingFlags">Additional invocation attributes.</param>
        /// <returns>The static field or property.</returns>
        public static T GetStaticFieldOrProperty<T>(this PrivateType pt, string name, BindingFlags bindingFlags)
        {
            return (T)pt.GetStaticFieldOrProperty(name, bindingFlags);
        }

        /// <summary>
        /// Gets the static property.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the field or property.</param>
        /// <param name="args">Arguements to the invocation.</param>
        /// <returns>The static property.</returns>
        public static T GetStaticProperty<T>(this PrivateType pt, string name, params object[] args)
        {
            return (T)pt.GetStaticProperty(name, args);
        }

        /// <summary>
        /// Gets the static property.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the property.</param>
        /// <param name="bindingFlags">Additional invocation attributes.</param>
        /// <param name="args">Arguments to pass to the member to invoke.</param>
        /// <returns>The static property.</returns>
        public static T GetStaticProperty<T>(this PrivateType pt, string name, BindingFlags bindingFlags, params object[] args)
        {
            return (T)pt.GetStaticProperty(name, bindingFlags, args);
        }

        /// <summary>
        /// Gets the static property.
        /// </summary>
        /// <typeparam name="T">The type that the return value should be casted to.</typeparam>
        /// <param name="pt">The private type.</param>
        /// <param name="name">Name of the property.</param>
        /// <param name="bindingFlags">Additional invocation attributes.</param>
        /// <param name="parameterTypes">An array of <see cref="T:System.Type"/> objects representing the number, order, and type of the parameters for the indexed property.</param>
        /// <param name="args">Arguments to pass to the member to invoke.</param>
        /// <returns>The static property.</returns>
        public static T GetStaticProperty<T>(this PrivateType pt, string name, BindingFlags bindingFlags, Type[] parameterTypes, object[] args)
        {
            return (T)pt.GetStaticProperty(name, bindingFlags, parameterTypes, args);
        }
    }
}
