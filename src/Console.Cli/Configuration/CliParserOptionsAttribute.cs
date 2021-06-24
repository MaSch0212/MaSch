using System;

namespace MaSch.Console.Cli.Configuration
{
    /// <summary>
    /// Overwrites the specified parser options from the application options.
    /// This only has effect on classes or interfaces with the <see cref="CliCommandAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class CliParserOptionsAttribute : Attribute
    {
        internal bool? IgnoreUnknownOptionsValue { get; private set; }
        internal bool? IgnoreAdditionalValuesValue { get; private set; }
        internal bool? ProvideHelpCommandValue { get; private set; }
        internal bool? ProvideVersionCommandValue { get; private set; }
        internal bool? ProvideHelpOptionsValue { get; private set; }
        internal bool? ProvideVersionOptionsValue { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether unknown options should be ignored while parsing command line arguments.
        /// </summary>
        public bool IgnoreUnknownOptions
        {
            get => IgnoreUnknownOptionsValue ?? false;
            set => IgnoreUnknownOptionsValue = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether additional values should be ignored while parsing command line arguments.
        /// </summary>
        public bool IgnoreAdditionalValues
        {
            get => IgnoreAdditionalValuesValue ?? false;
            set => IgnoreAdditionalValuesValue = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the "help" command should be automatically provided.
        /// </summary>
        public bool ProvideHelpCommand
        {
            get => ProvideHelpCommandValue ?? true;
            set => ProvideHelpCommandValue = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the "version" command should be automatically provided.
        /// </summary>
        public bool ProvideVersionCommand
        {
            get => ProvideVersionCommandValue ?? true;
            set => ProvideVersionCommandValue = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the "help" option should be automatically provided to all commands.
        /// </summary>
        public bool ProvideHelpOptions
        {
            get => ProvideHelpOptionsValue ?? true;
            set => ProvideHelpOptionsValue = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the "version" option should be automatically provided to all commands.
        /// </summary>
        public bool ProvideVersionOptions
        {
            get => ProvideVersionOptionsValue ?? true;
            set => ProvideVersionOptionsValue = value;
        }
    }
}
