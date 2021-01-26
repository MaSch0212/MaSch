using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Win10
{
    [MarkupExtensionReturnType(typeof(Icon))]
    public class Win10IconExtension : MarkupExtension
    {
        public static readonly FontFamily FontFamily;

        [ConstructorArgument("symbol")]
        public Win10IconCode Symbol { get; set; }
        public double FontSize { get; set; } = 12;
        public Stretch Stretch { get; set; } = Stretch.Uniform;

        static Win10IconExtension()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                FontFamily = new FontFamily("Segoe MDL2 Assets");
            else
            {
                FontFamily = Application.Current != null
                    ? new FontFamily(new Uri("pack://application:,,,/MaSch.Presentation.Wpf.Win10;component/"), "./#Segoe MDL2 Assets")
                    : new FontFamily("Segoe MDL2 Assets");
            }
        }

        public Win10IconExtension() : this(Win10IconCode.Close) { }
        public Win10IconExtension(Win10IconCode symbol) { Symbol = symbol; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (pvt?.TargetObject is Setter)
                return this;
            return new Icon(FontFamily, Symbol.GetChar(), FontSize, Stretch);
        }
    }
}
