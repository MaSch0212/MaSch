using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.TemplateSelectors
{
    // https://stackoverflow.com/questions/4672867/can-i-use-a-different-template-for-the-selected-item-in-a-wpf-combobox-than-for#33421573

    /// <summary>
    /// A <see cref="DataTemplateSelector"/> that select templates for a <see cref="ComboBox"/>.
    /// </summary>
    /// <seealso cref="DataTemplateSelector" />
    public class ComboBoxTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets the template to use for the selected item.
        /// </summary>
        public DataTemplate? SelectedItemTemplate { get; set; }

        /// <summary>
        /// Gets or sets the template selector to use for the selected item.
        /// </summary>
        public DataTemplateSelector? SelectedItemTemplateSelector { get; set; }

        /// <summary>
        /// Gets or sets the template to use for items in the drop down.
        /// </summary>
        public DataTemplate? DropdownItemsTemplate { get; set; }

        /// <summary>
        /// Gets or sets the template selector to use for items in the drop down.
        /// </summary>
        public DataTemplateSelector? DropdownItemsTemplateSelector { get; set; }

        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate" /> based on custom logic.
        /// </summary>
        /// <param name="item">The data object for which to select the template.</param>
        /// <param name="container">The data-bound object.</param>
        /// <returns>
        /// Returns a <see cref="T:System.Windows.DataTemplate" /> or <see langword="null" />. The default value is <see langword="null" />.
        /// </returns>
        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            var parent = container;

            // Search up the visual tree, stopping at either a ComboBox or
            // a ComboBoxItem (or null). This will determine which template to use
            while (parent != null && !(parent is ComboBoxItem) && !(parent is ComboBox))
                parent = VisualTreeHelper.GetParent(parent);

            // If you stopped at a ComboBoxItem, you're in the dropdown
            var inDropDown = parent is ComboBoxItem;

            return inDropDown
                ? DropdownItemsTemplate ?? DropdownItemsTemplateSelector?.SelectTemplate(item, container)
                : SelectedItemTemplate ?? SelectedItemTemplateSelector?.SelectTemplate(item, container);
        }
    }

    /// <summary>
    /// A <see cref="MarkupExtension"/> that creates a <see cref="ComboBoxTemplateSelector"/>.
    /// </summary>
    /// <seealso cref="MarkupExtension" />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Markup extension is related to the template selector.")]
    public class ComboBoxTemplateExtension : MarkupExtension
    {
        /// <summary>
        /// Gets or sets the template to use for the selected item.
        /// </summary>
        public DataTemplate? SelectedItemTemplate { get; set; }

        /// <summary>
        /// Gets or sets the template selector to use for the selected item.
        /// </summary>
        public DataTemplateSelector? SelectedItemTemplateSelector { get; set; }

        /// <summary>
        /// Gets or sets the template to use for items in the drop down.
        /// </summary>
        public DataTemplate? DropdownItemsTemplate { get; set; }

        /// <summary>
        /// Gets or sets the template selector to use for items in the drop down.
        /// </summary>
        public DataTemplateSelector? DropdownItemsTemplateSelector { get; set; }

        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new ComboBoxTemplateSelector()
            {
                SelectedItemTemplate = SelectedItemTemplate,
                SelectedItemTemplateSelector = SelectedItemTemplateSelector,
                DropdownItemsTemplate = DropdownItemsTemplate,
                DropdownItemsTemplateSelector = DropdownItemsTemplateSelector,
            };
        }
    }
}
