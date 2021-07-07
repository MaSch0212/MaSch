using MaSch.Console.Cli.Runtime;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MaSch.Console.Cli
{
    /// <summary>
    /// Represents base functionality for the <see cref="ICliApplication"/> and <see cref="ICliAsyncApplication"/> interfaces.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Base interface")]
    public interface ICliApplicationBase
    {
        /// <summary>
        /// Gets the service provider.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the list of all commands specified for this application.
        /// </summary>
        IReadOnlyCliCommandInfoCollection Commands { get; }

        /// <summary>
        /// Gets the options for this application.
        /// </summary>
        ICliApplicationOptions Options { get; }
    }

    /// <summary>
    /// Represents an application that is using a command line interface.
    /// </summary>
    public interface ICliApplication : ICliApplicationBase
    {
        /// <summary>
        /// Runs the current application.
        /// </summary>
        /// <param name="args">The command line arguments to parse.</param>
        /// <returns>The exit code of this application.</returns>
        int Run(string[] args);
    }

    /// <summary>
    /// Represents an asynchronous application that is using a command line interface.
    /// </summary>
    public interface ICliAsyncApplication : ICliApplicationBase
    {
        /// <summary>
        /// Runs the current application asynchronously.
        /// </summary>
        /// <param name="args">The command line arguments to parse.</param>
        /// <returns>The exit code of this application.</returns>
        Task<int> RunAsync(string[] args);
    }
}
