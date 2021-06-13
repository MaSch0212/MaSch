using System;
using System.Collections.Generic;

namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents the result of an <see cref="ICliArgumentParser"/>.
    /// </summary>
    public class CliArgumentParserResult
    {
        /// <summary>
        /// Gets a value indicating whether the parse completed successfully.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets the errors that where detected while parsing.
        /// </summary>
        public IEnumerable<CliError> Errors { get; }

        /// <summary>
        /// Gets the command that has been detected by the parse.
        /// </summary>
        public ICliCommandInfo? Command { get; }

        /// <summary>
        /// Gets the command instance that has been created during the parse.
        /// </summary>
        public object? Options { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliArgumentParserResult"/> class.
        /// </summary>
        /// <param name="errors">The errors that where detected while parsing.</param>
        /// <param name="command">The command that has been detected by the parse.</param>
        /// <param name="options">The command instance that has been created during the parse.</param>
        internal protected CliArgumentParserResult(IEnumerable<CliError> errors, ICliCommandInfo? command, object? options)
        {
            Success = false;
            Errors = errors;
            Command = command;
            Options = options;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliArgumentParserResult"/> class.
        /// </summary>
        /// <param name="errors">The errors that where detected while parsing.</param>
        internal protected CliArgumentParserResult(IEnumerable<CliError> errors)
            : this(errors, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliArgumentParserResult"/> class.
        /// </summary>
        /// <param name="command">The command that has been detected by the parse.</param>
        /// <param name="options">The command instance that has been created during the parse.</param>
        internal protected CliArgumentParserResult(ICliCommandInfo command, object options)
        {
            Success = true;
            Errors = Array.Empty<CliError>();
            Command = command;
            Options = options;
        }
    }
}
