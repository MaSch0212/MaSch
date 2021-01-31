using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;
using MaSch.Core;

namespace MaSch.Presentation.Wpf.Markup
{
    /// <summary>
    /// A <see cref="MarkupExtension"/> that returns all elements in an enumeration.
    /// </summary>
    /// <seealso cref="System.Windows.Markup.MarkupExtension" />
    public class EnumerationExtension : MarkupExtension
    {
        private Type _enumType;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationExtension"/> class.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        public EnumerationExtension(Type enumType)
        {
            Guard.NotNull(enumType, nameof(enumType));

            EnumType = enumType;
        }

        /// <summary>
        /// Gets the type of the enum.
        /// </summary>
        /// <exception cref="ArgumentException">Type must be an Enum.</exception>
        public Type EnumType
        {
            get => _enumType;
            private set
            {
                if (_enumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                _enumType = value;
            }
        }

        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);

            return (
              from object enumValue in enumValues
              select new EnumerationMember
              {
                  Value = enumValue,
                  Description = GetDescription(enumValue),
              }).ToArray();
        }

        private string GetDescription(object enumValue)
        {
            return EnumType
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() is DescriptionAttribute descriptionAttribute
              ? descriptionAttribute.Description
              : enumValue.ToString();
        }

        /// <summary>
        /// Represents a member of an enumeration.
        /// </summary>
        public class EnumerationMember
        {
            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            public object Value { get; set; }
        }
    }
}
