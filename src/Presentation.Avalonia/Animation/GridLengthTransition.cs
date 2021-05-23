using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using System;
using System.Reactive.Linq;

namespace MaSch.Presentation.Avalonia.Animation
{
    /// <summary>
    /// Transition class that handles <see cref="AvaloniaProperty"/> with <see cref="GridLength"/> types.
    /// </summary>
    public class GridLengthTransition : Transition<GridLength>
    {
        private static readonly GridLengthAnimator _animator = new GridLengthAnimator();

        /// <inheritdoc/>
        public override IObservable<GridLength> DoTransition(IObservable<double> progress, GridLength oldValue, GridLength newValue)
        {
            return progress.Select(p => _animator.Interpolate(Easing.Ease(p), oldValue, newValue));
        }
    }
}
