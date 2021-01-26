using System.Windows.Media.Animation;

namespace MaSch.Presentation.Wpf.Extensions
{
    public static class StoryboardExtensions
    {
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
