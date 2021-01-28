using System;
using System.ComponentModel.DataAnnotations;

namespace MaSch.Presentation.Translation.Validation
{
    /// <summary>
    /// Validates the string property for its length.
    /// </summary>
    /// <seealso cref="TranslatableValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class StringLengthAttribute : TranslatableValidationAttribute
    {
        /// <summary>
        /// Gets the error message to return when the string is to long.
        /// </summary>
        /// <value>
        /// The error message returned when the string is to long.
        /// </value>
        public string ErrorMsgToLarge { get; }

        /// <summary>
        /// Gets the error message to return when the string is to short.
        /// </summary>
        /// <value>
        /// The error message returned when the string is to short.
        /// </value>
        public string ErrorMsgToSmall { get; }

        /// <summary>
        /// Gets the exact expected length of the string.
        /// </summary>
        /// <value>
        /// The exact expected length of the string. Returns -1 if min and max lengths are used.
        /// </value>
        public int ExactLength { get; }

        /// <summary>
        /// Gets or sets a value indicating whether an empty string is defined as valid.
        /// </summary>
        /// <value>
        /// <c>true</c> if an empty string is defined as valid; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreNullString { get; set; }

        /// <summary>
        /// Gets the maximum expected length of the string.
        /// </summary>
        /// <value>
        /// The maximum expected length of the string. Returns -1 if an exact length is used.
        /// </value>
        public int MaxLength { get; }

        /// <summary>
        /// Gets the minimum expected length of the string.
        /// </summary>
        /// <value>
        /// The minimum expected length of the string. Returns -1 if an exact length is used.
        /// </value>
        public int MinLength { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringLengthAttribute"/> class.
        /// </summary>
        /// <param name="exactLength">The exact expected length of the string.</param>
        /// <param name="errorMsg">The error message to return if a value is invalid.</param>
        public StringLengthAttribute(int exactLength, string errorMsg)
        {
            ExactLength = exactLength;
            MinLength = MaxLength = -1;
            ErrorMessageResourceName = errorMsg;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringLengthAttribute"/> class.
        /// </summary>
        /// <param name="minLength">The minimum expected length of the string.</param>
        /// <param name="maxLength">The maximum expected length of the string.</param>
        /// <param name="errorMsgToSmall">The error message returned if the string is to short.</param>
        /// <param name="errorMsgToLarge">The error message returned if the string is to long.</param>
        public StringLengthAttribute(int minLength = -1, int maxLength = -1, string errorMsgToSmall = null, string errorMsgToLarge = null)
        {
            ExactLength = -1;
            MinLength = minLength;
            MaxLength = maxLength;
            ErrorMsgToLarge = errorMsgToLarge;
            ErrorMsgToSmall = errorMsgToSmall;
        }

        /// <summary>
        /// Validates the specified value on the specified string length.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResult"></see> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var sValue = value as string;
            if (string.IsNullOrEmpty(sValue) && IgnoreNullString)
                return ValidationResult.Success;
            if (ExactLength >= 0 && (sValue?.Length ?? 0) != ExactLength)
                return new ValidationResult(GetTranslatedErrorMessage());
            if (MinLength >= 0 && (sValue?.Length ?? 0) < MinLength)
                return new ValidationResult(GetTranslatedErrorMessage(ErrorMsgToSmall));
            if (MaxLength >= 0 && (sValue?.Length ?? 0) > MaxLength)
                return new ValidationResult(GetTranslatedErrorMessage(ErrorMsgToLarge));
            return ValidationResult.Success;
        }
    }
}