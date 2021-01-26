using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.MaterialDesign
{
    [MarkupExtensionReturnType(typeof(Icon))]
    public class MaterialDesignIconExtension : MarkupExtension
    {
        [ConstructorArgument("icon")]
        public MaterialDesignIconCode Icon { get; set; }
        public double? FontSize { get; set; }
        public Stretch? Stretch { get; set; }

        public MaterialDesignIconExtension() { Icon = MaterialDesignIconCode.CheckboxBlankOutline; }
        public MaterialDesignIconExtension(MaterialDesignIconCode icon) { Icon = icon; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (pvt?.TargetObject is Setter)
                return this;
            return new MaterialDesignIcon(Icon, Stretch, FontSize);
        }
    }
}
