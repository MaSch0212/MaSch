using MaSch.Core;
using System.Windows;
using System.Windows.Media.Animation;

namespace MaSch.Presentation.Wpf.Animation
{
    /// <summary>
    /// Animates a double value.
    /// </summary>
    public class ExpanderDoubleAnimation : DoubleAnimationBase
    {
        /// <summary>
        /// Dependency property. Gets or sets the starting value for the animation.
        /// </summary>
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(double?), typeof(ExpanderDoubleAnimation));

        /// <summary>
        /// Dependency property. Gets or sets the ending value for the animation.
        /// </summary>
        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(double?), typeof(ExpanderDoubleAnimation));

        /// <summary>
        /// Dependency property. Gets or sets the reverse value for the second animation.
        /// </summary>
        public static readonly DependencyProperty ReverseValueProperty =
            DependencyProperty.Register("ReverseValue", typeof(double?), typeof(ExpanderDoubleAnimation), new UIPropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the starting value for the animation.
        /// </summary>
        public double? From
        {
            get => (double?)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        /// <summary>
        /// Gets or sets the ending value for the animation.
        /// </summary>
        public double? To
        {
            get => (double?)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        /// <summary>
        /// Gets or sets the reverse value for the second animation.
        /// </summary>
        public double? ReverseValue
        {
            get => (double)GetValue(ReverseValueProperty);
            set => SetValue(ReverseValueProperty, value);
        }

        /// <summary>
        /// Creates an instance of the animation.
        /// </summary>
        /// <returns>Returns the instance of the <see cref="ExpanderDoubleAnimation"/>.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ExpanderDoubleAnimation();
        }

        /// <summary>
        /// Animates the double value.
        /// </summary>
        /// <param name="defaultOriginValue">The original value to animate.</param>
        /// <param name="defaultDestinationValue">The final value.</param>
        /// <param name="animationClock">The animation clock (timer).</param>
        /// <returns>Returns the new double to set.</returns>
        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
        {
            var fromVal = Guard.NotNull(From, nameof(From)).Value;
            var toVal = Guard.NotNull(To, nameof(To)).Value;

            if (defaultOriginValue == toVal && ReverseValue.HasValue)
            {
                fromVal = toVal;
                toVal = ReverseValue.Value;
            }

            if (fromVal > toVal)
                return ((1 - (animationClock.CurrentProgress ?? 0)) * (fromVal - toVal)) + toVal;
            else
                return ((animationClock.CurrentProgress ?? 0) * (toVal - fromVal)) + fromVal;
        }
    }
}
