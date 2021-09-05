using System;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    /// <summary>
    /// When applied to a property inside a <see cref="IThemeValue"/> class, the raw property is linked with the actual value.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class ThemeValueParsedPropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeValueParsedPropertyAttribute"/> class.
        /// </summary>
        /// <param name="rawPropertyName">Name of the raw property.</param>
        public ThemeValueParsedPropertyAttribute(string rawPropertyName)
        {
            RawPropertyName = rawPropertyName;
        }

        /// <summary>
        /// Gets the name of the raw property.
        /// </summary>
        public string RawPropertyName { get; }
    }
}
