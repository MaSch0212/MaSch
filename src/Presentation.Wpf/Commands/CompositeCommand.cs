using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MaSch.Common.Extensions;

namespace MaSch.Presentation.Wpf.Commands
{
    public interface ICompositeCommand : ICommand
    {
        IReadOnlyCollection<ICommand> Commands { get; }

        void AddCommand(ICommand command);
    }

    public class CompositeCommand : CommandBase, ICompositeCommand
    {
        private readonly List<ICommand> _commands;

        public IReadOnlyCollection<ICommand> Commands => _commands.AsReadOnly();

        public CompositeCommand()
        {
            _commands = new List<ICommand>();
        }
        public CompositeCommand(params ICommand[] commands) : this((IEnumerable<ICommand>)commands) { }
        public CompositeCommand(IEnumerable<ICommand> commands)
        {
            _commands = commands.ToList();
        }

        public void AddCommand(ICommand command)
        {
            _commands.AddIfNotExists(command);
        }

        public override void Execute()
        {
            _commands.ForEach(x => x.Execute(null));
        }
    }

    public class CompositeCommand<T> : CommandBase<T>, ICompositeCommand
    {
        private readonly List<ICommand> _commands;

        public IReadOnlyCollection<ICommand> Commands => _commands.AsReadOnly();

        public CompositeCommand()
        {
            _commands = new List<ICommand>();
        }
        public CompositeCommand(params ICommand[] commands) : this((IEnumerable<ICommand>)commands) { }
        public CompositeCommand(IEnumerable<ICommand> commands)
        {
            _commands = commands.ToList();
        }

        public void AddCommand(ICommand command)
        {
            _commands.AddIfNotExists(command);
        }

        public override void Execute(T parameter)
        {
            _commands.ForEach(x => x.Execute(parameter));
        }
    }
}
