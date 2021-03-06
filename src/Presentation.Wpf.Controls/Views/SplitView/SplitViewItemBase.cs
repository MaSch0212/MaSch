using System.Windows;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    /// <summary>
    /// Base class for items in a <see cref="SplitView"/>.
    /// </summary>
    /// <seealso cref="System.Windows.DependencyObject" />
    public class SplitViewItemBase : DependencyObject
    {
        /// <summary>
        /// Dependency property. Gets or sets the internal unique name of the item.
        /// </summary>
        public static readonly DependencyProperty InternalNameProperty =
            DependencyProperty.Register("InternalName", typeof(string), typeof(SplitViewItemBase), new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the header.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(SplitViewItemBase), new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether this item is visible.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(SplitViewItemBase), new PropertyMetadata(true));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether this item is selectable.
        /// </summary>
        public static readonly DependencyProperty IsSelectableProperty =
            DependencyProperty.Register("IsSelectable", typeof(bool), typeof(SplitViewItemBase), new PropertyMetadata(true));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether this item is selected.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(SplitViewItemBase), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the internal unique name of the item.
        /// </summary>
        public string InternalName
        {
            get { return (string)GetValue(InternalNameProperty); }
            set { SetValue(InternalNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public string? Header
        {
            get { return (string?)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this item is visible.
        /// </summary>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this item is selectable.
        /// </summary>
        public bool IsSelectable
        {
            get { return (bool)GetValue(IsSelectableProperty); }
            set { SetValue(IsSelectableProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this item is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
    }
}
