using System;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ThemeValueParsedPropertyAttribute : Attribute
    {
        public string RawPropertyName { get; }

        public ThemeValueParsedPropertyAttribute(string rawPropertyName)
        {
            RawPropertyName = rawPropertyName;
        }
    }
}
