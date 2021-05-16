namespace MaSch.Console.Cli.Runtime
{
    public interface ICliCommandExecutor
    {
        int ExecuteCommand();
    }

    public interface ICliCommandExecutor<in TCommand>
    {
        int ExecuteCommand(TCommand parameters);
    }
}
