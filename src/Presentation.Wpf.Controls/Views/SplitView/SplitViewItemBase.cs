using System.Windows;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    public class SplitViewItemBase : DependencyObject
    {
        #region Fields
        public static readonly DependencyProperty InternalNameProperty =
            DependencyProperty.Register("InternalName", typeof(string), typeof(SplitViewItemBase), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(SplitViewItemBase), new PropertyMetadata(null));
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(SplitViewItemBase), new PropertyMetadata(true));
        public static readonly DependencyProperty IsSelectableProperty =
            DependencyProperty.Register("IsSelectable", typeof(bool), typeof(SplitViewItemBase), new PropertyMetadata(true));
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(SplitViewItemBase), new PropertyMetadata(false));
        #endregion

        #region Properties
        public string InternalName
        {
            get { return (string)GetValue(InternalNameProperty); }
            set { SetValue(InternalNameProperty, value); }
        }
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }
        public bool IsSelectable
        {
            get { return (bool)GetValue(IsSelectableProperty); }
            set { SetValue(IsSelectableProperty, value); }
        }
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        #endregion
    }
}
