using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.ComponentModel;

namespace MaSch.Presentation.Avalonia.Markup
{
    /// <summary>
    /// A <see cref="MarkupExtension"/> that creates an <see cref="Icon"/> using a geometry.
    /// </summary>
    /// <seealso cref="MarkupExtension" />
    public class GeometryIconExtension : MarkupExtension
    {
        /// <summary>
        /// Gets or sets the geometry to use.
        /// </summary>
        [ConstructorArgument("geometry")]
        public Geometry? Geometry { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the geometry is filled.
        /// </summary>
        [ConstructorArgument("filled")]
        public bool Filled { get; set; } = true;

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness { get; set; } = 0;

        /// <summary>
        /// Gets or sets the stretchmode.
        /// </summary>
        public Stretch Stretch { get; set; } = Stretch.Uniform;

        static GeometryIconExtension()
        {
            TypeDescriptor.AddAttributes(typeof(Geometry), new TypeConverterAttribute(typeof(TypeConverter)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryIconExtension"/> class.
        /// </summary>
        public GeometryIconExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryIconExtension"/> class.
        /// </summary>
        /// <param name="geometry">The geometry to use.</param>
        public GeometryIconExtension(Geometry geometry)
            : this(geometry, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryIconExtension"/> class.
        /// </summary>
        /// <param name="geometry">The geometry to use.</param>
        /// <param name="filled">if set to <c>true</c> the geometry will be filled.</param>
        public GeometryIconExtension(Geometry geometry, bool filled)
        {
            Geometry = geometry;
            Filled = filled;
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
            var pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (pvt?.TargetObject is Setter)
                return this;
            return new Icon(Geometry, Filled, StrokeThickness, Stretch);
        }
    }
}
