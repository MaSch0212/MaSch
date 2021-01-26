using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MaSch.Presentation.Wpf.Helper
{
    public enum SwitchDirection { Left, Right, Down, Up }
    public class AnimationHelper
    {
        /// <summary>
        /// Adds a new DoubleAnimation to the given storyboard that animates the property FrameworkElement.OpacityProperty with the given informations
        /// </summary>
        /// <param name="storyboard">the storyboard to add the animation</param>
        /// <param name="to">the new opacity-value of the FrameworkElement</param>
        /// <param name="duration">the duration for the animation</param>
        /// <param name="element">the element to animate</param>
        /// <param name="beginTime">the begin time of the animation</param>
        public static void AddOpacityAnimationToStoryboard(Storyboard storyboard, double to, TimeSpan duration, UIElement element, TimeSpan beginTime)
        {
            var da = new DoubleAnimation(to, new Duration(duration)) { BeginTime = beginTime };
            Storyboard.SetTarget(da, element);
            Storyboard.SetTargetProperty(da, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(da);
        }

        public static void SwitchViews(UIElement from, UIElement to, SwitchDirection direction)
        {
            SwitchDirection opdir;
            switch (direction)
            {
                case SwitchDirection.Left:
                    opdir = SwitchDirection.Right;
                    break;
                case SwitchDirection.Right:
                    opdir = SwitchDirection.Left;
                    break;
                case SwitchDirection.Down:
                    opdir = SwitchDirection.Up;
                    break;
                case SwitchDirection.Up:
                    opdir = SwitchDirection.Down;
                    break;
                default:
                    opdir = SwitchDirection.Left;
                    break;
            }
            Switch(to, opdir, beginTime: TimeSpan.FromSeconds(0.2));
            Switch(from, opdir, false);
        }

        public static void Switch(UIElement control, SwitchDirection direction, bool fromHidden = true, TimeSpan? beginTime = null, double durationSec = 0.2)
        {
            var dur = new Duration(TimeSpan.FromSeconds(durationSec));
            var daOp = fromHidden ? new DoubleAnimation(1, dur) : new DoubleAnimation(0, dur);
            var daTrans = new DoubleAnimation(0, 0, dur);
            if (beginTime.HasValue)
            {
                daOp.BeginTime = beginTime.Value;
                daTrans.BeginTime = beginTime.Value;
            }

            switch (direction)
            {
                case SwitchDirection.Left:
                case SwitchDirection.Up:
                    if (fromHidden)
                        daTrans.From = 50;
                    else
                        daTrans.To = -50;
                    break;
                case SwitchDirection.Right:
                case SwitchDirection.Down:
                    if (fromHidden)
                        daTrans.From = -50;
                    else
                        daTrans.To = 50;
                    break;
            }

            var trans = new TranslateTransform(0, 0);
            switch (direction)
            {
                case SwitchDirection.Left:
                case SwitchDirection.Right:
                    trans.X = daTrans.From ?? 0;
                    control.RenderTransform = trans;
                    trans.BeginAnimation(TranslateTransform.XProperty, daTrans);
                    break;
                case SwitchDirection.Down:
                case SwitchDirection.Up:
                    trans.Y = daTrans.From ?? 0;
                    control.RenderTransform = trans;
                    trans.BeginAnimation(TranslateTransform.YProperty, daTrans);
                    break;
            }
            control.BeginAnimation(UIElement.OpacityProperty, daOp);
        }
    }
}
