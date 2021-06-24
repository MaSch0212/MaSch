namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Options for the <see cref="ICliArgumentParser"/>.
    /// </summary>
    public class CliParserOptions
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// Gets or sets the year in which the version has been released.
        /// </summary>
        public string? Year { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// Gets or sets the name of the command or executable file.
        /// </summary>
        public string? CliName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether unknown options should be ignored while parsing command line arguments.
        /// </summary>
        public bool? IgnoreUnknownOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether additional values should be ignored while parsing command line arguments.
        /// </summary>
        public bool? IgnoreAdditionalValues { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the "help" command should be automatically provided.
        /// </summary>
        public bool? ProvideHelpCommand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the "version" command should be automatically provided.
        /// </summary>
        public bool? ProvideVersionCommand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the "help" option should be automatically provided to all commands.
        /// </summary>
        public bool? ProvideHelpOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the "version" option should be automatically provided to all commands.
        /// </summary>
        public bool? ProvideVersionOptions { get; set; }
    }
}
