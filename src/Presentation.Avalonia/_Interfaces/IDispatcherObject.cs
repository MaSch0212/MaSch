using Avalonia.Threading;

namespace MaSch.Presentation.Avalonia
{
    /// <summary>
    /// Represents an object that has a <see cref="Dispatcher"/> attached to it.
    /// </summary>
    public interface IDispatcherObject
    {
        /// <summary>
        /// Gets the dispatcher that is attached to this <see cref="IDispatcherObject"/>.
        /// </summary>
        Dispatcher Dispatcher { get; }
    }
}
