using System.Collections.Generic;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliCommandInfoCollection : ICollection<ICliCommandInfo>
    {
        ICliCommandInfo? DefaultCommand { get; }

        IReadOnlyCollection<ICliCommandInfo> AsReadOnly();
    }
}
