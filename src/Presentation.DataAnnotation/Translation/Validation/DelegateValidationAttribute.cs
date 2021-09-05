using MaSch.Core.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MaSch.Presentation.Translation.Validation
{
    /// <summary>
    /// Validates the property with a given method.
    /// </summary>
    /// <seealso cref="TranslatableValidationAttribute" />
    [SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "Needed to access members that use this attribute.")]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DelegateValidationAttribute : TranslatableValidationAttribute
    {
        /// <summary>
        /// The failed result.
        /// </summary>
        public static readonly ValidationResult FailedResult = new(string.Empty);

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateValidationAttribute"/> class.
        /// </summary>
        /// <param name="methodName">The name of the method to use. The method head needs to look like this: <see cref="ValidationResult"/> MyValidation(<see cref="object"/> value);
        /// (can be private or public as well as static or non static).</param>
        public DelegateValidationAttribute(string methodName)
        {
            MethodName = methodName;
        }

        /// <summary>
        /// Gets the name of the validation method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        public string MethodName { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the validation method returns a resource key
        /// which should be translated to the current language.
        /// </summary>
        /// <value>
        /// <c>true</c> if the validation method returns a resource key which will be translated on validation;
        /// otherwise, <c>false</c>.
        /// </value>
        public bool MethodReturnsErrorMessageResourceKey { get; set; } = true;

        /// <summary>
        /// Gets or sets the type in which the method with the name set in <see cref="MethodName"/> is contained in.
        /// </summary>
        /// <value>
        /// The type that contains the method with the name set in <see cref="MethodName"/>.
        /// </value>
        public Type? ContainingType { get; set; }

        /// <summary>
        /// Validates the given value using the given validation method.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResult"></see> class.
        /// </returns>
        /// <exception cref="MissingMethodException">
        /// The method with name <see cref="MethodName"/> could not be found.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The Method <see cref="MethodName"/> has to have a return type of <see cref="ValidationResult"/> or a derived type.
        /// </exception>
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
                throw new MissingMethodException($"The method with name \"{MethodName}\" could not be found.");
            if (!typeof(ValidationResult).IsAssignableFrom(methodInfo.ReturnType))
            {
                throw new NotSupportedException(
                    $"The Method \"{MethodName}\" has to have a return type of \"ValidationResult\" or a derived type.");
            }

            var result =
                methodInfo.Invoke(methodInfo.IsStatic ? null : validationContext.ObjectInstance, new[] { value }) as
                    ValidationResult;
            if (ReferenceEquals(result, FailedResult))
                result = new ValidationResult(GetTranslatedErrorMessage());
            else if (MethodReturnsErrorMessageResourceKey && result != null && result != ValidationResult.Success && result.ErrorMessage != null)
                result.ErrorMessage = GetTranslatedErrorMessage(result.ErrorMessage);
            return result;
        }
    }
}