using MaSch.Presentation.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace MaSch.Presentation.Wpf.Controls
{
    /// <summary>
    /// Content presenter that can transition between contents using an animation.
    /// </summary>
    /// <seealso cref="ContentControl" />
    public class TransitionContentPresenter : ContentControl
    {
        /// <summary>
        /// Dependency property. Gets or sets the animation for transitioning content into view.
        /// </summary>
        public static readonly DependencyProperty TransitionInProperty =
            DependencyProperty.Register(
                "TransitionIn",
                typeof(TransitionInType),
                typeof(TransitionContentPresenter),
                new PropertyMetadata(TransitionInType.None));

        /// <summary>
        /// Dependency property. Gets or sets the animation for transitioning content out of view.
        /// </summary>
        public static readonly DependencyProperty TransitionOutProperty =
            DependencyProperty.Register(
                "TransitionOut",
                typeof(TransitionOutType),
                typeof(TransitionContentPresenter),
                new PropertyMetadata(TransitionOutType.None));

        /// <summary>
        /// Dependency property. Gets or sets the duration of the transition animation.
        /// </summary>
        public static readonly DependencyProperty TransitionDurationProperty =
            DependencyProperty.Register(
                "TransitionDuration",
                typeof(TimeSpan),
                typeof(TransitionContentPresenter),
                new PropertyMetadata(TimeSpan.FromSeconds(0.2)));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether to animate the first content of the <see cref="TransitionContentPresenter"/>.
        /// </summary>
        public static readonly DependencyProperty TransitionFirstContentProperty =
            DependencyProperty.Register(
                "TransitionFirstContent",
                typeof(bool),
                typeof(TransitionContentPresenter),
                new PropertyMetadata(true));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether in and out animations should run simultaneously or after each other.
        /// </summary>
        public static readonly DependencyProperty RunAnimationsSimultaneouslyProperty =
            DependencyProperty.Register(
                "RunAnimationsSimultaneously",
                typeof(bool),
                typeof(TransitionContentPresenter),
                new PropertyMetadata(true));

        /// <summary>
        /// Dependency property. Gets or sets the easing function to use for the animations.
        /// </summary>
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(TransitionContentPresenter),
                new PropertyMetadata(null));

        private int _currentlyActive;
        private Storyboard? _lastStoryboard;
        private ContentPresenter? _content1;
        private ContentPresenter? _content2;
        private Grid? _contentGrid;

        static TransitionContentPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TransitionContentPresenter), new FrameworkPropertyMetadata(typeof(TransitionContentPresenter)));
        }

        /// <summary>
        /// Occurs when the content changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler? ContentChanged;

        /// <summary>
        /// Gets or sets the animation for transitioning content into view.
        /// </summary>
        public TransitionInType TransitionIn
        {
            get => GetValue(TransitionInProperty) as TransitionInType? ?? TransitionInType.None;
            set => SetValue(TransitionInProperty, value);
        }

        /// <summary>
        /// Gets or sets the animation for transitioning content out of view.
        /// </summary>
        public TransitionOutType TransitionOut
        {
            get => GetValue(TransitionOutProperty) as TransitionOutType? ?? TransitionOutType.None;
            set => SetValue(TransitionOutProperty, value);
        }

        /// <summary>
        /// Gets or sets the duration of the transition animation.
        /// </summary>
        public TimeSpan TransitionDuration
        {
            get => GetValue(TransitionDurationProperty) as TimeSpan? ?? TimeSpan.FromSeconds(0.2);
            set => SetValue(TransitionDurationProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to animate the first content of the <see cref="TransitionContentPresenter"/>.
        /// </summary>
        public bool TransitionFirstContent
        {
            get => GetValue(TransitionFirstContentProperty) as bool? ?? true;
            set => SetValue(TransitionFirstContentProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether in and out animations should run simultaneously or after each other.
        /// </summary>
        public bool RunAnimationsSimultaneously
        {
            get => GetValue(RunAnimationsSimultaneouslyProperty) as bool? ?? true;
            set => SetValue(RunAnimationsSimultaneouslyProperty, value);
        }

        /// <summary>
        /// Gets or sets the easing function to use for the animations.
        /// </summary>
        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            _contentGrid = GetTemplateChild("PART_ContentGrid") as Grid ?? throw new KeyNotFoundException("Control could not be found: PART_ContentGrid");
            var contents = _contentGrid.Children.OfType<ContentPresenter>().Take(2).ToArray();
            if (contents.Length != 2)
                throw new ArgumentException("The PART_ContentGrid needs to have at least 2 ContenPresenters as Children.");
            _content1 = contents[0];
            _content2 = contents[1];

            _content1.Visibility = Visibility.Hidden;
            _content2.Visibility = Visibility.Hidden;
            _content1.RenderTransformOrigin = new Point(0.5, 0.5);
            _content2.RenderTransformOrigin = new Point(0.5, 0.5);

            if (Content != null)
                Loaded += (s, e) => OnContentChanged(null, Content);
        }

        /// <inheritdoc/>
        protected override void OnContentChanged(object? oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            if (_content1 == null || _content2 == null)
                return;

            var fadeOut = _currentlyActive == 0 ? _content1 : _content2;
            var fadeIn = _currentlyActive == 0 ? _content2 : _content1;

            _contentGrid!.Children.Remove(fadeIn);
            _contentGrid.Children.Insert(_contentGrid.Children.IndexOf(fadeOut) + 1, fadeIn);

            var transitionIn = TransitionIn;
            var transitionOut = TransitionOut;
            if (oldContent == null && !TransitionFirstContent)
                transitionIn = TransitionInType.None;
            var storyboard = new Storyboard { Duration = TransitionDuration };
            if (!RunAnimationsSimultaneously && transitionIn != TransitionInType.None && oldContent != null)
                storyboard.Duration += TransitionDuration;

            fadeIn.Content = newContent;
            var beginTime = !RunAnimationsSimultaneously && transitionIn != TransitionInType.None && oldContent != null ? TransitionDuration : TimeSpan.Zero;
            storyboard.Children.Add(CreateObjectAnimation(storyboard.Duration, Visibility.Visible, beginTime == TimeSpan.Zero ? 0 : 0.5, fadeIn, VisibilityProperty));
            FadeIn(fadeIn, transitionIn, EasingFunction, beginTime, TransitionDuration, storyboard);

            storyboard.Children.Add(CreateObjectAnimation(TransitionDuration, Visibility.Visible, 0, fadeOut, VisibilityProperty));
            storyboard.Children.Add(CreateObjectAnimation(TransitionDuration, null, 1, fadeOut, ContentPresenter.ContentProperty));
            storyboard.Children.Add(CreateObjectAnimation(TransitionDuration, Visibility.Hidden, 1, fadeOut, VisibilityProperty));
            FadeOut(fadeOut, transitionOut, EasingFunction, TimeSpan.Zero, TransitionDuration, storyboard);

            _lastStoryboard?.Stop();
            _lastStoryboard = null;
            if (storyboard.Children.Any())
            {
                _lastStoryboard = storyboard;
                storyboard.Begin();
            }

            _currentlyActive = (_currentlyActive + 1) % 2;
            ContentChanged?.Invoke(this, new DependencyPropertyChangedEventArgs(ContentProperty, oldContent, newContent));
        }

        /// <summary>
        /// Fades a specific content presenter into view.
        /// </summary>
        /// <param name="control">The control to fade in.</param>
        /// <param name="transition">The transition to use.</param>
        /// <param name="easing">The easing function to apply on the animation.</param>
        /// <param name="beginTime">The begin time of the animation.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="targetStoryboard">The target storyboard.</param>
        protected virtual void FadeIn(ContentPresenter control, TransitionInType transition, IEasingFunction easing, TimeSpan beginTime, Duration duration, Storyboard targetStoryboard)
        {
            TransformGroup transform = control.RenderTransform as TransformGroup ?? new TransformGroup();
            control.RenderTransform = transform;

            if (transition.HasFlag(TransitionInType.Fade))
            {
                targetStoryboard.Children.Add(
                    CreateDoubleAnimation(0, 1, beginTime, duration, easing).SetTarget(control, OpacityProperty));
            }

            var blur = control.Effect as BlurEffect;
            if (transition.HasFlag(TransitionInType.Blur))
            {
                blur ??= new BlurEffect
                {
                    Radius = 32,
                    KernelType = KernelType.Gaussian,
                    RenderingBias = RenderingBias.Performance,
                };
                control.Effect = blur;
                targetStoryboard.Children.Add(
                    CreateDoubleAnimation(32, 0, beginTime, duration, easing).SetTarget(control, new PropertyPath("Effect.Radius", Array.Empty<object>())));
            }
            else if (blur != null)
            {
                targetStoryboard.Children.Add(
                    CreateDoubleAnimation(0, 0, beginTime, duration, easing).SetTarget(control, new PropertyPath("Effect.Radius", Array.Empty<object>())));
            }

            var scale = transform.Children.OfType<ScaleTransform>().FirstOrDefault();
            if (transition.HasFlag(TransitionInType.ZoomToUser) || transition.HasFlag(TransitionInType.ZoomFromUser))
            {
                var x = transition.HasFlag(TransitionInType.ZoomToUser) ? 0 : 5;
                var y = transition.HasFlag(TransitionInType.ZoomToUser) ? 0 : 5;
                if (scale == null)
                {
                    scale = new ScaleTransform
                    {
                        ScaleX = x,
                        ScaleY = y,
                    };
                    transform.Children.Add(scale);
                }

                targetStoryboard.Children.Add(CreateDoubleAnimation(x, 1, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(scale)}].ScaleX", Array.Empty<object>())));
                targetStoryboard.Children.Add(CreateDoubleAnimation(y, 1, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(scale)}].ScaleY", Array.Empty<object>())));
            }
            else if (scale != null)
            {
                targetStoryboard.Children.Add(CreateDoubleAnimation(1, 1, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(scale)}].ScaleX", Array.Empty<object>())));
                targetStoryboard.Children.Add(CreateDoubleAnimation(1, 1, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(scale)}].ScaleY", Array.Empty<object>())));
            }

            var translate = transform.Children.OfType<TranslateTransform>().FirstOrDefault();
            if (transition.HasFlag(TransitionInType.SlideInFromBottom) || transition.HasFlag(TransitionInType.SlideInFromLeft) ||
                transition.HasFlag(TransitionInType.SlideInFromRight) || transition.HasFlag(TransitionInType.SlideInFromTop))
            {
                var width = control.ActualWidth;
                var height = control.ActualHeight;
                var factor = transition.HasFlag(TransitionInType.SlideOnlyQuater) ? 0.25D : (transition.HasFlag(TransitionInType.SlideOnlyHalf) ? 0.5D : 1D);
                var x = transition.HasFlag(TransitionInType.SlideInFromLeft) ? -(width * factor) : (transition.HasFlag(TransitionInType.SlideInFromRight) ? (width * factor) : 0);
                var y = transition.HasFlag(TransitionInType.SlideInFromTop) ? -(height * factor) : (transition.HasFlag(TransitionInType.SlideInFromBottom) ? (height * factor) : 0);
                if (translate == null)
                {
                    translate = new TranslateTransform
                    {
                        X = x,
                        Y = y,
                    };
                    transform.Children.Add(translate);
                }

                targetStoryboard.Children.Add(CreateDoubleAnimation(x, 0, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(translate)}].X", Array.Empty<object>())));
                targetStoryboard.Children.Add(CreateDoubleAnimation(y, 0, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(translate)}].Y", Array.Empty<object>())));
            }
            else if (translate != null)
            {
                targetStoryboard.Children.Add(CreateDoubleAnimation(0, 0, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(translate)}].X", Array.Empty<object>())));
                targetStoryboard.Children.Add(CreateDoubleAnimation(0, 0, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(translate)}].Y", Array.Empty<object>())));
            }
        }

        /// <summary>
        /// Fades a specific content presenter out of view.
        /// </summary>
        /// <param name="control">The control to fade out.</param>
        /// <param name="transition">The transition to use.</param>
        /// <param name="easing">The easing function to apply on the animation.</param>
        /// <param name="beginTime">The begin time of the animation.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="targetStoryboard">The target storyboard.</param>
        protected virtual void FadeOut(ContentPresenter control, TransitionOutType transition, IEasingFunction easing, TimeSpan beginTime, Duration duration, Storyboard targetStoryboard)
        {
            TransformGroup transform = control.RenderTransform as TransformGroup ?? new TransformGroup();
            control.RenderTransform = transform;

            if (transition.HasFlag(TransitionOutType.Fade))
            {
                targetStoryboard.Children.Add(
                    CreateDoubleAnimation(1, 0, beginTime, duration, easing).SetTarget(control, OpacityProperty));
            }

            var blur = control.Effect as BlurEffect;
            if (transition.HasFlag(TransitionOutType.Blur))
            {
                blur ??= new BlurEffect
                {
                    Radius = 0,
                    KernelType = KernelType.Gaussian,
                    RenderingBias = RenderingBias.Performance,
                };
                control.Effect = blur;
                targetStoryboard.Children.Add(
                    CreateDoubleAnimation(0, 32, beginTime, duration, easing).SetTarget(control, new PropertyPath("Effect.Radius", Array.Empty<object>())));
            }
            else if (blur != null)
            {
                targetStoryboard.Children.Add(
                    CreateDoubleAnimation(0, 0, beginTime, duration, easing).SetTarget(control, new PropertyPath("Effect.Radius", Array.Empty<object>())));
            }

            var scale = transform.Children.OfType<ScaleTransform>().FirstOrDefault();
            if (transition.HasFlag(TransitionOutType.ZoomToUser) || transition.HasFlag(TransitionOutType.ZoomFromUser))
            {
                var x = transition.HasFlag(TransitionOutType.ZoomToUser) ? 5 : 0;
                var y = transition.HasFlag(TransitionOutType.ZoomToUser) ? 5 : 0;
                if (scale == null)
                {
                    scale = new ScaleTransform
                    {
                        ScaleX = 1,
                        ScaleY = 1,
                    };
                    transform.Children.Add(scale);
                }

                targetStoryboard.Children.Add(CreateDoubleAnimation(1, x, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(scale)}].ScaleX", Array.Empty<object>())));
                targetStoryboard.Children.Add(CreateDoubleAnimation(1, y, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(scale)}].ScaleY", Array.Empty<object>())));
            }
            else if (scale != null)
            {
                targetStoryboard.Children.Add(CreateDoubleAnimation(1, 1, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(scale)}].ScaleX", Array.Empty<object>())));
                targetStoryboard.Children.Add(CreateDoubleAnimation(1, 1, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(scale)}].ScaleY", Array.Empty<object>())));
            }

            var translate = transform.Children.OfType<TranslateTransform>().FirstOrDefault();
            if (transition.HasFlag(TransitionOutType.SlideOutToBottom) || transition.HasFlag(TransitionOutType.SlideOutToLeft) ||
                transition.HasFlag(TransitionOutType.SlideOutToRight) || transition.HasFlag(TransitionOutType.SlideOutToTop))
            {
                var width = control.ActualWidth;
                var height = control.ActualHeight;
                var factor = transition.HasFlag(TransitionOutType.SlideOnlyQuater) ? 0.25D : (transition.HasFlag(TransitionOutType.SlideOnlyHalf) ? 0.5D : 1D);
                var x = transition.HasFlag(TransitionOutType.SlideOutToLeft) ? -(width * factor) : (transition.HasFlag(TransitionOutType.SlideOutToRight) ? (width * factor) : 0);
                var y = transition.HasFlag(TransitionOutType.SlideOutToTop) ? -(height * factor) : (transition.HasFlag(TransitionOutType.SlideOutToBottom) ? (height * factor) : 0);
                if (translate == null)
                {
                    translate = new TranslateTransform
                    {
                        X = 0,
                        Y = 0,
                    };
                    transform.Children.Add(translate);
                }

                targetStoryboard.Children.Add(CreateDoubleAnimation(0, x, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(translate)}].X", Array.Empty<object>())));
                targetStoryboard.Children.Add(CreateDoubleAnimation(0, y, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(translate)}].Y", Array.Empty<object>())));
            }
            else if (translate != null)
            {
                targetStoryboard.Children.Add(CreateDoubleAnimation(0, 0, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(translate)}].X", Array.Empty<object>())));
                targetStoryboard.Children.Add(CreateDoubleAnimation(0, 0, beginTime, duration, easing).SetTarget(control, new PropertyPath($"RenderTransform.Children[{transform.Children.IndexOf(translate)}].Y", Array.Empty<object>())));
            }
        }

        private static ObjectAnimationUsingKeyFrames CreateObjectAnimation(Duration duration, object? value, double percent, DependencyObject target, DependencyProperty property)
        {
            var result = new ObjectAnimationUsingKeyFrames { Duration = duration };
            _ = result.KeyFrames.Add(new DiscreteObjectKeyFrame(value, KeyTime.FromPercent(percent)));
            _ = result.SetTarget(target, property);
            return result;
        }

        private static DoubleAnimationUsingKeyFrames CreateDoubleAnimation(double from, double to, TimeSpan beginTime, Duration duration, IEasingFunction easing)
        {
            var result = new DoubleAnimationUsingKeyFrames { Duration = duration + beginTime };
            _ = result.KeyFrames.Add(new EasingDoubleKeyFrame(from, KeyTime.FromPercent(0), easing));
            if (beginTime > TimeSpan.Zero)
                _ = result.KeyFrames.Add(new EasingDoubleKeyFrame(from, KeyTime.FromPercent(0.5), easing));
            _ = result.KeyFrames.Add(new EasingDoubleKeyFrame(to, KeyTime.FromPercent(1), easing));
            return result;
        }
    }
}
