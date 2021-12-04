namespace MaSch.Presentation.Wpf.Views.SplitView;

/// <summary>
/// Represents a page group that is used by the <see cref="SplitView"/>.
/// </summary>
public interface IPageGroup
{
    /// <summary>
    /// Gets the page group identifier.
    /// </summary>
    string PageGroupId { get; }

    /// <summary>
    /// Gets the name of the page group.
    /// </summary>
    string PageGroupName { get; }

    /// <summary>
    /// Gets the translation provider key to use to translate the <see cref="PageGroupName"/>.
    /// </summary>
    string TranslationProviderKey { get; }

    /// <summary>
    /// Gets the page group priority.
    /// </summary>
    int PageGroupPriority { get; }
}
