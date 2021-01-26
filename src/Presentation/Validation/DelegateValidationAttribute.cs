using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MaSch.Common.Extensions;

namespace MaSch.Presentation.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DelegateValidationAttribute : ValidationAttribute
    {
        #region Properties

        public string MethodName { get; set; }
        public Type ContainingType { get; set; }

        #endregion

        #region Ctor

        public DelegateValidationAttribute(string methodName)
        {
            MethodName = methodName;
        }

        #endregion

        #region Overrides

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
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
                throw new NotSupportedException(
                    $"The Method \"{MethodName}(object)\" has to have a return type of \"ValidationResult\" or a derived type.");

            var result = methodInfo.Invoke(methodInfo.IsStatic ? null : validationContext.ObjectInstance, new[] { value }) as ValidationResult;
            return result;
        }

        #endregion
    }
}
