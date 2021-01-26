using MaSch.Common;
using MaSch.Common.Extensions;
using MaSch.Presentation.Translation;
using MaSch.Presentation.Wpf.Animation;
using MaSch.Presentation.Wpf.Controls;
using MaSch.Presentation.Wpf.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using ScrollViewer = System.Windows.Controls.ScrollViewer;
using Window = System.Windows.Window;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    [ContentProperty(nameof(ItemSource))]
    [DefaultProperty(nameof(ItemSource))]
    public class SplitView : Control
    {
        #region Private Fields

        private readonly Duration _animationDuration = new Duration(TimeSpan.FromSeconds(0.25));
        private TreeView _treeView;
        private Button _menuButton;
        private ColumnDefinition _buttonsColumn;
        private ContentControl _content;
        private Storyboard _currentStoryboard;
        private ContentPresenter _infoAreaExpandedTop;
        private ContentPresenter _infoAreaExpandedBottom;
        private ContentPresenter _infoAreaCollapsedTop;
        private ContentPresenter _infoAreaCollapsedBottom;
        private ScrollViewer _treeViewScroll;
        private UIElement _indicatorTop;
        private UIElement _indicatorBottom;
        private bool _ignoreNextSwitch;

        private DispatcherTimer _scrollTopTimer;
        private DispatcherTimer _scrollBottomTimer;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource", typeof(IList), typeof(SplitView), new PropertyMetadata(null));
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(SplitView), new PropertyMetadata(true, OnIsExpandedChanged));
        public static readonly DependencyProperty UseAnimationsProperty =
            DependencyProperty.Register("UseAnimations", typeof(bool), typeof(SplitView), new PropertyMetadata(true, OnUseAnimationsChanged));
        public static readonly DependencyProperty MenuButtonContentProperty =
            DependencyProperty.Register("MenuButtonContent", typeof(string), typeof(SplitView), new PropertyMetadata(null));
        public static readonly DependencyProperty MenuInfoAreaExpandedTopProperty =
            DependencyProperty.Register("MenuInfoAreaExpandedTop", typeof(object), typeof(SplitView), new PropertyMetadata(null));
        public static readonly DependencyProperty MenuInfoAreaExpandedBottomProperty =
            DependencyProperty.Register("MenuInfoAreaExpandedBottom", typeof(object), typeof(SplitView), new PropertyMetadata(null));
        public static readonly DependencyProperty MenuInfoAreaCollapsedTopProperty =
            DependencyProperty.Register("MenuInfoAreaCollapsedTop", typeof(object), typeof(SplitView), new PropertyMetadata(null));
        public static readonly DependencyProperty MenuInfoAreaCollapsedBottomProperty =
            DependencyProperty.Register("MenuInfoAreaCollapsedBottom", typeof(object), typeof(SplitView), new PropertyMetadata(null));
        public static readonly DependencyProperty IsLoadingPageProperty =
            DependencyProperty.Register("IsLoadingPage", typeof(bool), typeof(SplitView), new PropertyMetadata(false));
        public static readonly DependencyProperty ShowLoadingPaneProperty =
            DependencyProperty.Register("ShowLoadingPane", typeof(bool), typeof(SplitView), new PropertyMetadata(false));

        public static readonly DependencyProperty TransitionInProperty =
            DependencyProperty.Register("TransitionIn", typeof(TransitionInType), typeof(SplitView), new PropertyMetadata(TransitionInType.None));
        public static readonly DependencyProperty TransitionOutProperty =
            DependencyProperty.Register("TransitionOut", typeof(TransitionOutType), typeof(SplitView), new PropertyMetadata(TransitionOutType.None));
        public static readonly DependencyProperty TransitionDurationProperty =
            DependencyProperty.Register("TransitionDuration", typeof(TimeSpan), typeof(SplitView), new PropertyMetadata(TimeSpan.FromSeconds(0.2)));
        public static readonly DependencyProperty TransitionFirstContentProperty =
            DependencyProperty.Register("TransitionFirstContent", typeof(bool), typeof(SplitView), new PropertyMetadata(true));
        public static readonly DependencyProperty RunAnimationsSimultaneouslyProperty =
            DependencyProperty.Register("RunAnimationsSimultaneously", typeof(bool), typeof(SplitView), new PropertyMetadata(true));
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(SplitView), new PropertyMetadata(null));

        #endregion

        #region Dependecy Property Changed Handlers

        private static void OnIsExpandedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is SplitView view)
            {
                if (view.IsExpanded)
                    view.StartExpandAnimation();
                else
                    view.StartShrinkAnimation();
            }
        }

        private static void OnUseAnimationsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is SplitView view)
            {
                if (!view.UseAnimations)
                    view.StopAnimations();
            }
        }

        #endregion

        #region Properties

        [Bindable(true)]
        public IList ItemSource
        {
            get => (IList)GetValue(ItemSourceProperty);
            set => SetValue(ItemSourceProperty, value);
        }
        public bool IsExpanded
        {
            get => GetValue(IsExpandedProperty) as bool? ?? true;
            set => SetValue(IsExpandedProperty, value);
        }
        public bool UseAnimations
        {
            get => GetValue(UseAnimationsProperty) as bool? ?? true;
            set => SetValue(UseAnimationsProperty, value);
        }
        public string MenuButtonContent
        {
            get => (string)GetValue(MenuButtonContentProperty);
            set => SetValue(MenuButtonContentProperty, value);
        }
        public object MenuInfoAreaExpandedTop
        {
            get => GetValue(MenuInfoAreaExpandedTopProperty);
            set => SetValue(MenuInfoAreaExpandedTopProperty, value);
        }
        public object MenuInfoAreaExpandedBottom
        {
            get => GetValue(MenuInfoAreaExpandedBottomProperty);
            set => SetValue(MenuInfoAreaExpandedBottomProperty, value);
        }
        public object MenuInfoAreaCollapsedTop
        {
            get => GetValue(MenuInfoAreaCollapsedTopProperty);
            set => SetValue(MenuInfoAreaCollapsedTopProperty, value);
        }
        public object MenuInfoAreaCollapsedBottom
        {
            get => GetValue(MenuInfoAreaCollapsedBottomProperty);
            set => SetValue(MenuInfoAreaCollapsedBottomProperty, value);
        }
        public bool IsLoadingPage
        {
            get => GetValue(IsLoadingPageProperty) as bool? ?? true;
            set => SetValue(IsLoadingPageProperty, value);
        }
        public bool ShowLoadingPane
        {
            get => GetValue(ShowLoadingPaneProperty) as bool? ?? true;
            set => SetValue(ShowLoadingPaneProperty, value);
        }

        public SplitViewItemBase SelectedItem
        {
            get => (SplitViewItemBase)_treeView?.SelectedItem;
            set => value.IsSelected = true;
        }
        public TransitionInType TransitionIn
        {
            get => GetValue(TransitionInProperty) as TransitionInType? ?? TransitionInType.None;
            set => SetValue(TransitionInProperty, value);
        }
        public TransitionOutType TransitionOut
        {
            get => GetValue(TransitionOutProperty) as TransitionOutType? ?? TransitionOutType.None;
            set => SetValue(TransitionOutProperty, value);
        }
        public TimeSpan TransitionDuration
        {
            get => GetValue(TransitionDurationProperty) as TimeSpan? ?? TimeSpan.Zero;
            set => SetValue(TransitionDurationProperty, value);
        }
        public bool TransitionFirstContent
        {
            get => GetValue(TransitionFirstContentProperty) as bool? ?? false;
            set => SetValue(TransitionFirstContentProperty, value);
        }
        public bool RunAnimationsSimultaneously
        {
            get => GetValue(RunAnimationsSimultaneouslyProperty) as bool? ?? false;
            set => SetValue(RunAnimationsSimultaneouslyProperty, value);
        }
        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        #endregion

        #region Ctor

        static SplitView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitView), new FrameworkPropertyMetadata(typeof(SplitView)));
        }

        public SplitView()
        {
            ItemSource = new ObservableCollection<SplitViewItemBase>();
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _treeView = GetTemplateChild("PART_TreeView") as TreeView;
            _menuButton = GetTemplateChild("PART_MenuButton") as Button;
            _buttonsColumn = GetTemplateChild("PART_ButtonsColumn") as ColumnDefinition;
            _content = GetTemplateChild("PART_Content") as ContentControl;
            _infoAreaCollapsedTop = GetTemplateChild("PART_InfoAreaCollapsedTop") as ContentPresenter;
            _infoAreaCollapsedBottom = GetTemplateChild("PART_InfoAreaCollapsedBottom") as ContentPresenter;
            _infoAreaExpandedTop = GetTemplateChild("PART_InfoAreaExpandedTop") as ContentPresenter;
            _infoAreaExpandedBottom = GetTemplateChild("PART_InfoAreaExpandedBottom") as ContentPresenter;
            _treeViewScroll = GetTemplateChild("PART_TreeViewScroll") as ScrollViewer;
            _indicatorBottom = GetTemplateChild("PART_IndicatorBottom") as UIElement;
            _indicatorTop = GetTemplateChild("PART_IndicatorTop") as UIElement;

            Guard.NotNull(_treeView, nameof(_treeView));
            Guard.NotNull(_menuButton, nameof(_menuButton));
            Guard.NotNull(_buttonsColumn, nameof(_buttonsColumn));
            Guard.NotNull(_content, nameof(_content));
            Guard.NotNull(_infoAreaCollapsedTop, nameof(_infoAreaCollapsedTop));
            Guard.NotNull(_infoAreaCollapsedBottom, nameof(_infoAreaCollapsedBottom));
            Guard.NotNull(_infoAreaExpandedTop, nameof(_infoAreaExpandedTop));
            Guard.NotNull(_infoAreaExpandedBottom, nameof(_infoAreaExpandedBottom));
            Guard.NotNull(_treeViewScroll, nameof(_treeViewScroll));
            Guard.NotNull(_indicatorBottom, nameof(_indicatorBottom));
            Guard.NotNull(_indicatorTop, nameof(_indicatorTop));

            _treeView.SelectedItemChanged += _treeView_SelectedItemChanged;
            _menuButton.Click += _menuButton_Click;
            _treeViewScroll.ScrollChanged += TreeViewScroll_ScrollChanged;
            TreeViewScroll_ScrollChanged(_treeViewScroll, null);

            _scrollTopTimer = new DispatcherTimer(DispatcherPriority.Render) { Interval = TimeSpan.FromSeconds(0.05) };
            _scrollTopTimer.Tick += ScrollTopTimer_Tick;
            _scrollBottomTimer = new DispatcherTimer(DispatcherPriority.Render) { Interval = TimeSpan.FromSeconds(0.05) };
            _scrollBottomTimer.Tick += ScrollBottomTimer_Tick;
            _indicatorTop.MouseEnter += (s, e) => _scrollTopTimer.Start();
            _indicatorTop.MouseLeave += (s, e) => _scrollTopTimer.Stop();
            _indicatorBottom.MouseEnter += (s, e) => _scrollBottomTimer.Start();
            _indicatorBottom.MouseLeave += (s, e) => _scrollBottomTimer.Stop();

            Loaded += (s, e) =>
            {
                _infoAreaCollapsedTop.Opacity = 0;
                _infoAreaCollapsedBottom.Opacity = 0;
            };

            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Closing += ParentWindow_Closing;
                window.StateChanged += ParentWindow_StateChanged;
            }
        }

        private void ScrollTopTimer_Tick(object sender, EventArgs e)
        {
            _treeViewScroll.ScrollToVerticalOffset(_treeViewScroll.VerticalOffset - 10);
        }

        private void ScrollBottomTimer_Tick(object sender, EventArgs e)
        {
            _treeViewScroll.ScrollToVerticalOffset(_treeViewScroll.VerticalOffset + 10);
        }

        private void TreeViewScroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender is ScrollViewer sv)
            {
                _indicatorTop.Visibility = Math.Abs(sv.VerticalOffset) < 0.00001 ? Visibility.Collapsed : Visibility.Visible;
                _indicatorBottom.Visibility = Math.Abs(sv.VerticalOffset - sv.ScrollableHeight) < 0.00001 ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        #endregion

        #region Private Methods

        private void ParentWindow_StateChanged(object sender, EventArgs e)
        {
            if (sender is Window wdw)
            {
                switch (wdw.WindowState)
                {
                    case WindowState.Normal:
                        Margin = new Thickness(-5, 1, -5, -5);
                        break;
                    case WindowState.Maximized:
                        Margin = new Thickness(0, 1, 0, 0);
                        break;
                }
            }
        }

        private bool _closeConfirmed;
        private async void ParentWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_closeConfirmed && SelectedItem is SplitViewItem item && item.Content is SplitViewContent content)
            {
                if (content.CallCloseAsync)
                {
                    e.Cancel = true;
                    IsLoadingPage = true;
                    try
                    {
                        if (await content.Close())
                        {
                            _closeConfirmed = true;
                            await Task.Run(() => Waiter.Retry(() => Application.Current.Dispatcher.Invoke(() => (sender as Window)?.Close()), new RetryOptions { ThinkTimeBetweenChecks = TimeSpan.FromSeconds(1000) }));
                        }
                    }
                    finally { IsLoadingPage = false; }
                }
                else
                    e.Cancel = e.Cancel || !content.Close().Result;
            }
        }

        private void CancelTreeViewSelection(SplitViewItem oldItem, SplitViewItem newItem)
        {
            _ignoreNextSwitch = true;
            if (oldItem != null)
                SetSelected(oldItem);
            else
                SetSelected(newItem, false);
            _content.Content = oldItem?.Content;
        }
        private async Task<bool> SwitchContentInternal(SplitViewItem oldItem, SplitViewItem newItem)
        {
            IsLoadingPage = true;
            try
            {
                var svcOld = oldItem?.Content as SplitViewContent;
                var svcNew = newItem?.Content as SplitViewContent;

                if (svcOld != null && !await svcOld.Close())
                {
                    CancelTreeViewSelection(oldItem, newItem);
                    return false;
                }

                _content.Content = newItem?.Content;
                if (svcNew != null && !await svcNew.Open())
                {
                    CancelTreeViewSelection(oldItem, newItem);
                    return false;
                }
                if (svcOld?.DataContext is SplitViewContentViewModel oldVm)
                    oldVm.IsOpen = false;
                if (svcNew?.DataContext is SplitViewContentViewModel newVm)
                    newVm.IsOpen = true;
            }
            finally { IsLoadingPage = false; }
            return true;
        }
        private async void _treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var o = e.OldValue as SplitViewItem;
            var n = e.NewValue as SplitViewItem;

            if (!_ignoreNextSwitch)
            {
                if (IsLoadingPage)
                {
                    CancelTreeViewSelection(o, n);
                    return;
                }
            }

            if (!_ignoreNextSwitch)
                await SwitchContentInternal(o, n);
            else
                _ignoreNextSwitch = false;
        }

        private void SetSelected(SplitViewItemBase item, bool value = true)
        {
            void EventHandler(object s, EventArgs ea)
            {
                _treeView.LayoutUpdated -= EventHandler;
                item.IsSelected = value;
            }

            _treeView.LayoutUpdated += EventHandler;
        }

        private void _menuButton_Click(object sender, RoutedEventArgs e)
        {
            IsExpanded = !IsExpanded;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private void StartShrinkAnimation()
        {
            var groupItems = GetGroupItems();
            if (!UseAnimations)
            {
                _buttonsColumn.Width = new GridLength(36.5);
                _infoAreaExpandedTop.Opacity = 0;
                _infoAreaExpandedBottom.Opacity = 0;
                _infoAreaCollapsedTop.Opacity = 1;
                _infoAreaCollapsedBottom.Opacity = 1;
                foreach (var item in groupItems)
                {
                    if (double.IsNaN(item.Height))
                    {
                        item.Height = item.ActualHeight;
                        item.Tag = item.ActualHeight;
                    }
                    item.Height = 10;
                    //item.Opacity = 0;
                }
                return;
            }
            StopAnimations(false);

            _currentStoryboard = new Storyboard { Duration = _animationDuration };

            var columnAnimation = new GridLengthAnimation
            {
                From = _buttonsColumn.Width.IsAuto ? new GridLength(_treeView.ActualWidth) : _buttonsColumn.Width,
                To = new GridLength(36.5),
                Duration = _animationDuration,
                EasingFunction = new CubicEase()
            };
            Storyboard.SetTarget(columnAnimation, _buttonsColumn);
            Storyboard.SetTargetProperty(columnAnimation, new PropertyPath(ColumnDefinition.WidthProperty));
            _currentStoryboard.Children.Add(columnAnimation);

            var iAET = new DoubleAnimation(0, _animationDuration) { EasingFunction = new CubicEase() };
            var iAEB = new DoubleAnimation(0, _animationDuration) { EasingFunction = new CubicEase() };
            var iACT = new DoubleAnimation(1, _animationDuration) { EasingFunction = new CubicEase() };
            var iACB = new DoubleAnimation(1, _animationDuration) { EasingFunction = new CubicEase() };
            iAET.SetTarget(_infoAreaExpandedTop, OpacityProperty);
            iAEB.SetTarget(_infoAreaExpandedBottom, OpacityProperty);
            iACT.SetTarget(_infoAreaCollapsedTop, OpacityProperty);
            iACB.SetTarget(_infoAreaCollapsedBottom, OpacityProperty);
            _currentStoryboard.Children.Add(iAET, iAEB, iACT, iACB);

            foreach (var item in groupItems)
            {
                if (double.IsNaN(item.Height))
                {
                    item.Height = item.ActualHeight;
                    item.Tag = item.ActualHeight;
                }
                //DoubleAnimation opAnimation = new DoubleAnimation(0, _animationDuration) { EasingFunction = new CubicEase() };
                //Storyboard.SetTarget(opAnimation, item);
                //Storyboard.SetTargetProperty(opAnimation, new PropertyPath(Border.OpacityProperty));
                //_currentStoryboard.Children.Add(opAnimation);
                var hAnimation = new DoubleAnimation(10, _animationDuration) { EasingFunction = new CubicEase() };
                Storyboard.SetTarget(hAnimation, item);
                Storyboard.SetTargetProperty(hAnimation, new PropertyPath(HeightProperty));
                _currentStoryboard.Children.Add(hAnimation);
            }

            _currentStoryboard.Completed += _currentStoryboard_Completed;
            _currentStoryboard.Begin();
        }

        private void _currentStoryboard_Completed(object sender, EventArgs e)
        {
            _currentStoryboard = null;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private void StartExpandAnimation()
        {
            var groupItems = GetGroupItems();
            if (!UseAnimations)
            {
                _buttonsColumn.Width = GridLength.Auto;
                _infoAreaExpandedTop.Opacity = 1;
                _infoAreaExpandedBottom.Opacity = 1;
                _infoAreaCollapsedTop.Opacity = 0;
                _infoAreaCollapsedBottom.Opacity = 0;
                foreach (var item in groupItems)
                {
                    item.Height = double.NaN;
                    //item.Opacity = 1;
                }
                return;
            }
            StopAnimations(false);

            _currentStoryboard = new Storyboard { Duration = _animationDuration };

            var columnAnimation = new GridLengthAnimation
            {
                From = _buttonsColumn.Width.IsAuto ? new GridLength(_treeView.ActualWidth) : _buttonsColumn.Width,
                To = new GridLength(_treeView.ActualWidth),
                Duration = _animationDuration,
                EasingFunction = new CubicEase()
            };
            Storyboard.SetTarget(columnAnimation, _buttonsColumn);
            Storyboard.SetTargetProperty(columnAnimation, new PropertyPath(ColumnDefinition.WidthProperty));
            _currentStoryboard.Children.Add(columnAnimation);

            var iAET = new DoubleAnimation(1, _animationDuration) { EasingFunction = new CubicEase() };
            var iAEB = new DoubleAnimation(1, _animationDuration) { EasingFunction = new CubicEase() };
            var iACT = new DoubleAnimation(0, _animationDuration) { EasingFunction = new CubicEase() };
            var iACB = new DoubleAnimation(0, _animationDuration) { EasingFunction = new CubicEase() };
            iAET.SetTarget(_infoAreaExpandedTop, OpacityProperty);
            iAEB.SetTarget(_infoAreaExpandedBottom, OpacityProperty);
            iACT.SetTarget(_infoAreaCollapsedTop, OpacityProperty);
            iACB.SetTarget(_infoAreaCollapsedBottom, OpacityProperty);
            _currentStoryboard.Children.Add(iAET, iAEB, iACT, iACB);

            foreach (var item in groupItems)
            {
                if (double.IsNaN(item.Height))
                {
                    item.Height = item.ActualHeight;
                    item.Tag = item.ActualHeight;
                }
                //var opAnimation = new DoubleAnimation(1, _animationDuration) { EasingFunction = new CubicEase() };
                //Storyboard.SetTarget(opAnimation, item);
                //Storyboard.SetTargetProperty(opAnimation, new PropertyPath(Border.OpacityProperty));
                //_currentStoryboard.Children.Add(opAnimation);
                var hAnimation = new DoubleAnimation((double)item.Tag, _animationDuration) { EasingFunction = new CubicEase() };
                Storyboard.SetTarget(hAnimation, item);
                Storyboard.SetTargetProperty(hAnimation, new PropertyPath(HeightProperty));
                _currentStoryboard.Children.Add(hAnimation);
            }

            _currentStoryboard.Completed += _currentStoryboard_Completed;
            _currentStoryboard.Completed += (s, e) =>
            {
                Task.Run(() =>
                {
                    Thread.Sleep(100);
                    Dispatcher.Invoke(() =>
                    {
                        _buttonsColumn.Width = GridLength.Auto;
                        //foreach(var item in groupItems)
                        //    item.Height = double.NaN;
                    });
                });
            };
            _currentStoryboard.Begin();
        }

        private List<Border> GetGroupItems()
        {
            var result = new List<Border>();
            if (ItemSource != null)
            {
                foreach (var tvi in ItemSource.OfType<SplitViewItemGroup>()
                    .Select(x => _treeView.ItemContainerGenerator.ContainerFromItem(x))
                    .OfType<TreeViewItem>())
                {
                    GetGroupItemsRec(result, tvi);
                }
            }
            return result;
        }

        private void StopAnimations(bool fill = true)
        {
            if (_currentStoryboard != null)
            {
                if (fill)
                    _currentStoryboard.SkipToFill();
                else
                    _currentStoryboard.Stop();
            }
        }

        #endregion

        #region Public Methods

        public async Task<bool> SwitchToItem(SplitViewItem item)
        {
            _ignoreNextSwitch = true;
            SetSelected(item);
            return await SwitchContentInternal(_treeView.SelectedItem as SplitViewItem, item);
        }

        public void CollapseMenu(bool? useAnimation)
        {
            if (!IsExpanded)
                return;
            var prevUa = UseAnimations;
            if (useAnimation.HasValue)
                UseAnimations = useAnimation.Value;
            try
            {
                IsExpanded = false;
            }
            finally { UseAnimations = prevUa; }
        }

        public void ExpandMenu(bool? useAnimation)
        {
            if (IsExpanded)
                return;
            var prevUa = UseAnimations;
            if (useAnimation.HasValue)
                UseAnimations = useAnimation.Value;
            try
            {
                IsExpanded = true;
            }
            finally { UseAnimations = prevUa; }
        }

        public void SetItems(IEnumerable<IPageGroup> pageGroups, IEnumerable<IPage> pages) => SetItems(pageGroups, pages, null);
        public void SetItems(IEnumerable<IPageGroup> pageGroups, IEnumerable<IPage> pages, ITranslationManager translationManager)
        {
            var items = new List<SplitViewItemBase>();

            var pageArr = pages.ToArray();
            var groups = from g in pageGroups.OrderBy(x => x.PageGroupPriority).Append(null)
                         select new { Group = g, Pages = pageArr.Where(x => x.PageGroupId == g?.PageGroupId).OrderBy(x => x.PagePriority) };

            bool selectionSet = false;
            var topLevelPrio = new List<int>();
            foreach (var group in groups)
            {
                IList list = items;
                if (group.Group != null)
                {
                    var svGroup = new SplitViewItemGroup
                    {
                        Header = group.Group.PageGroupName == null ? null : translationManager?.GetTranslation(group.Group.PageGroupName, group.Group.TranslationProviderKey ?? translationManager.DefaultProviderKey) ?? group.Group.PageGroupName
                    };
                    if (translationManager != null && group.Group.PageGroupName != null)
                        translationManager.LanguageChanged += (s, e) => svGroup.Header = translationManager.GetTranslation(group.Group.PageGroupName, group.Group.TranslationProviderKey ?? translationManager.DefaultProviderKey);
                    items.Add(svGroup);
                    topLevelPrio.Add(group.Group.PageGroupPriority);
                    list = svGroup.Children;
                }

                foreach (var page in group.Pages)
                {
                    var svItem = new SplitViewItem
                    {
                        Icon = page.PageIcon,
                        Header = translationManager?.GetTranslation(page.PageName, page.TranslationProviderKey ?? translationManager.DefaultProviderKey) ?? page.PageName,
                        Content = page.PageContent
                    };
                    if (translationManager != null)
                        translationManager.LanguageChanged += (s, e) => svItem.Header = translationManager.GetTranslation(page.PageName, page.TranslationProviderKey ?? translationManager.DefaultProviderKey);
                    if (ReferenceEquals(list, items))
                    {
                        var idx = topLevelPrio.IndexOf(x => x > page.PagePriority);
                        if (idx == -1)
                            items.Add(svItem);
                        else
                            items.Insert(idx, svItem);
                    }
                    else
                        list.Add(svItem);
                    if (page.IsPageSelectedByDefault && !selectionSet)
                    {
                        svItem.IsSelected = true;
                        selectionSet = true;
                    }
                }
            }

            ItemSource = items;
        }

        #endregion

        #region Static Methods

        private static void GetGroupItemsRec(List<Border> items, TreeViewItem root)
        {
            Guard.NotNull(items, nameof(items));
            Guard.NotNull(root, nameof(root));

            if (root.DataContext is SplitViewItemGroup group)
            {
                if ((VisualTreeHelper.GetChild(root, 0) as Grid)?.Children[0] is Border border)
                    items.Add(border);
                if (group.Children != null)
                {
                    foreach (var child in group.Children.OfType<SplitViewItemGroup>())
                    {
                        if (root.ItemContainerGenerator.ContainerFromItem(child) is TreeViewItem tvi)
                            GetGroupItemsRec(items, tvi);
                    }
                }
            }
        }

        #endregion
    }
}
