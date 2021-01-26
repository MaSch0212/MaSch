using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MaSch.Console.Cli
{
    public class CliCommandInfoCollection : ICollection<CliCommandInfo>
    {
        private readonly IDictionary<Type, CliCommandInfo> _allCommands = new Dictionary<Type, CliCommandInfo>();
        private readonly List<CliCommandInfo> _rootCommands = new List<CliCommandInfo>();

        public CliCommandInfo? DefaultCommand;
        public int Count => _allCommands.Count;
        public bool IsReadOnly => false;

        public CliCommandInfoCollection() { }
        public CliCommandInfoCollection(IEnumerable<CliCommandInfo> collection)
        {
            if (collection != null)
                this.Add(collection);
        }

        public void Add(CliCommandInfo item)
        {
            if (item.CommandType == null || item.Attribute == null)
                throw new ArgumentException("The given command has to have a CommandType.");

            ValidateCommand(item);
            if (item.Attribute.ParentCommand != null)
            {
                if (!_allCommands.TryGetValue(item.Attribute.ParentCommand, out var parent))
                    throw new ArgumentException($"The command type \"{item.Attribute.ParentCommand.FullName}\" has not been registered yet. Please register all parent commands before the child commands.");
                parent.AddChildCommand(item);
            }
            else
                _rootCommands.Add(item);
            _allCommands.Add(item.CommandType, item);
            if (item.IsDefault)
                DefaultCommand = item;
        }

        public bool Remove(CliCommandInfo item)
        {
            if (item.CommandType == null || !_allCommands.TryGetValue(item.CommandType, out var ei) || ei != item)
                return false;

            foreach (var child in item.ChildCommands.ToArray())
            {
                item.RemoveChildCommand(child);
                Remove(child);
            }
            if (_rootCommands.Contains(item))
                _rootCommands.Remove(item);
            _allCommands.Remove(item.CommandType);
            return true;
        }

        public void Clear()
        {
            foreach (var item in _allCommands.Values.ToArray())
                Remove(item);
        }

        public bool Contains(CliCommandInfo item)
            => item.CommandType != null && _allCommands.TryGetValue(item.CommandType, out var ei) && ei == item;

        public void CopyTo(CliCommandInfo[] array, int arrayIndex) => _allCommands.Values.CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<CliCommandInfo> GetEnumerator() => _allCommands.Values.GetEnumerator();

        public IEnumerable<CliCommandInfo> GetRootCommands()
            => _rootCommands.AsEnumerable();

        public IReadOnlyCollection<CliCommandInfo> AsReadOnly() => new ReadOnly(this);

        private void ValidateCommand(CliCommandInfo command)
        {
            if (this.TryFirst(x => x.CommandType == command.CommandType, out var existing))
                throw new ArgumentException($"A command for command type \"{command.CommandType.FullName}\" is already registered.");
            foreach (var c in this)
            {
                var existingNames = command.Aliases.Intersect(c.Aliases, StringComparer.OrdinalIgnoreCase).ToArray();
                if (existingNames.Length > 0)
                    throw new ArgumentException($"The name(s) \"{string.Join("\", \"", existingNames)}\" cannot be used for command \"{command.CommandType.FullName}\" because the same name(s) are already used by command \"{c.CommandType.FullName}\".");
            }
            if (command.Attribute.ParentCommand == null && command.IsDefault && DefaultCommand != null)
                throw new ArgumentException($"The command {command.Name} cannot be added because another default command is already added: {DefaultCommand.CommandType.FullName}.");
        }

        private class ReadOnly : IReadOnlyCollection<CliCommandInfo>
        {
            private readonly ICollection<CliCommandInfo> _collection;

            public ReadOnly(ICollection<CliCommandInfo> collection)
            {
                _collection = collection;
            }

            public int Count => _collection.Count;
            public IEnumerator<CliCommandInfo> GetEnumerator() => _collection.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();
        }
    }
}
