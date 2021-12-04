using System.Windows;

namespace MaSch.Presentation.Wpf.Views.SplitView;

/// <summary>
/// Represents a page that is used in the <see cref="SplitView"/>.
/// </summary>
public interface IPage
{
    /// <summary>
    /// Gets a value indicating whether this page should be selected by default.
    /// </summary>
    bool IsPageSelectedByDefault { get; }

    /// <summary>
    /// Gets the page group identifier to which this page is assigned.
    /// </summary>
    string PageGroupId { get; }

    /// <summary>
    /// Gets the name of the page.
    /// </summary>
    string PageName { get; }

    /// <summary>
    /// Gets the translation provider key to translate the <see cref="PageName"/>.
    /// </summary>
    string TranslationProviderKey { get; }

    /// <summary>
    /// Gets the page priority.
    /// </summary>
    int PagePriority { get; }

    /// <summary>
    /// Gets the page icon.
    /// </summary>
    IIcon PageIcon { get; }

    /// <summary>
    /// Gets the content of the page.
    /// </summary>
    FrameworkElement PageContent { get; }
}
