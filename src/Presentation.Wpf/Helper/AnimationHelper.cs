using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MaSch.Presentation.Wpf.Helper
{
    /// <summary>
    /// Specifies the direction of the switch animation.
    /// </summary>
    public enum SwitchDirection
    {
        /// <summary>
        /// Switches the view to the left.
        /// </summary>
        Left,

        /// <summary>
        /// Switches the view to the right.
        /// </summary>
        Right,

        /// <summary>
        /// Switches the view down.
        /// </summary>
        Down,

        /// <summary>
        /// Switches the view up.
        /// </summary>
        Up,
    }

    /// <summary>
    /// Helper methods for animations.
    /// </summary>
    public static class AnimationHelper
    {
        /// <summary>
        /// Adds a new DoubleAnimation to the given storyboard that animates the property FrameworkElement.OpacityProperty with the given informations.
        /// </summary>
        /// <param name="storyboard">the storyboard to add the animation.</param>
        /// <param name="to">the new opacity-value of the FrameworkElement.</param>
        /// <param name="duration">the duration for the animation.</param>
        /// <param name="element">the element to animate.</param>
        /// <param name="beginTime">the begin time of the animation.</param>
        public static void AddOpacityAnimationToStoryboard(Storyboard storyboard, double to, TimeSpan duration, UIElement element, TimeSpan beginTime)
        {
            var da = new DoubleAnimation(to, new Duration(duration)) { BeginTime = beginTime };
            Storyboard.SetTarget(da, element);
            Storyboard.SetTargetProperty(da, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(da);
        }

        /// <summary>
        /// Begins an animation that switches two views.
        /// </summary>
        /// <param name="from">The view to hide.</param>
        /// <param name="to">The view to show.</param>
        /// <param name="direction">The direction the switch should happen.</param>
        public static void SwitchViews(UIElement from, UIElement to, SwitchDirection direction)
        {
            var opdir = direction switch
            {
                SwitchDirection.Left => SwitchDirection.Right,
                SwitchDirection.Right => SwitchDirection.Left,
                SwitchDirection.Down => SwitchDirection.Up,
                SwitchDirection.Up => SwitchDirection.Down,
                _ => SwitchDirection.Left,
            };
            Switch(to, opdir, beginTime: TimeSpan.FromSeconds(0.2));
            Switch(from, opdir, false);
        }

        /// <summary>
        /// Begins an animation that swicthes the control out or into view.
        /// </summary>
        /// <param name="control">The control to attach the animation to.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="fromHidden">if set to <c>true</c> the control will be shown.</param>
        /// <param name="beginTime">The begin time.</param>
        /// <param name="durationSec">The duration in seconds.</param>
        public static void Switch(UIElement control, SwitchDirection direction, bool fromHidden = true, TimeSpan? beginTime = null, double durationSec = 0.2)
        {
            var duration = new Duration(TimeSpan.FromSeconds(durationSec));
            var opacityAnimation = fromHidden ? new DoubleAnimation(1, duration) : new DoubleAnimation(0, duration);
            var translateAnimation = new DoubleAnimation(0, 0, duration);
            if (beginTime.HasValue)
            {
                opacityAnimation.BeginTime = beginTime.Value;
                translateAnimation.BeginTime = beginTime.Value;
            }

            switch (direction)
            {
                case SwitchDirection.Left:
                case SwitchDirection.Up:
                    if (fromHidden)
                        translateAnimation.From = 50;
                    else
                        translateAnimation.To = -50;
                    break;
                case SwitchDirection.Right:
                case SwitchDirection.Down:
                    if (fromHidden)
                        translateAnimation.From = -50;
                    else
                        translateAnimation.To = 50;
                    break;
            }

            var trans = new TranslateTransform(0, 0);
            switch (direction)
            {
                case SwitchDirection.Left:
                case SwitchDirection.Right:
                    trans.X = translateAnimation.From ?? 0;
                    control.RenderTransform = trans;
                    trans.BeginAnimation(TranslateTransform.XProperty, translateAnimation);
                    break;
                case SwitchDirection.Down:
                case SwitchDirection.Up:
                    trans.Y = translateAnimation.From ?? 0;
                    control.RenderTransform = trans;
                    trans.BeginAnimation(TranslateTransform.YProperty, translateAnimation);
                    break;
            }

            control.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
        }
    }
}
