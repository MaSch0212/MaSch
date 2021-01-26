using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    [ContentProperty(nameof(Children))]
    public class SplitViewItemGroup : SplitViewItemBase
    {
        public static readonly DependencyProperty ChildrenProperty =
            DependencyProperty.Register("Children", typeof(IList), typeof(SplitViewItemGroup), new PropertyMetadata(null));

        public IList Children
        {
            get { return (IList)GetValue(ChildrenProperty); }
            set { SetValue(ChildrenProperty, value); }
        }

        public SplitViewItemGroup()
        {
            Children = new ObservableCollection<SplitViewItemBase>();
            IsSelectable = false;
        }
    }
}
