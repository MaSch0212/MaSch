using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MaSch.Presentation.Translation.Validation
{
    /// <summary>
    /// Validates the property with a regular expression.
    /// </summary>
    /// <seealso cref="TranslatableValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class RegexAttribute : TranslatableValidationAttribute
    {
        #region Properties

        /// <summary>
        /// Gets the regular expression to use for validation.
        /// </summary>
        /// <value>
        /// The regular expression that is used for validation.
        /// </value>
        public Regex Regex { get; }

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexAttribute"/> class.
        /// </summary>
        /// <param name="regexPattern">The regular expression pattern to use for validation.</param>
        public RegexAttribute(string regexPattern)
        {
            Regex = new Regex(regexPattern);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexAttribute"/> class.
        /// </summary>
        /// <param name="regexPattern">The regular expression pattern to use for validation.</param>
        /// <param name="regexOptions">The options for the regular expression engine.</param>
        public RegexAttribute(string regexPattern, RegexOptions regexOptions)
        {
            Regex = new Regex(regexPattern, regexOptions);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Validates the specified value by using the regular expression defined in <see cref="Regex"/>.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResult"></see> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var sValue = value as string;
            if (sValue == null)
                return ValidationResult.Success;
            return Regex.IsMatch(sValue) ? ValidationResult.Success : new ValidationResult(GetTranslatedErrorMessage());
        }

        #endregion
    }
}