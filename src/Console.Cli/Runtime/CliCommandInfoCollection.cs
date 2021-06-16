using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MaSch.Console.Cli.Runtime
{
    /// <inheritdoc/>
    public class CliCommandInfoCollection : ICliCommandInfoCollection
    {
        private readonly Dictionary<Type, ICliCommandInfo> _allCommands = new();
        private readonly List<ICliCommandInfo> _rootCommands = new();

        /// <inheritdoc/>
        public ICliCommandInfo? DefaultCommand { get; private set; }

        /// <inheritdoc/>
        public int Count => _allCommands.Count;

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public bool IsReadOnly => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandInfoCollection"/> class.
        /// </summary>
        public CliCommandInfoCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandInfoCollection"/> class.
        /// </summary>
        /// <param name="collection">The elements to initialize the collection with.</param>
        public CliCommandInfoCollection(IEnumerable<ICliCommandInfo> collection)
        {
            if (collection != null)
                this.Add(collection);
        }

        /// <inheritdoc/>
        public void Add(ICliCommandInfo item)
        {
            Guard.NotNull(item, nameof(item));

            if (item.CommandType == null || item.Attribute == null)
                throw new ArgumentException("The given command has to have a CommandType.");

            if (item.Attribute.ParentCommand != null)
            {
                if (!_allCommands.TryGetValue(item.Attribute.ParentCommand, out var parent))
                    throw new ArgumentException($"The command type \"{item.Attribute.ParentCommand.FullName}\" has not been registered yet. Please register all parent commands before the child commands.");
                ValidateCommand(item, parent);
                parent.AddChildCommand(item);
            }
            else
            {
                ValidateCommand(item, null);
                _rootCommands.Add(item);
            }

            _allCommands.Add(item.CommandType, item);
            if (item.IsDefault && item.ParentCommand == null)
                DefaultCommand = item;
        }

        /// <inheritdoc/>
        public bool Remove(ICliCommandInfo item)
        {
            Guard.NotNull(item, nameof(item));

            if (item.CommandType == null || !_allCommands.TryGetValue(item.CommandType, out var ei) || ei != item)
                return false;

            foreach (var child in item.ChildCommands.ToArray())
            {
                Remove(child);
            }

            var parent = item.ParentCommand ?? (item.Attribute.ParentCommand != null && _allCommands.TryGetValue(item.Attribute.ParentCommand, out var p) ? p : null);
            if (parent != null)
                parent.RemoveChildCommand(item);

            if (_rootCommands.Contains(item))
                _rootCommands.Remove(item);
            _allCommands.Remove(item.CommandType);
            return true;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            foreach (var item in _allCommands.Values.ToArray())
                Remove(item);
        }

        /// <inheritdoc/>
        public bool Contains(ICliCommandInfo item)
            => item?.CommandType != null && _allCommands.TryGetValue(item.CommandType, out var ei) && ei == item;

        /// <inheritdoc/>
        public void CopyTo(ICliCommandInfo[] array, int arrayIndex) => _allCommands.Values.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        public IEnumerator<ICliCommandInfo> GetEnumerator() => _allCommands.Values.GetEnumerator();

        /// <inheritdoc/>
        public IEnumerable<ICliCommandInfo> GetRootCommands()
            => _rootCommands.AsEnumerable();

        /// <inheritdoc/>
        public IReadOnlyCliCommandInfoCollection AsReadOnly() => new ReadOnly(this);

        private void ValidateCommand(ICliCommandInfo command, ICliCommandInfo? parentCommand)
        {
            if (this.TryFirst(x => x.CommandType == command.CommandType, out var existing))
                throw new ArgumentException($"A command for command type \"{command.CommandType.FullName}\" is already registered.");

            var collection = parentCommand == null ? _rootCommands : parentCommand.ChildCommands;
            foreach (var c in collection)
            {
                var existingNames = command.Aliases.Intersect(c.Aliases, StringComparer.OrdinalIgnoreCase).ToArray();
                if (existingNames.Length > 0)
                    throw new ArgumentException($"The name(s) \"{string.Join("\", \"", existingNames)}\" cannot be used for command \"{command.CommandType.FullName}\" because the same name(s) are already used by command \"{c.CommandType.FullName}\".");
            }

            if (command.Attribute.ParentCommand == null && command.IsDefault && DefaultCommand != null)
                throw new ArgumentException($"The command {command.Name} cannot be added because another default command is already added: {DefaultCommand.CommandType.FullName}.");
        }

        /// <inheritdoc/>
        public class ReadOnly : IReadOnlyCliCommandInfoCollection
        {
            private readonly ICliCommandInfoCollection _collection;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReadOnly"/> class.
            /// </summary>
            /// <param name="collection">The collection to wrap.</param>
            public ReadOnly(ICliCommandInfoCollection collection)
            {
                _collection = collection;
            }

            /// <inheritdoc/>
            public ICliCommandInfo? DefaultCommand => _collection.DefaultCommand;

            /// <inheritdoc/>
            public int Count => _collection.Count;

            /// <inheritdoc/>
            public IEnumerator<ICliCommandInfo> GetEnumerator() => _collection.GetEnumerator();

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();

            /// <inheritdoc/>
            public IEnumerable<ICliCommandInfo> GetRootCommands() => _collection.GetRootCommands();
        }
    }
}
