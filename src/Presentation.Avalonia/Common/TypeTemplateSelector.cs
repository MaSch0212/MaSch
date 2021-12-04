using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;

namespace MaSch.Presentation.Avalonia.Common;

/// <summary>
/// Implementation of the <see cref="IDataTemplate"/> interface that selects templates by their <see cref="DataTemplate.DataType"/> property.
/// </summary>
/// <seealso cref="IDataTemplate" />
public class TypeTemplateSelector : IDataTemplate
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
    [Content]
    public List<DataTemplate> DataTemplates { get; set; }

    /// <inheritdoc/>
    public bool Match(object data)
    {
        return DataTemplates.Any(x => x.DataType?.IsInstanceOfType(data) != false);
    }

    /// <inheritdoc/>
    public IControl Build(object param)
    {
        return (DataTemplates.FirstOrDefault(x => x.DataType?.IsInstanceOfType(param) == true)
            ?? DataTemplates.First(x => x == null)).Build(param);
    }
}
