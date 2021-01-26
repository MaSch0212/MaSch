using System;
using MaSch.Core;
using System.ComponentModel.DataAnnotations;

namespace MaSch.Presentation.Translation.Validation
{
    /// <summary>
    /// Validates the property for null values.
    /// </summary>
    /// <seealso cref="TranslatableValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class NotNullAttribute : TranslatableValidationAttribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the validation rule for string values.
        /// </summary>
        /// <value>
        /// The validation rule for string values.
        /// </value>
        public StringNullMode StringNullMode { get; set; } = StringNullMode.IsNull;

        #endregion

        #region Overrides

        /// <summary>
        /// Validates the specified value for not being null.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResult"></see> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isError = false;
            if (value == null)
                isError = true;
            else if (value is string sValue)
            {
                isError = StringNullMode == StringNullMode.IsNull ||
                          StringNullMode == StringNullMode.IsNullOrEmpty && string.IsNullOrEmpty(sValue) ||
                          StringNullMode == StringNullMode.IsNullOrWhitespace && string.IsNullOrWhiteSpace(sValue);
            }

            return isError ? new ValidationResult(GetTranslatedErrorMessage()) : ValidationResult.Success;
        }

        #endregion
    }
}