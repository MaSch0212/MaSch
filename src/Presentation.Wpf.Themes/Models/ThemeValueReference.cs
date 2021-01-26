using MaSch.Core;
using MaSch.Presentation.Wpf.Themes;
using System;

namespace MaSch.Presentation.Wpf.Models
{
    public class ThemeValueReference : ICloneable
    {
        public ThemeKey Key
        {
            get => (ThemeKey)Enum.Parse(typeof(ThemeKey), CustomKey);
            set => CustomKey = value.ToString();
        }

        public string CustomKey { get; set; }
        public string Property { get; set; }

        public ThemeValueReference() { }
        public ThemeValueReference(string key) : this(key, null) { }
        public ThemeValueReference(string key, string property)
        {
            Guard.NotNullOrEmpty(key, nameof(key));

            CustomKey = key;
            Property = property;
        }

        public override bool Equals(object obj) => obj is ThemeValueReference other && other.CustomKey == CustomKey && other.Property == Property;
        public override int GetHashCode() => (CustomKey, Property).GetHashCode();

        public object Clone()
        {
            return new ThemeValueReference(CustomKey, Property);
        }
    }
}
