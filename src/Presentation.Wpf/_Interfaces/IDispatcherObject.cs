using System.Windows.Threading;

namespace MaSch.Presentation.Wpf
{
    /// <summary>
    /// Represents an object that has a <see cref="System.Windows.Threading.Dispatcher"/> attached to it.
    /// </summary>
    public interface IDispatcherObject
    {
        /// <summary>
        /// Gets the dispatcher that is attached to this <see cref="IDispatcherObject"/>.
        /// </summary>
        Dispatcher Dispatcher { get; }
    }
}
