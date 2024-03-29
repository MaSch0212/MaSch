﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace MaSch.Presentation.Wpf.Controls;

/// <summary>
/// Tab control with a lot more features than the default <see cref="System.Windows.Controls.TabControl"/>.
/// </summary>
/// <seealso cref="System.Windows.Controls.TabControl" />
public class TabControl : System.Windows.Controls.TabControl
{
    /// <summary>
    /// Dependency property. Gets or sets the duration of the tab switch animation in seconds.
    /// </summary>
    public static readonly DependencyProperty AnimationDurationSecondsProperty =
        DependencyProperty.Register(
            "AnimationDurationSeconds",
            typeof(double),
            typeof(TabControl),
            new PropertyMetadata(0.2D));

    /// <summary>
    /// Dependency property. Gets or sets a value indicating whether a preview of a tab should be shown when the mouse hovers over a tab header.
    /// </summary>
    public static readonly DependencyProperty IsPreviewEnabledProperty =
        DependencyProperty.Register(
            "IsPreviewEnabled",
            typeof(bool),
            typeof(TabControl),
            new PropertyMetadata(true));

    /// <summary>
    /// Dependency property. Gets or sets the time in miliseconds to wait after the mouse hovers over a tab header to show the tab preview.
    /// </summary>
    public static readonly DependencyProperty PreviewDelayProperty =
        DependencyProperty.Register(
            "PreviewDelay",
            typeof(int),
            typeof(TabControl),
            new PropertyMetadata(1000));

    /// <summary>
    /// Dependency property. Gets or sets a value indicating whether Buttons for next tab, previous tab and a combobox for all available tabs should be displayed.
    /// </summary>
    public static readonly DependencyProperty IsNavigationPartVisibleProperty =
        DependencyProperty.Register(
            "IsNavigationPartVisible",
            typeof(bool),
            typeof(TabControl),
            new PropertyMetadata(true));

    private ContentPresenter? _previewContent;
    private ContentPresenter? _currentContent;
    private int _previewIndex;
    private bool _isPreview;
    private bool _isNewPreview;

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

    /// <summary>
    /// Gets or sets the duration of the tab switch animation in seconds.
    /// </summary>
    public double AnimationDurationSeconds
    {
        get => GetValue(AnimationDurationSecondsProperty) as double? ?? 0.2D;
        set => SetValue(AnimationDurationSecondsProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether a preview of a tab should be shown when the mouse hovers over a tab header.
    /// </summary>
    public bool IsPreviewEnabled
    {
        get => GetValue(IsPreviewEnabledProperty) as bool? ?? true;
        set => SetValue(IsPreviewEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets the time in miliseconds to wait after the mouse hovers over a tab header to show the tab preview.
    /// </summary>
    public int PreviewDelay
    {
        get => GetValue(PreviewDelayProperty) as int? ?? 1000;
        set => SetValue(PreviewDelayProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether Buttons for next tab, previous tab and a combobox for all available tabs should be displayed.
    /// </summary>
    public bool IsNavigationPartVisible
    {
        get => GetValue(IsNavigationPartVisibleProperty) as bool? ?? true;
        set => SetValue(IsNavigationPartVisibleProperty, value);
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

    private void AddPreviewEventToTabItem(TabItem item, FrameworkElement? eventTo = null)
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
            var fadeOutAnimation = new DoubleAnimation(0.85, 0, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds / 2)));
            _previewContent.BeginAnimation(OpacityProperty, fadeOutAnimation);
            await Task.Delay((int)(AnimationDurationSeconds / 2 * 1000));
            _previewContent.Content = content;
            var fadeInAnimation = new DoubleAnimation(0, 0.85, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds / 2)));
            _previewContent.BeginAnimation(OpacityProperty, fadeInAnimation);
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
            var blurAnimation = new DoubleAnimation(0, 20, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
            be.BeginAnimation(BlurEffect.RadiusProperty, blurAnimation);
            var scaleAnimation = new DoubleAnimation(1, 0.9, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
            sc.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            sc.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
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
            var blurAnimation = new DoubleAnimation(20, 0, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
            be.BeginAnimation(BlurEffect.RadiusProperty, blurAnimation);
            var scaleAnimation = new DoubleAnimation(0.9, 1, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)));
            sc.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            sc.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
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
            TabItem? lastItem = null;
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
                    _previewContent!.RenderTransform = null;

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
                        _ = opaCurr.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                        _ = opaCurr.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(AnimationDurationSeconds / 2))));
                        _ = opaCurr.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(AnimationDurationSeconds))));
                        lastContent.BeginAnimation(OpacityProperty, opaLast);
                        _currentContent.BeginAnimation(OpacityProperty, opaCurr);
                        _previewContent.BeginAnimation(OpacityProperty, opaPrev);

                        float mult = lastIndex > SelectedIndex ? 1 : -1;
                        var transLast = new DoubleAnimation(0, mult * 50, new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds / 2)));
                        var transCurr = new DoubleAnimationUsingKeyFrames
                        {
                            Duration = new Duration(TimeSpan.FromSeconds(AnimationDurationSeconds)),
                        };
                        _ = transCurr.KeyFrames.Add(new LinearDoubleKeyFrame(-mult * 50, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                        _ = transCurr.KeyFrames.Add(new LinearDoubleKeyFrame(-mult * 50, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(AnimationDurationSeconds / 2))));
                        _ = transCurr.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(AnimationDurationSeconds))));
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
            var scaleTransform = new ScaleTransform();
            var translateTransform = new TranslateTransform();
            tg.Children.Add(scaleTransform);
            tg.Children.Add(translateTransform);
            underline.RenderTransform = tg;
            TabItem? lastItem = null;

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
                            scaleTransform.ScaleX = t.ActualWidth - 6;
                            translateTransform.X = v.Transform(new Point(0, 0)).X + 3;
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
                            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scale);
                            translateTransform.BeginAnimation(TranslateTransform.XProperty, translate);
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
