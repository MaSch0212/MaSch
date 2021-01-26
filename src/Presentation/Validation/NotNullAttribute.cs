using System.ComponentModel.DataAnnotations;
using MaSch.Core;

namespace MaSch.Presentation.Validation
{
    public class NotNullAttribute : ValidationAttribute
    {
        #region Properties

        public StringNullMode StringNullMode { get; set; } = StringNullMode.IsNull;

        #endregion

        #region Overrides

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isError = false;
            if (value == null)
                isError = true;
            else
            {
                var sValue = value as string;
                if (sValue != null)
                {
                    isError = StringNullMode == StringNullMode.IsNull ||
                              StringNullMode == StringNullMode.IsNullOrEmpty && string.IsNullOrEmpty(sValue) ||
                              StringNullMode == StringNullMode.IsNullOrWhitespace && string.IsNullOrWhiteSpace(sValue);
                }
            }

            return isError ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
        }

        #endregion
    }

}