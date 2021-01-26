using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Markup
{
    [MarkupExtensionReturnType(typeof(Icon))]
    public class GeometryIconExtension : MarkupExtension
    {
        [ConstructorArgument("gemetry")]
        public Geometry Geometry { get; set; }
        [ConstructorArgument("filled")]
        public bool Filled { get; set; } = true;
        public double StrokeThickness { get; set; } = 0;
        public Stretch Stretch { get; set; } = Stretch.Uniform;

        static GeometryIconExtension()
        {
            TypeDescriptor.AddAttributes(typeof(Geometry), new TypeConverterAttribute(typeof(TypeConverter)));
        }

        public GeometryIconExtension() { }
        public GeometryIconExtension(Geometry geometry) : this(geometry, true) { }
        public GeometryIconExtension(Geometry geometry, bool filled)
        {
            Geometry = geometry;
            Filled = filled;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (pvt?.TargetObject is Setter)
                return this;
            return new Icon(Geometry, Filled, StrokeThickness, Stretch);
        }
    }
}
