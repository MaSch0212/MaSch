using System.Collections.Generic;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliCommandInfoCollection : ICollection<ICliCommandInfo>
    {
        ICliCommandInfo? DefaultCommand { get; }

        IEnumerable<ICliCommandInfo> GetRootCommands();
        IReadOnlyCliCommandInfoCollection AsReadOnly();
    }

    public interface IReadOnlyCliCommandInfoCollection : IReadOnlyCollection<ICliCommandInfo>
    {
        ICliCommandInfo? DefaultCommand { get; }

        IEnumerable<ICliCommandInfo> GetRootCommands();
    }
}
