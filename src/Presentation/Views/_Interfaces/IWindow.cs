using System;

namespace MaSch.Presentation.Views
{
    /// <summary>
    /// Provides members that represent a Window.
    /// </summary>
    /// <seealso cref="IResourceContainer" />
    public interface IWindow : IResourceContainer
    {
        /// <summary>
        /// Gets or sets a value that indicates whether a window is restored, minimized, or maximized.
        /// </summary>
        WindowVisualState WindowState { get; set; }

        /// <summary>
        /// Gets or sets the width of the window.
        /// </summary>
        double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the window.
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// Gets or sets the distance of the window in pixels from the top of the screen.
        /// </summary>
        double Top { get; set; }

        /// <summary>
        /// Gets or sets the distance of the window in pixels from the left of the screen.
        /// </summary>
        double Left { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a window appears in the topmost z-order.
        /// </summary>
        bool Topmost { get; set; }

        /// <summary>
        /// Manually closes a <see cref="IWindow"/>.
        /// </summary>
        void Close();

        /// <summary>
        /// Makes the window invisible.
        /// </summary>
        void Hide();

        /// <summary>
        /// Attempts to bring the window to the foreground and activates it.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="IWindow"/> was successfully activated; otherwise, <c>false</c>.</returns>
        bool Activate();

        /// <summary>
        /// Opens a window and returns without waiting for the newly opened window to close.
        /// </summary>
        void Show();

        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed.
        /// </summary>
        /// <returns>
        /// A <see cref="Nullable{T}"/> value of type <see cref="bool"/> that specifies whether the activity was accepted (<c>true</c>) or canceled (<c>false</c>).
        /// </returns>
        bool? ShowDialog();
    }
}
