using MaSch.Presentation.Views;

namespace MaSch.Presentation;

/// <summary>
/// Providers methods to Run, Initialize and Shutdown an application.
/// </summary>
/// <seealso cref="IResourceContainer" />
public interface IApplication : IResourceContainer
{
    /// <summary>
    /// Initializes the components of the application.
    /// </summary>
    void InitializeComponent();

    /// <summary>
    /// Runs the application.
    /// </summary>
    /// <returns>The exit code.</returns>
    int Run();

    /// <summary>
    /// Runs the application.
    /// </summary>
    /// <param name="window">The starting window.</param>
    /// <returns>The exit code.</returns>
    int Run(IWindow window);

    /// <summary>
    /// Shuts down the application.
    /// </summary>
    void Shutdown();

    /// <summary>
    /// Shuts down the application using a specific exit code.
    /// </summary>
    /// <param name="exitCode">The exit code.</param>
    void Shutdown(int exitCode);
}
