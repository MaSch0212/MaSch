using System.Windows;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    [ContentProperty(nameof(Content))]
    public class SplitViewItem : SplitViewItemBase
    {
        #region Fields
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(IIcon), typeof(SplitViewItem), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(SplitViewItem), new PropertyMetadata(null));
        #endregion

        #region Prperties
        public IIcon Icon
        {
            get { return (IIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public FrameworkElement Content
        {
            get { return (FrameworkElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        #endregion
    }
}
