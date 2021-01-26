using System.Windows;
using System.Windows.Media.Animation;

namespace MaSch.Presentation.Wpf.Extensions
{
    public static class TimelineExtensions
    {
        public static Timeline SetTarget(this Timeline timeline, string targetName, DependencyProperty targetProperty)
        {
            Storyboard.SetTargetName(timeline, targetName);
            Storyboard.SetTargetProperty(timeline, new PropertyPath(targetProperty));
            return timeline;
        }

        public static Timeline SetTarget(this Timeline timeline, DependencyObject target, DependencyProperty targetProperty)
        {
            Storyboard.SetTarget(timeline, target);
            Storyboard.SetTargetProperty(timeline, new PropertyPath(targetProperty));
            return timeline;
        }

        public static Timeline SetTarget(this Timeline timeline, string targetName, PropertyPath targetProperty)
        {
            Storyboard.SetTargetName(timeline, targetName);
            Storyboard.SetTargetProperty(timeline, targetProperty);
            return timeline;
        }

        public static Timeline SetTarget(this Timeline timeline, DependencyObject target, PropertyPath targetProperty)
        {
            Storyboard.SetTarget(timeline, target);
            Storyboard.SetTargetProperty(timeline, targetProperty);
            return timeline;
        }
    }
}
