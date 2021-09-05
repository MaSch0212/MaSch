using MaSch.Core.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Input;

namespace MaSch.Presentation.Wpf.Commands
{
    /// <summary>
    /// Defines a composite command.
    /// </summary>
    /// <seealso cref="ICommand" />
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Does not make sense in this case.")]
    public interface ICompositeCommand : ICommand
    {
        /// <summary>
        /// Gets the commands to execute.
        /// </summary>
        IReadOnlyCollection<ICommand> Commands { get; }

        /// <summary>
        /// Adds a command to this <see cref="ICompositeCommand"/>.
        /// </summary>
        /// <param name="command">The command to add.</param>
        void AddCommand(ICommand command);
    }

    /// <summary>
    /// Default implementation for the <see cref="ICompositeCommand"/> interface composing <see cref="ICommand"/>s without parameters.
    /// </summary>
    /// <seealso cref="AsyncCommandBase" />
    /// <seealso cref="ICompositeCommand" />
    public class CompositeCommand : CommandBase, ICompositeCommand
    {
        private readonly List<ICommand> _commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCommand"/> class.
        /// </summary>
        public CompositeCommand()
        {
            _commands = new List<ICommand>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCommand"/> class.
        /// </summary>
        /// <param name="commands">The commands to execute.</param>
        public CompositeCommand(params ICommand[] commands)
            : this((IEnumerable<ICommand>)commands)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCommand"/> class.
        /// </summary>
        /// <param name="commands">The commands to execute.</param>
        public CompositeCommand(IEnumerable<ICommand> commands)
        {
            _commands = commands.ToList();
        }

        /// <summary>
        /// Gets the commands to execute.
        /// </summary>
        public IReadOnlyCollection<ICommand> Commands => _commands.AsReadOnly();

        /// <summary>
        /// Adds a command to this <see cref="ICompositeCommand" />.
        /// </summary>
        /// <param name="command">The command to add.</param>
        public void AddCommand(ICommand command)
        {
            _ = _commands.AddIfNotExists(command);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public override void Execute()
        {
            _commands.ForEach(x => x.Execute(null));
        }
    }

    /// <summary>
    /// Default implementation for the <see cref="ICompositeCommand"/> interface composing <see cref="ICommand"/>s with parameters of a specific type.
    /// </summary>
    /// <typeparam name="T">The parameter type for this <see cref="ICommand"/>.</typeparam>
    /// <seealso cref="CommandBase" />
    /// <seealso cref="ICompositeCommand" />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file.")]
    public class CompositeCommand<T> : CommandBase<T>, ICompositeCommand
    {
        private readonly List<ICommand> _commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCommand{T}"/> class.
        /// </summary>
        public CompositeCommand()
        {
            _commands = new List<ICommand>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCommand{T}"/> class.
        /// </summary>
        /// <param name="commands">The commands to execute.</param>
        public CompositeCommand(params ICommand[] commands)
            : this((IEnumerable<ICommand>)commands)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCommand{T}"/> class.
        /// </summary>
        /// <param name="commands">The commands to execute.</param>
        public CompositeCommand(IEnumerable<ICommand> commands)
        {
            _commands = commands.ToList();
        }

        /// <summary>
        /// Gets the commands to execute.
        /// </summary>
        public IReadOnlyCollection<ICommand> Commands => _commands.AsReadOnly();

        /// <summary>
        /// Adds a command to this <see cref="ICompositeCommand" />.
        /// </summary>
        /// <param name="command">The command to add.</param>
        public void AddCommand(ICommand command)
        {
            _ = _commands.AddIfNotExists(command);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        public override void Execute(T? parameter)
        {
            _commands.ForEach(x => x.Execute(parameter));
        }
    }
}
