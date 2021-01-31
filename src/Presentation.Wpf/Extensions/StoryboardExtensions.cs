using System.Windows.Media.Animation;

namespace MaSch.Presentation.Wpf.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Storyboard"/> class.
    /// </summary>
    public static class StoryboardExtensions
    {
        /// <summary>
        /// Determines whether this instance of the <see cref="Storyboard"/> class is running.
        /// </summary>
        /// <param name="sb">The storyboard.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="Storyboard"/> is running; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsRunning(this Storyboard sb)
        {
            var state = ClockState.Stopped;
            try
            {
                state = sb.GetCurrentState();
            }
            catch
            {
                // ignored
            }

            return state == ClockState.Active;
        }
    }
}
