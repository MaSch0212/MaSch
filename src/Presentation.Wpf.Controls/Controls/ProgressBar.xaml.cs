﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MaSch.Presentation.Wpf.Helper;

namespace MaSch.Presentation.Wpf.Controls
{
    public class ProgressBar : System.Windows.Controls.ProgressBar
    {
        #region Fields

        #region Fields for the graph
        private double _maxValue;
        private readonly List<Point> _points;
        private DateTime _lastValueSet;
        #endregion
        #region Storyboards
        private Storyboard _ghraphStateStoryboard;
        #endregion
        #region Template Childs
        private ColumnDefinition _indicatorLeft;
        private ColumnDefinition _indicatorRight;
        // ReSharper disable once NotAccessedField.Local
        private Path _graphPath, _graphPathRaw;
        private Border _line;
        private FrameworkElement _graphIndicator;
        private FrameworkElement _normalIndicator;
        private FrameworkElement _normalIndeterminate;
        private FrameworkElement _graphIndeterminate;
        private Border _controlBorder;
        private TextBlock _currentSpeed;
        #endregion

        #endregion

        #region Dependcy Properties

        public static readonly DependencyProperty GraphModeProperty =
            DependencyProperty.Register("GraphMode", typeof(bool), typeof(ProgressBar),
            new PropertyMetadata(false, GraphModeChanged));
        public static readonly DependencyProperty CurrentSpeedProperty =
            DependencyProperty.Register("CurrentSpeed", typeof(double), typeof(ProgressBar), new PropertyMetadata(0D));
        public static readonly DependencyProperty CurrentSpeedUnitProperty =
            DependencyProperty.Register("CurrentSpeedUnit", typeof(string), typeof(ProgressBar), new PropertyMetadata("KB/s"));
        public static readonly DependencyProperty NormalBarHeightProperty =
            DependencyProperty.Register("NormalBarHeight", typeof(double), typeof(ProgressBar), new PropertyMetadata(5D, NormalBarHeightChanged));
        public static readonly DependencyProperty GraphBarHeightProperty =
            DependencyProperty.Register("GraphBarHeight", typeof(double), typeof(ProgressBar), new PropertyMetadata(80D, GraphBarHeightChanged));
        public static readonly DependencyProperty CurrentSpeedLabelProperty =
            DependencyProperty.Register("CurrentSpeedLabel", typeof(string), typeof(ProgressBar), new PropertyMetadata(""));
        public static readonly DependencyProperty GraphBackgroundProperty =
            DependencyProperty.Register("GraphBackground", typeof(Brush), typeof(ProgressBar), new PropertyMetadata(new SolidColorBrush(Colors.White)));
        public static readonly DependencyProperty CurrentSpeedFormatProperty =
            DependencyProperty.Register("CurrentSpeedFormat", typeof(string), typeof(ProgressBar), new PropertyMetadata("{0:N2}"));
        public static readonly DependencyProperty CurrentSpeedForegroundBrushProperty =
            DependencyProperty.Register("CurrentSpeedForegroundBrush", typeof(Brush), typeof(ProgressBar), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        #endregion

        #region Properties

        public bool GraphMode
        {
            get => GetValue(GraphModeProperty) as bool? ?? false;
            set => SetValue(GraphModeProperty, value);
        }
        public double CurrentSpeed
        {
            get => GetValue(CurrentSpeedProperty) as double? ?? 0D;
            set => SetValue(CurrentSpeedProperty, value);
        }
        public string CurrentSpeedUnit
        {
            get => (string)GetValue(CurrentSpeedUnitProperty);
            set => SetValue(CurrentSpeedUnitProperty, value);
        }
        public double NormalBarHeight
        {
            get => GetValue(NormalBarHeightProperty) as double? ?? 5D;
            set => SetValue(NormalBarHeightProperty, value);
        }
        public double GraphBarHeight
        {
            get => GetValue(GraphBarHeightProperty) as double? ?? 80D;
            set => SetValue(GraphBarHeightProperty, value);
        }
        public string CurrentSpeedLabel
        {
            get => (string)GetValue(CurrentSpeedLabelProperty);
            set => SetValue(CurrentSpeedLabelProperty, value);
        }
        public Brush GraphBackground
        {
            get => (Brush)GetValue(GraphBackgroundProperty);
            set => SetValue(GraphBackgroundProperty, value);
        }
        public string CurrentSpeedFormat
        {
            get => (string)GetValue(CurrentSpeedFormatProperty);
            set => SetValue(CurrentSpeedFormatProperty, value);
        }
        public Brush CurrentSpeedForegroundBrush
        {
            get => (Brush)GetValue(CurrentSpeedForegroundBrushProperty);
            set => SetValue(CurrentSpeedForegroundBrushProperty, value);
        }

        #endregion

        #region Ctor

        static ProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(typeof(ProgressBar)));
        }

        public ProgressBar()
        {
            _points = new List<Point>();
            _lastValueSet = DateTime.MinValue;
            _ghraphStateStoryboard = new Storyboard();
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitializeTemplateChilds();
            if (_indicatorLeft != null && _indicatorRight != null)
            {
                ValueChanged += (s, e) =>
                {
                    var percent = Value / Maximum;
                    _indicatorLeft.Width = new GridLength(percent, GridUnitType.Star);
                    _indicatorRight.Width = new GridLength(1D - percent, GridUnitType.Star);
                };
                _indicatorLeft.Width = new GridLength(Value / Maximum, GridUnitType.Star);
                _indicatorRight.Width = new GridLength(1D - Value / Maximum, GridUnitType.Star);
            }
            if (GraphMode)
            {
                if (_graphIndicator != null)
                    _graphIndicator.Opacity = 1;
                if (_normalIndicator != null)
                    _normalIndicator.Opacity = 0;
                if (_normalIndeterminate != null)
                    _normalIndeterminate.Opacity = 0;
                if (_graphIndeterminate != null)
                    _graphIndeterminate.Opacity = 0.5;
                if (_controlBorder != null)
                    _controlBorder.Opacity = 1;
            }
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            if (_indicatorLeft != null && _indicatorRight != null)
            {
                var percent = Value / Maximum;
                _indicatorLeft.Width = new GridLength(percent, GridUnitType.Star);
                _indicatorRight.Width = new GridLength(1D - percent, GridUnitType.Star);
            }
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            if (_indicatorLeft != null && _indicatorRight != null)
            {
                var percent = Value / Maximum;
                _indicatorLeft.Width = new GridLength(percent, GridUnitType.Star);
                _indicatorRight.Width = new GridLength(1D - percent, GridUnitType.Star);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// initializes the template-childs variables
        /// </summary>
        private void InitializeTemplateChilds()
        {
            _graphIndicator = GetTemplateChild("Graph") as FrameworkElement;
            _normalIndicator = GetTemplateChild("NormalIndicator") as FrameworkElement;
            _normalIndeterminate = GetTemplateChild("IndeterminateGradientFill") as FrameworkElement;
            _graphIndeterminate = GetTemplateChild("GraphIndeterminateGradientFill") as FrameworkElement;
            _indicatorLeft = GetTemplateChild("Indicator_Left") as ColumnDefinition;
            _indicatorRight = GetTemplateChild("Indicator_Right") as ColumnDefinition;
            _graphPath = GetTemplateChild("GraphPath") as Path;
            _graphPathRaw = GetTemplateChild("GraphPathRaw") as Path;
            _line = GetTemplateChild("MaSch_Line") as Border;
            _controlBorder = GetTemplateChild("ControlBorder") as Border;
            _currentSpeed = GetTemplateChild("MaSch_CurrentSpeed") as TextBlock;
        }

        /// <summary>
        /// Determindes if the given storyboard is not running an animation
        /// </summary>
        /// <param name="storyboard">the storyboard to check</param>
        /// <returns>true if the storyboard is not running an animation/false if the storyboard is running an animation</returns>
        private static bool IsStoryboardStopped(Storyboard storyboard)
        {
            try
            {
                return storyboard.GetCurrentState() == ClockState.Stopped;
            }
            catch (Exception)
            {
                return true;
            }
        }

        /// <summary>
        /// Starts the animation to the graph-view. If the animation to the normal-view is running, this animation will be stopped
        /// </summary>
        private void AnimationToGraphBar()
        {
            if (double.IsNaN(Height))
                Height = GraphBarHeight;
            if (_points.Count > 0)
                RefreshGraph();

            _ghraphStateStoryboard.Stop();
            _ghraphStateStoryboard = new Storyboard { Duration = new Duration(TimeSpan.FromSeconds(0.4)) };
            var daBigger = new DoubleAnimation(GraphBarHeight, new Duration(TimeSpan.FromSeconds(0.4)))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(daBigger, this);
            Storyboard.SetTargetProperty(daBigger, new PropertyPath(HeightProperty));
            _ghraphStateStoryboard.Children.Add(daBigger);

            if (_graphIndicator != null)
                AnimationHelper.AddOpacityAnimationToStoryboard(_ghraphStateStoryboard, 1, TimeSpan.FromSeconds(0.4), _graphIndicator, TimeSpan.Zero);
            if (_normalIndicator != null)
                AnimationHelper.AddOpacityAnimationToStoryboard(_ghraphStateStoryboard, 0, TimeSpan.FromSeconds(0.4), _normalIndicator, TimeSpan.Zero);
            if (_normalIndeterminate != null)
                AnimationHelper.AddOpacityAnimationToStoryboard(_ghraphStateStoryboard, 0, TimeSpan.FromSeconds(0.2), _normalIndeterminate, TimeSpan.Zero);
            if (_graphIndeterminate != null)
                AnimationHelper.AddOpacityAnimationToStoryboard(_ghraphStateStoryboard, 0.5, TimeSpan.FromSeconds(0.2), _graphIndeterminate, TimeSpan.FromSeconds(0.2));
            if (_controlBorder != null)
                AnimationHelper.AddOpacityAnimationToStoryboard(_ghraphStateStoryboard, 1, TimeSpan.FromSeconds(0.4), _controlBorder, TimeSpan.Zero);

            _ghraphStateStoryboard.Begin();
        }

        /// <summary>
        /// Starts the animation to the normal-view. If the animation to the graph-view is running, this animation will be stopped
        /// </summary>
        private void AnimationToNormalBar()
        {
            if (double.IsNaN(Height))
                Height = NormalBarHeight;

            _ghraphStateStoryboard.Stop();
            _ghraphStateStoryboard = new Storyboard { Duration = new Duration(TimeSpan.FromSeconds(0.4)) };
            var daSmaller = new DoubleAnimation(NormalBarHeight, new Duration(TimeSpan.FromSeconds(0.4)))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(daSmaller, this);
            Storyboard.SetTargetProperty(daSmaller, new PropertyPath(HeightProperty));
            _ghraphStateStoryboard.Children.Add(daSmaller);

            if (_graphIndicator != null)
                AnimationHelper.AddOpacityAnimationToStoryboard(_ghraphStateStoryboard, 0, TimeSpan.FromSeconds(0.4), _graphIndicator, TimeSpan.Zero);
            if (_normalIndicator != null)
                AnimationHelper.AddOpacityAnimationToStoryboard(_ghraphStateStoryboard, 1, TimeSpan.FromSeconds(0.4), _normalIndicator, TimeSpan.Zero);
            if (_normalIndeterminate != null)
                AnimationHelper.AddOpacityAnimationToStoryboard(_ghraphStateStoryboard, 1, TimeSpan.FromSeconds(0.2), _normalIndeterminate, TimeSpan.FromSeconds(0.2));
            if (_graphIndeterminate != null)
                AnimationHelper.AddOpacityAnimationToStoryboard(_ghraphStateStoryboard, 0, TimeSpan.FromSeconds(0.2), _graphIndeterminate, TimeSpan.Zero);
            if (_controlBorder != null)
                AnimationHelper.AddOpacityAnimationToStoryboard(_ghraphStateStoryboard, 0, TimeSpan.FromSeconds(0.4), _controlBorder, TimeSpan.Zero);

            _ghraphStateStoryboard.Begin();
        }

        /// <summary>
        /// Creates a point between one point and another
        /// </summary>
        /// <param name="p1">the first point</param>
        /// <param name="p2">the second point</param>
        /// <returns>the point between the first point and the second point</returns>
        private static Point GetPointBetween(Point p1, Point p2)
        {
            return new Point(p1.X + (p2.X - p1.X) / 2, p1.Y + (p2.Y - p1.Y) / 2);
        }

        /// <summary>
        /// Is called when the GraphMode changes. Animates to the graph-view or to the normal-view depending on the GraphMode value.
        /// </summary>
        /// <param name="obj">the ModernUIProgressBar object</param>
        /// <param name="e">the DependencyPropertyChangedEventArgs for the change</param>
        private static void GraphModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            bool? oldVal = e.OldValue as bool?, newVal = e.NewValue as bool?;
            var progBar = obj as ProgressBar;
            if (newVal != oldVal && progBar != null)
            {

                if (newVal == true)
                {
                    progBar.AnimationToGraphBar();
                }
                else
                {
                    progBar.AnimationToNormalBar();
                }
            }
        }

        /// <summary>
        /// Is called when the NormalBarHeight changes. Animates to the new normal-bar size if the animation is running else the height is set to the normal-bar size
        /// </summary>
        /// <param name="obj">the ModernUIProgressBar object</param>
        /// <param name="e">the DependencyPropertyChangedEventArgs for the change</param>
        private static void NormalBarHeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ProgressBar progBar && !progBar.GraphMode)
            {
                if (IsStoryboardStopped(progBar._ghraphStateStoryboard))
                    progBar.Height = (double)e.NewValue;
                else
                    progBar.AnimationToNormalBar();
            }
        }

        /// <summary>
        /// Is called when the GraphBarHeight changes. Animates to the new graph-bar size if the animation is running else the height is set to the graph-bar size
        /// </summary>
        /// <param name="obj">the ModernUIProgressBar object</param>
        /// <param name="e">the DependencyPropertyChangedEventArgs for the change</param>
        private static void GraphBarHeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ProgressBar progBar && progBar.GraphMode)
            {
                if (IsStoryboardStopped(progBar._ghraphStateStoryboard))
                    progBar.Height = (double)e.NewValue;
                else
                    progBar.AnimationToGraphBar();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the progress of the ProgressBar. The progres have to be greater than the current progress!
        /// </summary>
        /// <param name="newProgress">the new progress on the ProgressBar (X-Axis)</param>
        /// <param name="value">the value of the new progress for the graph (Y-Axis)</param>
        /// <exception cref="ArgumentException"></exception>
        public void SetProgress(double newProgress, double value)
        {
            if (Value >= newProgress && Value > 0)
                throw new ArgumentException("The new progress must be greater than the old progress!");

            Value = newProgress;

            //Determine the maximum value
            if (value > _maxValue && value >= 0)
            {
                for (var i = 0; i < _points.Count; i++)
                {
                    _points[i] = new Point(_points[i].X, _points[i].Y + value - _maxValue);
                }
                _maxValue = value;
            }

            //add the new values to the point-list
            _points.Add(value >= 0 ? new Point(newProgress, _maxValue - value) : new Point(newProgress, _maxValue));

            if (GraphMode)
            {
                RefreshGraph();
            }
        }
        
        // ReSharper disable once UnusedMember.Local
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private List<Point> Algo1(IReadOnlyList<Point> points)
        {
            var result = new List<Point> { points[0] };
            var f = 1D / Math.Abs(Maximum - Minimum);
            var beforeLastIndex = 0;
            var lastIndex = 0;
            for (var i = 1; i < points.Count; i++)
            {
                var distX = (points[i].X - _points[lastIndex].X) * f;
                if (distX > 0.01)
                {
                    var c = points.Skip(beforeLastIndex + 1).Take(i - beforeLastIndex);
                    if (c.Count() < 20)
                    {
                        var id = i <= 20 ? 0 : i - 20;
                        c = points.Skip(id).Take(i - id);
                    }

                    result.Add(new Point(_points[i].X, c.Average(x => x.Y)));
                    beforeLastIndex = lastIndex;
                    lastIndex = i;
                }
            }
            var d = points.Skip(beforeLastIndex + 1);
            if (d.Count() < 20)
            {
                var id = points.Count <= 20 ? 0 : points.Count - 20;
                d = points.Skip(id);
            }
            if (_points.Skip(beforeLastIndex + 1).Any())
                result.Add(new Point(points.Last().X, d.Average(x => x.Y)));
            return result;
        }

        // ReSharper disable once UnusedMember.Local
        private List<Point> Algo2(IReadOnlyList<Point> points)
        {
            var result = new List<Point> { points[0] };
            var f = 1D / Math.Abs(Maximum - Minimum);
            double min = points[0].Y, max = points[0].Y;
            for (var i = 1; i < points.Count; i++)
            {
                var distX = (points[i].X - points[i - 1].X);
                var relY = 1D / Math.Abs(Math.Max(max, points[i].Y) - Math.Min(min, points[i].Y)) * (ActualHeight / ActualWidth);

                double minAllowed, maxAllowed;
                if (i > 1)
                {
                    var prevDist = result[i - 1] - result[i - 2];
                    var anglePrev = Math.Atan((prevDist.Y * relY) / (prevDist.X * f));
                    var maxNextAngle = Math.Min(Math.PI / 3, anglePrev + distX * f * 100 * Math.Max(0.5, Math.Pow(anglePrev, 2)));
                    var minNextAngle = Math.Max(-Math.PI / 3, anglePrev - distX * f * 100 * Math.Max(0.5, Math.Pow(anglePrev, 2)));
                    var y1 = result[i - 1].Y + Math.Tan(maxNextAngle) * distX * f / relY;
                    var y2 = result[i - 1].Y + Math.Tan(minNextAngle) * distX * f / relY;
                    maxAllowed = Math.Max(y1, y2);
                    minAllowed = Math.Min(y1, y2);
                }
                else
                {
                    maxAllowed = points[i].Y;
                    minAllowed = points[i].Y;
                }
                var desiredY = points[i].Y;
                result.Add(new Point(points[i].X, Math.Min(maxAllowed, Math.Max(minAllowed, desiredY))));
                if (result.Last().Y < min)
                    min = result.Last().Y;
                if (result.Last().Y > max)
                    max = result.Last().Y;
            }
            return result;
        }

        // ReSharper disable once UnusedMember.Local
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private List<Point> Algo3(IReadOnlyList<Point> points)
        {
            var result = new List<Point> { points[0] };
            var f = 1D / Math.Abs(Maximum - Minimum);
            var beforeLastIndex = 0;
            var lastIndex = 0;
            double min = points[0].Y, max = points[0].Y;
            for (var i = 1; i < points.Count; i++)
            {
                var distX = (points[i].X - _points[lastIndex].X);
                if (distX * f > 0.01)
                {
                    var relY = 1D / Math.Abs(Math.Max(max, points[i].Y) - Math.Min(min, points[i].Y)) * (ActualHeight / ActualWidth);
                    double minAllowed, maxAllowed;

                    var avg = points.Skip(beforeLastIndex + 1).Take(i - beforeLastIndex).Average(x => x.Y);
                    if (beforeLastIndex > 0)
                    {
                        var prevDist = result[result.Count - 1] - result[result.Count - 2];
                        var anglePrev = Math.Atan((prevDist.Y * relY) / (prevDist.X * f));
                        var maxNextAngle = Math.Min(Math.PI / 3, anglePrev + distX * f * 100 * Math.Max(0.5, Math.Pow(anglePrev, 2)));
                        var minNextAngle = Math.Max(-Math.PI / 3, anglePrev - distX * f * 100 * Math.Max(0.5, Math.Pow(anglePrev, 2)));
                        var y1 = result[result.Count - 1].Y + Math.Tan(maxNextAngle) * distX * f / relY;
                        var y2 = result[result.Count - 1].Y + Math.Tan(minNextAngle) * distX * f / relY;
                        maxAllowed = Math.Max(y1, y2);
                        minAllowed = Math.Min(y1, y2);
                    }
                    else
                    {
                        maxAllowed = avg;
                        minAllowed = avg;
                    }
                    var desiredY = avg;
                    result.Add(new Point(points[i].X, Math.Min(maxAllowed, Math.Max(minAllowed, desiredY))));
                    if (result.Last().Y < min)
                        min = result.Last().Y;
                    if (result.Last().Y > max)
                        max = result.Last().Y;

                    beforeLastIndex = lastIndex;
                    lastIndex = i;
                }
            }
            var d = points.Skip(beforeLastIndex + 1);
            if (d.Count() < 20)
            {
                var id = points.Count <= 20 ? 0 : points.Count - 20;
                d = points.Skip(id);
            }
            if (_points.Skip(beforeLastIndex + 1).Any())
                result.Add(new Point(points.Last().X, d.Average(x => x.Y)));
            return result;
        }

        /// <summary>
        /// Refreshes the graph in the graph-mode. This method is called at the end of the SetProgress-Method and when the GraphMode is set to true.
        /// </summary>
        public void RefreshGraph()
        {
            if (_points.Count == 0)
                return;

            //create smoothed points
            var smoothedPoints = Algo1(_points);
            double maxDispValue = smoothedPoints.Max(x => x.Y), minDispValue = smoothedPoints.Min(x => x.Y);


            //Set the current value
            if ((DateTime.Now - _lastValueSet).TotalSeconds >= 0.2)
            {
                CurrentSpeed = maxDispValue - smoothedPoints.Last().Y;
                try
                {
                    _currentSpeed.Text = String.Format(CurrentSpeedFormat, CurrentSpeed);
                }
                catch (Exception)
                {
                    _currentSpeed.Text = "Format-Error";
                }
                _lastValueSet = DateTime.Now;
            }

            var allHeight = (maxDispValue - minDispValue);
            double hu = 0.2, hd = 1 - hu; //Percent-values of the height of the graph and the border on top
            var upSpace = allHeight / hd * hu; //Space at the top of the graph to the upper border

            for (var i = 0; i < smoothedPoints.Count; i++)
                smoothedPoints[i] = new Point(smoothedPoints[i].X, smoothedPoints[i].Y - minDispValue + upSpace);


            //draw graph
            if (_graphPath != null)
            {
                var streamGeo = new StreamGeometry();
                var ctx = streamGeo.Open();
                ctx.BeginFigure(new Point(0, allHeight + upSpace), true, false);
                ctx.LineTo(new Point(0, 0), true, false);
                ctx.LineTo(new Point(0, allHeight + upSpace), true, false);
                if (smoothedPoints.Count > 0)
                    ctx.LineTo(smoothedPoints[0], true, false);
                for (var i = 1; i < smoothedPoints.Count - 1; i++)
                {
                    Point p1 = GetPointBetween(smoothedPoints[i - 1], smoothedPoints[i]),
                        p2 = smoothedPoints[i],
                        p3 = GetPointBetween(smoothedPoints[i], smoothedPoints[i + 1]);
                    ctx.BezierTo(p1, p2, p3, true, false);
                }
                if (smoothedPoints.Count > 1)
                    ctx.LineTo(smoothedPoints.Last(), true, false);
                ctx.LineTo(new Point(Value, allHeight + upSpace), true, false);
                ctx.LineTo(new Point(Maximum, allHeight + upSpace), true, false);
                ctx.Close();
                _graphPath.Data = streamGeo;
            }

            //var nonSmoothedPoints = _points.ToList();
            //for (var i = 0; i < nonSmoothedPoints.Count; i++)
            //    nonSmoothedPoints[i] = new Point(nonSmoothedPoints[i].X, nonSmoothedPoints[i].Y - minDispValue + upSpace);

            ////draw graph
            //if (_graphPathRaw != null)
            //{
            //    var streamGeo = new StreamGeometry();
            //    var ctx = streamGeo.Open();
            //    ctx.BeginFigure(new Point(0, allHeight + upSpace), true, false);
            //    ctx.LineTo(new Point(0, 0), true, false);
            //    ctx.LineTo(new Point(0, allHeight + upSpace), true, false);
            //    if (nonSmoothedPoints.Count > 0)
            //        ctx.LineTo(nonSmoothedPoints[0], true, false);
            //    for (var i = 1; i < nonSmoothedPoints.Count - 1; i++)
            //    {
            //        Point p1 = GetPointBetween(nonSmoothedPoints[i - 1], nonSmoothedPoints[i]),
            //            p2 = nonSmoothedPoints[i],
            //            p3 = GetPointBetween(nonSmoothedPoints[i], nonSmoothedPoints[i + 1]);
            //        ctx.BezierTo(p1, p2, p3, true, false);
            //    }
            //    if (nonSmoothedPoints.Count > 1)
            //        ctx.LineTo(nonSmoothedPoints.Last(), true, false);
            //    ctx.LineTo(new Point(Value, allHeight + upSpace), true, false);
            //    ctx.LineTo(new Point(Maximum, allHeight + upSpace), true, false);
            //    ctx.Close();
            //    _graphPathRaw.Data = streamGeo;
            //}

            //set line to right position
            if (Math.Abs(maxDispValue) > 0 && !double.IsNaN(maxDispValue) && _line != null)
                _line.Margin = new Thickness(0, 0, 0, hd * ActualHeight * (((allHeight + upSpace) - smoothedPoints.Last().Y) / allHeight));
        }

        /// <summary>
        /// Sets the ProgressBar to the start-state
        /// </summary>
        public void ResetProgressBar()
        {
            Value = 0;
            if (_graphPath != null)
                _graphPath.Data = null;
            _maxValue = 0;
            _points?.Clear();
            _lastValueSet = DateTime.MinValue;
            RefreshGraph();
        }

        #endregion
    }
}
