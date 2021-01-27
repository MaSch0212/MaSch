using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.MaterialDesign
{
    /// <summary>
    /// Markup extension that creates an <see cref="Wpf.Icon"/> using a material design icon.
    /// </summary>
    /// <seealso cref="MarkupExtension" />
    [MarkupExtensionReturnType(typeof(Icon))]
    public class MaterialDesignIconExtension : MarkupExtension
    {
        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        [ConstructorArgument("icon")]
        public MaterialDesignIconCode Icon { get; set; }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        public double? FontSize { get; set; }

        /// <summary>
        /// Gets or sets the stretch mode.
        /// </summary>
        public Stretch? Stretch { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDesignIconExtension"/> class.
        /// </summary>
        public MaterialDesignIconExtension()
        {
            Icon = MaterialDesignIconCode.CheckboxBlankOutline;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDesignIconExtension"/> class.
        /// </summary>
        /// <param name="icon">The icon to use.</param>
        public MaterialDesignIconExtension(MaterialDesignIconCode icon)
        {
            Icon = icon;
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (pvt?.TargetObject is Setter)
                return this;
            return new MaterialDesignIcon(Icon, Stretch, FontSize);
        }
    }
}
