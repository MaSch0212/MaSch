using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    /// <summary>
    /// Represents an item group in a <see cref="SplitView"/>.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.Views.SplitView.SplitViewItemBase" />
    [ContentProperty(nameof(Children))]
    public class SplitViewItemGroup : SplitViewItemBase
    {
        /// <summary>
        /// Dependency property. Gets or sets the children.
        /// </summary>
        public static readonly DependencyProperty ChildrenProperty =
            DependencyProperty.Register("Children", typeof(IList), typeof(SplitViewItemGroup), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        public IList Children
        {
            get { return (IList)GetValue(ChildrenProperty); }
            set { SetValue(ChildrenProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitViewItemGroup"/> class.
        /// </summary>
        public SplitViewItemGroup()
        {
            Children = new ObservableCollection<SplitViewItemBase>();
            IsSelectable = false;
        }
    }
}
