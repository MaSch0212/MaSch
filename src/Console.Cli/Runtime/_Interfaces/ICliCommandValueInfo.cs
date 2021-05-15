using MaSch.Console.Cli.Configuration;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliCommandValueInfo : ICliCommandMemberInfo
    {
        CliCommandValueAttribute Attribute { get; }

        string DisplayName { get; }
        int Order { get; }
    }
}
