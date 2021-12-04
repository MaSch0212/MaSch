using System.Windows;
using System.Windows.Media.Animation;

namespace MaSch.Presentation.Wpf.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="Timeline"/> class.
/// </summary>
public static class TimelineExtensions
{
    /// <summary>
    /// Sets the target object and property.
    /// </summary>
    /// <param name="timeline">The timeline.</param>
    /// <param name="targetName">Name of the target object.</param>
    /// <param name="targetProperty">The target property.</param>
    /// <returns>A reference to the given <see cref="Timeline"/> instance.</returns>
    public static Timeline SetTarget(this Timeline timeline, string targetName, DependencyProperty targetProperty)
    {
        Storyboard.SetTargetName(timeline, targetName);
        Storyboard.SetTargetProperty(timeline, new PropertyPath(targetProperty));
        return timeline;
    }

    /// <summary>
    /// Sets the target object and property.
    /// </summary>
    /// <param name="timeline">The timeline.</param>
    /// <param name="target">The target object.</param>
    /// <param name="targetProperty">The target property.</param>
    /// <returns>A reference to the given <see cref="Timeline"/> instance.</returns>
    public static Timeline SetTarget(this Timeline timeline, DependencyObject target, DependencyProperty targetProperty)
    {
        Storyboard.SetTarget(timeline, target);
        Storyboard.SetTargetProperty(timeline, new PropertyPath(targetProperty));
        return timeline;
    }

    /// <summary>
    /// Sets the target object and property.
    /// </summary>
    /// <param name="timeline">The timeline.</param>
    /// <param name="targetName">Name of the target object.</param>
    /// <param name="targetProperty">The target property.</param>
    /// <returns>A reference to the given <see cref="Timeline"/> instance.</returns>
    public static Timeline SetTarget(this Timeline timeline, string targetName, PropertyPath targetProperty)
    {
        Storyboard.SetTargetName(timeline, targetName);
        Storyboard.SetTargetProperty(timeline, targetProperty);
        return timeline;
    }

    /// <summary>
    /// Sets the target object and property.
    /// </summary>
    /// <param name="timeline">The timeline.</param>
    /// <param name="target">The target object.</param>
    /// <param name="targetProperty">The target property.</param>
    /// <returns>A reference to the given <see cref="Timeline"/> instance.</returns>
    public static Timeline SetTarget(this Timeline timeline, DependencyObject target, PropertyPath targetProperty)
    {
        Storyboard.SetTarget(timeline, target);
        Storyboard.SetTargetProperty(timeline, targetProperty);
        return timeline;
    }
}
