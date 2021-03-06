using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Markup
{
    /// <summary>
    /// A <see cref="MarkupExtension"/> that provides the value of a Binding in a static resource.
    /// </summary>
    /// <seealso cref="System.Windows.StaticResourceExtension" />
    public class BindingResourceExtension : StaticResourceExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingResourceExtension"/> class.
        /// </summary>
        public BindingResourceExtension()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingResourceExtension"/> class.
        /// </summary>
        /// <param name="resourceKey">The key of the resource that this markup extension references.</param>
        public BindingResourceExtension(object resourceKey)
            : base(resourceKey)
        {
        }

        /// <summary>
        /// Returns an object that should be set on the property where this extension is applied. For <see cref="T:System.Windows.StaticResourceExtension" />, this is the object found in a resource dictionary, where the object to find is identified by the <see cref="P:System.Windows.StaticResourceExtension.ResourceKey" />.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the markup extension provided value is evaluated.
        /// </returns>
        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            if (base.ProvideValue(serviceProvider) is BindingBase binding)
                return binding.ProvideValue(serviceProvider);
            return null;
        }
    }
}
