namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Options for the <see cref="ICliArgumentParser"/>.
    /// </summary>
    public interface ICliParserOptions
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public string? Version { get; }

        /// <summary>
        /// Gets the year in which the version has been released.
        /// </summary>
        public string? Year { get; }

        /// <summary>
        /// Gets the author.
        /// </summary>
        public string? Author { get; }

        /// <summary>
        /// Gets the name of the command or executable file.
        /// </summary>
        public string? CliName { get; }

        /// <summary>
        /// Gets a value indicating whether unknown options should be ignored while parsing command line arguments.
        /// </summary>
        public bool? IgnoreUnknownOptions { get; }

        /// <summary>
        /// Gets a value indicating whether additional values should be ignored while parsing command line arguments.
        /// </summary>
        public bool? IgnoreAdditionalValues { get; }

        /// <summary>
        /// Gets a value indicating whether the "help" command should be automatically provided.
        /// </summary>
        public bool? ProvideHelpCommand { get; }

        /// <summary>
        /// Gets a value indicating whether the "version" command should be automatically provided.
        /// </summary>
        public bool? ProvideVersionCommand { get; }

        /// <summary>
        /// Gets a value indicating whether the "help" option should be automatically provided to all commands.
        /// </summary>
        public bool? ProvideHelpOptions { get; }

        /// <summary>
        /// Gets a value indicating whether the "version" option should be automatically provided to all commands.
        /// </summary>
        public bool? ProvideVersionOptions { get; }
    }
}
