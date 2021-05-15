using MaSch.Console.Cli.Configuration;
using System.Collections.Generic;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliCommandOptionInfo : ICliCommandMemberInfo
    {
        CliCommandOptionAttribute Attribute { get; }

        IReadOnlyList<char> ShortAliases { get; }
        IReadOnlyList<string> Aliases { get; }
        int HelpOrder { get; }
    }
}
