using Avalonia.Animation.Animators;
using Avalonia.Controls;

namespace MaSch.Presentation.Avalonia.Animation
{
    /// <summary>
    /// Animates a grid length value just like the <see cref="DoubleAnimator"/> animates a double value.
    /// </summary>
    public class GridLengthAnimator : Animator<GridLength>
    {
        /// <inheritdoc/>
        public override GridLength Interpolate(double progress, GridLength oldValue, GridLength newValue)
        {
            if (progress >= 0.995)
                return newValue;

            var fromVal = oldValue.Value;
            var toVal = newValue.Value;

            if (fromVal > toVal)
            {
                return new GridLength(
                    ((1 - progress) * (fromVal - toVal)) + toVal,
                    oldValue.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
            else
            {
                return new GridLength(
                    (progress * (toVal - fromVal)) + fromVal,
                    oldValue.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
        }
    }
}
