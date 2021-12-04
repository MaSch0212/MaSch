using System.Windows;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Markup;

/// <summary>
/// Copied from http://www.thomaslevesque.com/2009/07/28/wpf-a-markup-extension-that-can-update-its-target/.
/// </summary>
public abstract class UpdateableMarkupExtension : MarkupExtension
{
    /// <summary>
    /// Gets the target object.
    /// </summary>
    /// <value>
    /// The target object.
    /// </value>
    protected object? TargetObject { get; private set; }

    /// <summary>
    /// Gets the target property.
    /// </summary>
    /// <value>
    /// The target property.
    /// </value>
    protected object? TargetProperty { get; private set; }

    /// <summary>
    /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
    /// </summary>
    /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
    /// <returns>
    /// The object value to set on the property where the extension is applied.
    /// </returns>
    public sealed override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget target)
        {
            TargetObject = target.TargetObject;
            TargetProperty = target.TargetProperty;
            if (target.TargetObject is Setter)
                return this;
        }

        return ProvideValueInternal(serviceProvider);
    }

    /// <summary>
    /// Updates the value.
    /// </summary>
    /// <param name="value">The value.</param>
    protected void UpdateValue(object value)
    {
        if (TargetObject != null)
        {
            if (TargetProperty is DependencyProperty propDp)
            {
                var obj = TargetObject as DependencyObject;

                void UpdateAction() => obj.SetValue(propDp, value);

                // Check whether the target object can be accessed from the
                // current thread, and use Dispatcher.Invoke if it can't
                if (obj?.CheckAccess() ?? false)
                    UpdateAction();
                else
                    obj?.Dispatcher?.Invoke(UpdateAction);
            }

            // targetProperty is PropertyInfo
            else
            {
                var prop = TargetProperty as PropertyInfo;
                prop?.SetValue(TargetObject, value, null);
            }
        }
    }

    /// <summary>
    /// Provides the value internal.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>The value.</returns>
    protected abstract object ProvideValueInternal(IServiceProvider serviceProvider);
}
