﻿using MaSch.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace MaSch.Presentation.Validation
{
    /// <summary>
    /// If applied to a property its value is validated to not be null.
    /// </summary>
    /// <seealso cref="ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotNullAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets or sets what value a string can have, so that the validation fails.
        /// </summary>
        public StringNullMode StringNullMode { get; set; } = StringNullMode.IsNull;

        /// <inheritdoc />
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var isError = false;
            if (value == null)
            {
                isError = true;
            }
            else if (value is string sValue)
            {
                isError = StringNullMode == StringNullMode.IsNull ||
                         (StringNullMode == StringNullMode.IsNullOrEmpty && string.IsNullOrEmpty(sValue)) ||
                         (StringNullMode == StringNullMode.IsNullOrWhitespace && string.IsNullOrWhiteSpace(sValue));
            }

            return isError ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
        }
    }
}