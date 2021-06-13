namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents a parser that parses command line arguments.
    /// </summary>
    public interface ICliArgumentParser
    {
        /// <summary>
        /// Adds a validator for the created options object.
        /// </summary>
        /// <param name="validator">The validator to add.</param>
        void AddValidator(ICliValidator<object> validator);

        /// <summary>
        /// Parses the given command line arguments using the options and comands of the given application.
        /// </summary>
        /// <param name="application">The application for which to parse the command line arguments.</param>
        /// <param name="args">The command line arguments to parse.</param>
        /// <returns>A <see cref="CliArgumentParserResult"/> object containing the result of the parse.</returns>
        CliArgumentParserResult Parse(ICliApplicationBase application, string[] args);
    }
}
