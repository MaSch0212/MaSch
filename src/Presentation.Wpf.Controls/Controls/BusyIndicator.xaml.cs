﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MaSch.Presentation.Wpf.Controls;

/// <summary>
/// Control that indicates something is loading.
/// </summary>
/// <seealso cref="Control" />
public class BusyIndicator : Control
{
    /// <summary>
    /// Dependency property. Gets or sets the Brush to use for the dots.
    /// </summary>
    public static readonly DependencyProperty DotBrushProperty =
        DependencyProperty.Register(
            "DotBrush",
            typeof(Brush),
            typeof(BusyIndicator),
            new PropertyMetadata(new SolidColorBrush(Colors.Black)));

    /// <summary>
    /// Dependency property. Gets or sets a value indicating whether this <see cref="BusyIndicator"/> should be a circle instead of a horizontal line.
    /// </summary>
    public static readonly DependencyProperty CircleModeProperty =
        DependencyProperty.Register(
            "CircleMode",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(true, CircleModeChanged));

    /// <summary>
    /// Dependency property. Gets or sets the width of the bar.
    /// </summary>
    public static readonly DependencyProperty BarWidthProperty =
        DependencyProperty.Register(
            "BarWidth",
            typeof(double),
            typeof(BusyIndicator),
            new PropertyMetadata(0D));

    private Storyboard? _bSb;
    private Storyboard? _cSb;
    private DispatcherTimer? _resizeTimer;
    private Viewbox? _circle;
    private Canvas? _barEllipses;

    static BusyIndicator()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyIndicator), new FrameworkPropertyMetadata(typeof(BusyIndicator)));
    }

    /// <summary>
    /// Gets or sets the Brush to use for the dots.
    /// </summary>
    public Brush DotBrush
    {
        get => (Brush)GetValue(DotBrushProperty);
        set => SetValue(DotBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="BusyIndicator"/> should be a circle instead of a horizontal line.
    /// </summary>
    public bool CircleMode
    {
        get => GetValue(CircleModeProperty) as bool? ?? true;
        set => SetValue(CircleModeProperty, value);
    }

    /// <summary>
    /// Gets or sets the width of the bar.
    /// </summary>
    public double BarWidth
    {
        get => GetValue(BarWidthProperty) as double? ?? 0D;
        set => SetValue(BarWidthProperty, value);
    }

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _cSb = FindResource("CircleAnimation") as Storyboard;
        _circle = GetTemplateChild("PART_Circle") as Viewbox ?? throw new KeyNotFoundException("Control could not be found: PART_Circle");
        _barEllipses = GetTemplateChild("PART_BarEllipses") as Canvas ?? throw new KeyNotFoundException("Control could not be found: PART_BarEllipses");
        _resizeTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.1), IsEnabled = false };

        IsEnabledChanged += ModernUILoading_IsEnabledChanged;
        IsVisibleChanged += ModernUILoading_IsEnabledChanged;
        SizeChanged += ModernUILoading_SizeChanged;
        _resizeTimer.Tick += ResizeTimer_Tick;

        Loaded += (s, e) =>
        {
            ModernUILoading_IsEnabledChanged(this, default);
            ModernUILoading_SizeChanged(this, null);
            CircleModeChanged(this, new DependencyPropertyChangedEventArgs(CircleModeProperty, false, CircleMode));
        };
    }

    private static void CircleModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is BusyIndicator th && th._circle != null && th._barEllipses != null)
        {
            th._circle.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Hidden;
            th._barEllipses.Visibility = (bool)e.NewValue ? Visibility.Hidden : Visibility.Visible;
            if (e.OldValue != e.NewValue && e.NewValue != null && th.IsEnabled)
            {
                if ((bool)e.NewValue)
                {
                    th._bSb?.Stop(th);
                    th._cSb?.Begin(th, th.Template, true);
                }
                else
                {
                    th._cSb?.Stop(th);
                    th.InitializeAndStartBarStoryboard();
                }
            }
        }
    }

    private void ResizeTimer_Tick(object? sender, EventArgs e)
    {
        _resizeTimer!.IsEnabled = false;
        InitializeAndStartBarStoryboard();
    }

    private void ModernUILoading_SizeChanged(object sender, SizeChangedEventArgs? e)
    {
        if (!_resizeTimer!.IsEnabled)
            _bSb?.Pause();
        _resizeTimer.IsEnabled = true;
        _resizeTimer.Stop();
        _resizeTimer.Start();
    }

    private void ModernUILoading_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (IsEnabled && IsVisible)
        {
            if (CircleMode)
                _cSb?.Begin(this, Template, true);
            else
                InitializeAndStartBarStoryboard();
        }
        else
        {
            if (CircleMode)
            {
                _cSb?.Stop(this);
            }
            else if (_bSb != null)
            {
                _bSb.Stop(this);
                _barEllipses!.Visibility = Visibility.Hidden;
            }
        }
    }

    private void InitializeAndStartBarStoryboard()
    {
        _bSb?.Stop(this);

        if (!IsEnabled || CircleMode)
            return;

        _barEllipses!.Visibility = Visibility.Visible;
        var dotHeight = _barEllipses.ActualHeight;
        var splineIndicator = ActualWidth / ((20 * dotHeight) + 100);
        if (splineIndicator < 1)
            splineIndicator = 1;

        var time = 0.4;
        var durationTime = time * 2;
        var beginTimeIncrement = time / 2;

        _bSb = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };

        for (var i = 0; i < 5; i++)
        {
            var middle = ActualWidth / ((splineIndicator * 2) + 1);
            var fromX = -dotHeight;
            var middleXEnter = splineIndicator * middle;
            var middleXLeave = middleXEnter + middle;
            var targetX = ActualWidth + dotHeight;
            var beginTime = i * beginTimeIncrement;

            var animation = new DoubleAnimationUsingKeyFrames
            {
                Duration = TimeSpan.FromSeconds(durationTime * 5),
            };
            _ = animation.KeyFrames.Add(new LinearDoubleKeyFrame(fromX, TimeSpan.FromSeconds(0)));
            _ = animation.KeyFrames.Add(new LinearDoubleKeyFrame(fromX, TimeSpan.FromSeconds(beginTime)));
            _ = animation.KeyFrames.Add(new SplineDoubleKeyFrame(
                middleXEnter,
                TimeSpan.FromSeconds(durationTime + beginTime),
                new KeySpline(0, 0, 0, 1 - (1 / splineIndicator))));
            _ = animation.KeyFrames.Add(new LinearDoubleKeyFrame(middleXLeave, TimeSpan.FromSeconds((durationTime * 2) + beginTime)));
            _ = animation.KeyFrames.Add(new SplineDoubleKeyFrame(
                targetX,
                TimeSpan.FromSeconds((durationTime * 3) + beginTime),
                new KeySpline(1, 1 / splineIndicator, 1, 1)));

            if (_barEllipses.Children[i] is Ellipse ellipse)
            {
                Storyboard.SetTarget(animation, ellipse);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
                _bSb.Children.Add(animation);
            }
        }

        _bSb.Begin(this, Template, true);
    }
}
