using MaSch.Core.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MaSch.Presentation.Validation
{
    /// <summary>
    /// If applied to a property its value is validated by using a delegate function.
    /// </summary>
    /// <seealso cref="ValidationAttribute" />
    [SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "Needed to access members that use this attribute.")]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DelegateValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateValidationAttribute"/> class.
        /// </summary>
        /// <param name="methodName">
        /// Name of the method to use for validation.
        /// This can be a public or non-public instance or static method in the same class as the validation context.
        /// If the <see cref="ContainingType"/> property is set the method is searched in that type, but only static methods are supported then.
        /// The validation method should have one parameter of type <see cref="object"/> and should return an instance of type <see cref="ValidationResult"/>.
        /// </param>
        public DelegateValidationAttribute(string methodName)
        {
            MethodName = methodName;
        }

        /// <summary>
        /// Gets the name of the method that is used for validation.
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// Gets or sets the type in which the method defined by <see cref="MethodName"/> is located in.
        /// </summary>
        public Type? ContainingType { get; set; }

        /// <inheritdoc />
        /// <exception cref="MissingMethodException">The method \"{MethodName}(object)\" could not be found.</exception>
        /// <exception cref="NotSupportedException">The Method \"{MethodName}(object)\" has to have a return type of \"ValidationResult\" or a derived type.</exception>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var type = validationContext.ObjectType;
            var bindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            if (ContainingType != null)
            {
                type = ContainingType;
                bindingFlags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            }

            var methodInfo = type.GetMethodRecursive(MethodName, bindingFlags, typeof(object));

            if (methodInfo == null)
                throw new MissingMethodException($"The method \"{MethodName}(object)\" could not be found.");
            if (!typeof(ValidationResult).IsAssignableFrom(methodInfo.ReturnType))
            {
                throw new NotSupportedException(
                    $"The Method \"{MethodName}(object)\" has to have a return type of \"ValidationResult\" or a derived type.");
            }

            var result = methodInfo.Invoke(methodInfo.IsStatic ? null : validationContext.ObjectInstance, new[] { value }) as ValidationResult;
            return result;
        }
    }
}
