using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace MaSch.Presentation.Wpf.Animation
{
    /// <summary>
    /// Animates a grid length value just like the DoubleAnimation animates a double value.
    /// </summary>
    /// <remarks>
    /// Credit: https://social.msdn.microsoft.com/Forums/vstudio/en-US/da47a4b8-4d39-4d6e-a570-7dbe51a842e4/gridlengthanimation.
    /// </remarks>
    public class GridLengthAnimation : AnimationTimeline
    {
        private bool _isCompleted;
        private AnimationClock _clock;

        /// <summary>
        /// Dependency property. Gets or sets the easing function to use for this animation.
        /// </summary>
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(EasingFunctionBase), typeof(GridLengthAnimation), new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Sets the reverse value for the second animation.
        /// </summary>
        public static readonly DependencyProperty ReverseValueProperty =
            DependencyProperty.Register("ReverseValue", typeof(double), typeof(GridLengthAnimation), new UIPropertyMetadata(0.0));

        /// <summary>
        /// Dependency property. Gets or sets the starting value for the animation.
        /// </summary>
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(GridLength), typeof(GridLengthAnimation));

        /// <summary>
        /// Dependency property. Gets or sets the ending value for the animation.
        /// </summary>
        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(GridLength), typeof(GridLengthAnimation));

        /// <summary>
        /// Gets or sets the easing function to use for this animation.
        /// </summary>
        public EasingFunctionBase EasingFunction
        {
            get => (EasingFunctionBase)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        /// <summary>
        /// Gets or sets the reverse value for the second animation.
        /// </summary>
        public double ReverseValue
        {
            get => GetValue(ReverseValueProperty) as double? ?? 0D;
            set => SetValue(ReverseValueProperty, value);
        }

        /// <summary>
        /// Gets or sets the starting value for the animation.
        /// </summary>
        public GridLength From
        {
            get => (GridLength)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        /// <summary>
        /// Gets or sets the ending value for the animation.
        /// </summary>
        public GridLength To
        {
            get => (GridLength)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the animation is completed.
        /// </summary>
        public bool IsCompleted
        {
            get => _isCompleted;
            set => _isCompleted = value;
        }

        /// <summary>
        /// Gets the type of object to animate.
        /// </summary>
        public override Type TargetPropertyType => typeof(GridLength);

        /// <summary>
        /// Creates an instance of the animation object.
        /// </summary>
        /// <returns>Returns the instance of the <see cref="GridLengthAnimation"/>.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        /// <summary>
        /// Registers to the completed event of the animation clock.
        /// </summary>
        /// <param name="clock">The animation clock to notify completion status.</param>
        private void VerifyAnimationCompletedStatus(AnimationClock clock)
        {
            if (_clock == null)
            {
                _clock = clock;
                _clock.Completed += (sender, e) => { _isCompleted = true; };
            }
        }

        /// <summary>
        /// Animates the grid let set.
        /// </summary>
        /// <param name="defaultOriginValue">The original value to animate.</param>
        /// <param name="defaultDestinationValue">The final value.</param>
        /// <param name="animationClock">The animation clock (timer).</param>
        /// <returns>Returns the new grid length to set.</returns>
        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            // check the animation clock event
            VerifyAnimationCompletedStatus(animationClock);

            // check if the animation was completed
            if (_isCompleted)
                return (GridLength)defaultDestinationValue;

            // if not then create the value to animate
            var fromVal = From.Value;
            var toVal = To.Value;

            if (animationClock.CurrentProgress.Value == 1.0)
                return To;

            if (fromVal > toVal)
            {
                return new GridLength(
                    ((1 - Ease(animationClock.CurrentProgress.Value)) * (fromVal - toVal)) + toVal,
                    From.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
            else
            {
                return new GridLength(
                    (Ease(animationClock.CurrentProgress.Value) * (toVal - fromVal)) + fromVal,
                    From.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
        }

        private double Ease(double value)
        {
            return EasingFunction?.Ease(value) ?? value;
        }
    }
}
