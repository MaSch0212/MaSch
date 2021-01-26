using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Markup
{
    public class BindingResourceExtension : StaticResourceExtension
    {
        public BindingResourceExtension() : base() { }

        public BindingResourceExtension(object resourceKey) : base(resourceKey) { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (base.ProvideValue(serviceProvider) is BindingBase binding)
                return binding.ProvideValue(serviceProvider);
            return null;
        }
    }
}
