using System;

namespace MaSch.Presentation.Wpf.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class WpfIconFontAttribute : Attribute
    {
        public string FontName { get; }
        public string CssFileName { get; }
        public string CssClassPrefix { get; }
        public uint ExtraIconIdStart { get; }
        public string CssClassSuffix { get; set; }
        public uint DefaultIconCode { get; set; }

        public WpfIconFontAttribute(string fontName, string cssFileName, string cssClassPrefix, uint extraIconIdStart)
        {
            FontName = fontName;
            CssFileName = cssFileName;
            CssClassPrefix = cssClassPrefix;
            ExtraIconIdStart = extraIconIdStart;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class WpfIconFontExtraGeometryAttribute : Attribute
    {
        public string Name { get; }
        public string GeometryPath { get; }

        public WpfIconFontExtraGeometryAttribute(string name, string geometryPath)
        {
            Name = name;
            GeometryPath = geometryPath;
        }
    }
}
