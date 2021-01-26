using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaSch.Core.Extensions;

namespace MaSch.Presentation.Wpf.Commands
{
    public interface IAsyncCompositeCommand : IAsyncCommand
    {
        IReadOnlyCollection<IAsyncCommand> Commands { get; }

        void AddCommand(IAsyncCommand command);
    }

    public class AsyncCompositeCommand : AsyncCommandBase, IAsyncCompositeCommand
    {
        private readonly List<IAsyncCommand> _commands;

        public IReadOnlyCollection<IAsyncCommand> Commands => _commands.AsReadOnly();

        public AsyncCompositeCommand()
        {
            _commands = new List<IAsyncCommand>();
        }
        public AsyncCompositeCommand(params IAsyncCommand[] commands) : this((IEnumerable<IAsyncCommand>)commands) { }
        public AsyncCompositeCommand(IEnumerable<IAsyncCommand> commands)
        {
            _commands = commands.ToList();
        }

        public void AddCommand(IAsyncCommand command)
        {
            _commands.AddIfNotExists(command);
        }

        public override async Task Execute()
        {
            foreach (var c in _commands)
                await c.ExecuteAsync(null);
        }
    }

    public class AsyncCompositeCommand<T> : AsyncCommandBase<T>, IAsyncCompositeCommand
    {
        private readonly List<IAsyncCommand> _commands;

        public IReadOnlyCollection<IAsyncCommand> Commands => _commands.AsReadOnly();

        public AsyncCompositeCommand()
        {
            _commands = new List<IAsyncCommand>();
        }
        public AsyncCompositeCommand(params IAsyncCommand[] commands) : this((IEnumerable<IAsyncCommand>)commands) { }
        public AsyncCompositeCommand(IEnumerable<IAsyncCommand> commands)
        {
            _commands = commands.ToList();
        }

        public void AddCommand(IAsyncCommand command)
        {
            _commands.AddIfNotExists(command);
        }

        public override async Task Execute(T parameter)
        {
            foreach (var c in _commands)
                await c.ExecuteAsync(parameter);
        }
    }
}
