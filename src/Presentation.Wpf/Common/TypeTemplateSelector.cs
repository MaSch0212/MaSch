using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Common;

/// <summary>
/// Implementation of the <see cref="DataTemplateSelector"/> abstract class that selects templates by their <see cref="DataTemplate.DataType"/> property.
/// </summary>
/// <seealso cref="DataTemplateSelector" />
[ContentProperty(nameof(DataTemplates))]
public class TypeTemplateSelector : DataTemplateSelector
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTemplateSelector"/> class.
    /// </summary>
    public TypeTemplateSelector()
    {
        DataTemplates = new List<DataTemplate>();
    }

    /// <summary>
    /// Gets or sets the data templates.
    /// </summary>
    public List<DataTemplate> DataTemplates { get; set; }

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
        foreach (var dataTemplate in DataTemplates)
        {
            if ((dataTemplate.DataType as Type)?.IsInstanceOfType(item) == true)
                return dataTemplate;
        }

        return DataTemplates.FirstOrDefault(x => x.DataType == null);
    }
}
