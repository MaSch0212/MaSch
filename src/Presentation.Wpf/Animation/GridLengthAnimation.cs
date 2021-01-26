using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media.Animation;

namespace MaSch.Presentation.Wpf.Animation
{
    /// <summary>
    /// Animates a grid length value just like the DoubleAnimation animates a double value
    /// </summary>
    public class GridLengthAnimation : AnimationTimeline
    {
        private bool _isCompleted;

        /// <summary>
        /// Marks the animation as completed
        /// </summary>
        public bool IsCompleted
        {
            get => _isCompleted;
            set => _isCompleted = value;
        }
        
        public EasingFunctionBase EasingFunction
        {
            get => (EasingFunctionBase)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(EasingFunctionBase), typeof(GridLengthAnimation), new PropertyMetadata(null));
        
        /// <summary>
        /// Sets the reverse value for the second animation
        /// </summary>
        public double ReverseValue
        {
            get => GetValue(ReverseValueProperty) as double? ?? 0D;
            set => SetValue(ReverseValueProperty, value);
        }

        /// <summary>
        /// Dependency property. Sets the reverse value for the second animation
        /// </summary>
        public static readonly DependencyProperty ReverseValueProperty =
            DependencyProperty.Register("ReverseValue", typeof(double), typeof(GridLengthAnimation), new UIPropertyMetadata(0.0));


        /// <summary>
        /// Returns the type of object to animate
        /// </summary>
        public override Type TargetPropertyType => typeof(GridLength);

        /// <summary>
        /// Creates an instance of the animation object
        /// </summary>
        /// <returns>Returns the instance of the GridLengthAnimation</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        /// <summary>
        /// Dependency property for the From property
        /// </summary>
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(GridLength),
                typeof(GridLengthAnimation));

        /// <summary>
        /// CLR Wrapper for the From depenendency property
        /// </summary>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public GridLength From
        {
            get => (GridLength)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        /// <summary>
        /// Dependency property for the To property
        /// </summary>
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(GridLength),
                typeof(GridLengthAnimation));

        /// <summary>
        /// CLR Wrapper for the To property
        /// </summary>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public GridLength To
        {
            get => (GridLength)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        private AnimationClock _clock;

        /// <summary>
        /// registers to the completed event of the animation clock
        /// </summary>
        /// <param name="clock">the animation clock to notify completion status</param>
        private void VerifyAnimationCompletedStatus(AnimationClock clock)
        {
            if (_clock == null)
            {
                _clock = clock;
                _clock.Completed += delegate { _isCompleted = true; };
            }
        }

        /// <summary>
        /// Animates the grid let set
        /// </summary>
        /// <param name="defaultOriginValue">The original value to animate</param>
        /// <param name="defaultDestinationValue">The final value</param>
        /// <param name="animationClock">The animation clock (timer)</param>
        /// <returns>Returns the new grid length to set</returns>
        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        public override object GetCurrentValue(object defaultOriginValue,
            object defaultDestinationValue, AnimationClock animationClock)
        {
            //check the animation clock event
            VerifyAnimationCompletedStatus(animationClock);

            //check if the animation was completed
            if (_isCompleted)
                return (GridLength)defaultDestinationValue;

            //if not then create the value to animate
            var fromVal = From.Value;
            var toVal = To.Value;

            //check if the value is already collapsed
            //if (((GridLength)defaultOriginValue).Value == toVal)
            //{
            //    fromVal = toVal;
            //    toVal = this.ReverseValue;
            //}
            //else
                //check to see if this is the last tick of the animation clock.
            // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (animationClock.CurrentProgress.Value == 1.0)
                    return To;

            if (fromVal > toVal)
                return new GridLength((1 - Ease(animationClock.CurrentProgress.Value)) *
                    (fromVal - toVal) + toVal, From.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            else
                return new GridLength(Ease(animationClock.CurrentProgress.Value) *
                    (toVal - fromVal) + fromVal, From.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
        }

        private double Ease(double value)
        {
            return EasingFunction?.Ease(value) ?? value;
        }
    }

    /// <summary>
    /// Animates a double value 
    /// </summary>
    public class ExpanderDoubleAnimation : DoubleAnimationBase
    {
        /// <summary>
        /// Dependency property for the From property
        /// </summary>
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(double?),
                typeof(ExpanderDoubleAnimation));

        /// <summary>
        /// CLR Wrapper for the From depenendency property
        /// </summary>
        public double? From
        {
            get => (double?)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        /// <summary>
        /// Dependency property for the To property
        /// </summary>
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(double?),
                typeof(ExpanderDoubleAnimation));

        /// <summary>
        /// CLR Wrapper for the To property
        /// </summary>
        public double? To
        {
            get => (double?)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        /// <summary>
        /// Sets the reverse value for the second animation
        /// </summary>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public double? ReverseValue
        {
            get => (double)GetValue(ReverseValueProperty);
            set => SetValue(ReverseValueProperty, value);
        }

        /// <summary>
        /// Sets the reverse value for the second animation
        /// </summary>
        public static readonly DependencyProperty ReverseValueProperty =
            DependencyProperty.Register("ReverseValue", typeof(double?), typeof(ExpanderDoubleAnimation), new UIPropertyMetadata(0.0));


        /// <summary>
        /// Creates an instance of the animation
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ExpanderDoubleAnimation();
        }

        /// <summary>
        /// Animates the double value
        /// </summary>
        /// <param name="defaultOriginValue">The original value to animate</param>
        /// <param name="defaultDestinationValue">The final value</param>
        /// <param name="animationClock">The animation clock (timer)</param>
        /// <returns>Returns the new double to set</returns>
        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
        {
            var fromVal = From.Value;
            var toVal = To.Value;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (defaultOriginValue == toVal)
            {
                fromVal = toVal;
                toVal = ReverseValue.Value;
            }

            if (fromVal > toVal)
                return (1 - animationClock.CurrentProgress.Value) *
                    (fromVal - toVal) + toVal;
            else
                return (animationClock.CurrentProgress.Value *
                    (toVal - fromVal) + fromVal);
        }
    }
}
