using System.Windows;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Views.SplitView;

/// <summary>
/// Represents a split view item (page) in a <see cref="SplitView"/>.
/// </summary>
/// <seealso cref="SplitViewItemBase" />
[ContentProperty(nameof(Content))]
public class SplitViewItem : SplitViewItemBase
{
    /// <summary>
    /// Dependency property. Gets or sets the icon.
    /// </summary>
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(IIcon), typeof(SplitViewItem), new PropertyMetadata(null));

    /// <summary>
    /// Dependency property. Gets or sets the content of the page.
    /// </summary>
    public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(SplitViewItem), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the icon.
    /// </summary>
    public IIcon Icon
    {
        get { return (IIcon)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    /// <summary>
    /// Gets or sets the content of the page.
    /// </summary>
    public FrameworkElement Content
    {
        get { return (FrameworkElement)GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }
}
