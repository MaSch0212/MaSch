using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace MaSch.Presentation.Wpf.Controls
{
    public class TabControl : System.Windows.Controls.TabControl
    {
        public static readonly DependencyProperty AnimationDurationSecondsProperty =
            DependencyProperty.Register(
                "AnimationDurationSeconds",
                typeof(double),
                typeof(TabControl),
                new PropertyMetadata(0.2D));

        public static readonly DependencyProperty IsPreviewEnabledProperty =
            DependencyProperty.Register(
                "IsPreviewEnabled",
                typeof(bool),
                typeof(TabControl),
                new PropertyMetadata(true));

        public static readonly DependencyProperty PreviewDelayProperty =
            DependencyProperty.Register(
                "PreviewDelay",
                typeof(int),
                typeof(TabControl),
                new PropertyMetadata(1000));

        public static readonly DependencyProperty IsNavigationPartVisibleProperty =
            DependencyProperty.Register(
                "IsNavigationPartVisible",
                typeof(bool),
                typeof(TabControl),
                new PropertyMetadata(true));

        private ContentPresenter _previewContent;
        private ContentPresenter _currentContent;
        private int _previewIndex;
        private bool _isPreview;
        private bool _isNewPreview;

        public double AnimationDurationSeconds
        {
            get => GetValue(AnimationDurationSecondsProperty) as double? ?? 0.2D;
            set => SetValue(AnimationDurationSecondsProperty, value);
        }

        public bool IsPreviewEnabled
        {
            get => GetValue(IsPreviewEnabledProperty) as bool? ?? true;
            set => SetValue(IsPreviewEnabledProperty, value);
        }

        public int PreviewDelay
        {
            get => GetValue(PreviewDelayProperty) as int? ?? 1000;
            set => SetValue(PreviewDelayProperty, value);
        }

        public bool IsNavigationPartVisible
        {
            get => GetValue(IsNavigationPartVisibleProperty) as bool? ?? true;
            set => SetValue(IsNavigationPartVisibleProperty, value);
        }

        static TabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(typeof(TabControl)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        public TabControl()
        {
            _previewIndex = -1;
        }

        /// <inheritdoc/>
        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (e.NewItems != null)
            {
                foreach (var ti in e.NewItems.OfType<TabItem>())
                {
                    AddPreviewEventToTabItem(ti);
                }
            }
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitializeButtons();
            InitializeUnderline();
            InitializeTabItems();
            InitializeContent();
        }

        private void AddPreviewEventToTabItem(TabItem item, FrameworkElement eventTo = null)
        {
            if (eventTo == null)
                eventTo = item;
            eventTo.MouseEnter += async (s, ex) =>
            {
                if (IsPreviewEnabled && !item.IsSelected)
                {
                    var waited = 0;
                    while (waited < PreviewDelay && !_isPreview)
                    {
                        if (!item.IsMouseOver || item.IsSelected)
                            return;
                        await Task.Delay(50);
                        waited += 50;
                    }

                    if (item.IsMouseOver && !item.IsSelected)
                    {
                        var wasPreview = _isPreview;
                        _isPreview = true;
                        _previewIndex = item.TabIndex;
                        if (wasPreview)
                            _isNewPreview = true;
                        await ShowPreview(item.Content, wasPreview);
                        while (item.IsMouseOver && !item.IsSelected)
                        {
                            await Task.Delay(50);
                        }

                        if (!item.IsSelected && !_isNewPreview)
                            await Task.Delay(PreviewDelay);
                        if (_isPreview && !_isNewPreview)
                        {
                            _isPreview = false;
                            await HidePreview();
                        }
                        else
                        {
                            _isNewPreview = false;
                        }
                    }
                }
            };
        }

        private async Task ShowPreview(object content, bool wasPreview)
        {
            if (_previewContent != null && _currentContent != null && wasPreview)
            {
                var daOut = new DoubleAnimation(0.85, 0, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds / 2)));
                _previewContent.BeginAnimation(OpacityProperty, daOut);
                await Task.Delay((int)(AnimationDurationSeconds / 2 * 1000));
                _previewContent.Content = content;
                var daIn = new DoubleAnimation(0, 0.85, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds / 2)));
                _previewContent.BeginAnimation(OpacityProperty, daIn);
            }

            if (_previewContent != null && _currentContent != null && !wasPreview)
            {
                _previewContent.RenderTransform = null;
                _previewContent.Content = null;
                _previewContent.Content = content;
                var be = new BlurEffect();
                var sc = new ScaleTransform(1, 1);
                _currentContent.Effect = be;
                _currentContent.RenderTransform = sc;
                var opaPrev = new DoubleAnimation(0, 0.85, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
                _previewContent.BeginAnimation(OpacityProperty, opaPrev);
                var blur = new DoubleAnimation(0, 20, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
                be.BeginAnimation(BlurEffect.RadiusProperty, blur);
                var scDa = new DoubleAnimation(1, 0.9, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
                sc.BeginAnimation(ScaleTransform.ScaleXProperty, scDa);
                sc.BeginAnimation(ScaleTransform.ScaleYProperty, scDa);
                var opaCur = new DoubleAnimation(1, 0.75, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
                _currentContent.BeginAnimation(OpacityProperty, opaCur);
            }
        }

        private async Task HidePreview()
        {
            if (_previewContent != null && _currentContent != null && _currentContent.Effect is BlurEffect be && _currentContent.RenderTransform is ScaleTransform sc)
            {
                var opaPrev = new DoubleAnimation(0.85, 0, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
                _previewContent.BeginAnimation(OpacityProperty, opaPrev);
                var blur = new DoubleAnimation(20, 0, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
                be.BeginAnimation(BlurEffect.RadiusProperty, blur);
                var scDa = new DoubleAnimation(0.9, 1, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
                sc.BeginAnimation(ScaleTransform.ScaleXProperty, scDa);
                sc.BeginAnimation(ScaleTransform.ScaleYProperty, scDa);
                var opaCur = new DoubleAnimation(0.75, 1, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
                _currentContent.BeginAnimation(OpacityProperty, opaCur);

                await Task.Delay((int)(AnimationDurationSeconds * 1000));
                if (!_isPreview)
                    _currentContent.Effect = null;
            }
        }

        private void InitializeTabItems()
        {
            _previewContent = GetTemplateChild("MaSch_PreviewContent") as ContentPresenter;
            Loaded += (s, e) =>
            {
                foreach (TabItem ti in Items)
                {
                    AddPreviewEventToTabItem(ti);
                }
            };
        }

        private void InitializeContent()
        {
            _currentContent = GetTemplateChild("PART_SelectedContentHost") as ContentPresenter;
            if (GetTemplateChild("MaSch_LastContent") is ContentPresenter lastContent && _currentContent != null)
            {
                TabItem lastItem = null;
                var lastIndex = -1;
                SelectionChanged += (s, e) =>
                {
                    if (Equals(lastItem, SelectedItem))
                    {
                        Debug.WriteLine("Ignore one event!");
                        return;
                    }

                    if (lastItem != null)
                    {
                        lastContent.Content = lastItem.Content;
                        lastContent.RenderTransform = null;
                        _currentContent.RenderTransform = null;
                        _previewContent.RenderTransform = null;

                        if (_isPreview && _previewIndex == lastItem.TabIndex)
                        {
                            lastContent.Effect = _currentContent.Effect;
                            _currentContent.Effect = null;
                            _previewContent.Opacity = 0;
                            lastContent.RenderTransform = new ScaleTransform(0.9, 0.9);
                            var opaLast = new DoubleAnimation(0.75, 0, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
                            var opaCurr = new DoubleAnimation(0.85, 1, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
                            _currentContent.BeginAnimation(OpacityProperty, opaCurr);
                            lastContent.BeginAnimation(OpacityProperty, opaLast);
                        }
                        else
                        {
                            var lastTrans = new TranslateTransform();
                            var currTrans = new TranslateTransform();
                            lastContent.RenderTransform = lastTrans;
                            _currentContent.RenderTransform = currTrans;
                            _previewContent.RenderTransform = lastTrans;

                            lastContent.Effect = null;
                            _currentContent.Effect = null;
                            var opaLast = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds / 2)));
                            var opaPrev = new DoubleAnimation(0.85, 0, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds / 2)));
                            var opaCurr = new DoubleAnimationUsingKeyFrames
                            {
                                Duration = new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)),
                            };
                            opaCurr.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                            opaCurr.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(AnimationDurationSeconds / 2))));
                            opaCurr.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(AnimationDurationSeconds))));
                            lastContent.BeginAnimation(OpacityProperty, opaLast);
                            _currentContent.BeginAnimation(OpacityProperty, opaCurr);
                            _previewContent.BeginAnimation(OpacityProperty, opaPrev);

                            float mult = lastIndex > SelectedIndex ? 1 : -1;
                            var transLast = new DoubleAnimation(0, mult * 50, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds / 2)));
                            var transCurr = new DoubleAnimationUsingKeyFrames
                            {
                                Duration = new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)),
                            };
                            transCurr.KeyFrames.Add(new LinearDoubleKeyFrame(-mult * 50, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                            transCurr.KeyFrames.Add(new LinearDoubleKeyFrame(-mult * 50, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(AnimationDurationSeconds / 2))));
                            transCurr.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(AnimationDurationSeconds))));
                            lastTrans.BeginAnimation(TranslateTransform.XProperty, transLast);
                            currTrans.BeginAnimation(TranslateTransform.XProperty, transCurr);
                        }
                    }

                    lastIndex = SelectedIndex;
                    lastItem = SelectedItem as TabItem;
                };
            }
        }

        private void InitializeUnderline()
        {
            if (GetTemplateChild("MaSch_Underline") is Border underline && GetTemplateChild("MaSch_TabsInner") is StackPanel stackpnl)
            {
                var tg = new TransformGroup();
                var scTrans = new ScaleTransform();
                var tlTrans = new TranslateTransform();
                tg.Children.Add(scTrans);
                tg.Children.Add(tlTrans);
                underline.RenderTransform = tg;
                TabItem lastItem = null;

                Loaded += (s, e) =>
                {
                    if (SelectedItem != null)
                    {
                        var t = SelectedItem as TabItem;
                        var tabText = t?.Template.FindName("MaSch_Text", t) as ContentPresenter;
                        tabText?.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                        InvalidateArrange();
                        UpdateLayout();
                        if (t != null)
                        {
                            try
                            {
                                var v = t.TransformToVisual(stackpnl);
                                scTrans.ScaleX = t.ActualWidth - 6;
                                tlTrans.X = v.Transform(new Point(0, 0)).X + 3;
                            }
                            catch
                            {
                                if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                                    throw;
                            }
                        }
                    }

                    lastItem = SelectedItem as TabItem;
                };
                SelectionChanged += (s, e) =>
                {
                    if (Equals(lastItem, SelectedItem))
                    {
                        Debug.WriteLine("Ignore one event!");
                        return;
                    }

                    if (SelectedItem != null)
                    {
                        var tab = SelectedItem as TabItem;
                        var tabText = tab?.Template.FindName("MaSch_Text", tab) as ContentPresenter;
                        tabText?.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);

                        if (tab != null && tab.ActualWidth > 0)
                        {
                            var lastText = lastItem?.Template.FindName("MaSch_Text", lastItem) as ContentPresenter;
                            lastText?.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
                            InvalidateArrange();
                            UpdateLayout();
                            try
                            {
                                var ttv = tab.TransformToVisual(stackpnl);
                                var scale = new DoubleAnimation(tab.ActualWidth - 6, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
                                var translate = new DoubleAnimation(ttv.Transform(new Point(0, 0)).X + 3, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
                                scTrans.BeginAnimation(ScaleTransform.ScaleXProperty, scale);
                                tlTrans.BeginAnimation(TranslateTransform.XProperty, translate);
                            }
                            catch
                            {
                                if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                                    throw;
                            }
                        }

                        lastItem = SelectedItem as TabItem;
                    }
                };
            }
        }

        private void InitializeButtons()
        {
            if (GetTemplateChild("MaSch_PrevTab") is Button btnPrev)
            {
                SelectionChanged += (s, e) =>
                {
                    btnPrev.IsEnabled = SelectedIndex > 0;
                };
                btnPrev.Click += (s, e) =>
                {
                    SelectedIndex--;
                };
            }

            if (GetTemplateChild("MaSch_NextTab") is Button btnNext)
            {
                SelectionChanged += (s, e) =>
                {
                    btnNext.IsEnabled = SelectedIndex < Items.Count - 1;
                };
                btnNext.Click += (s, e) =>
                {
                    SelectedIndex++;
                };
            }
        }
    }
}