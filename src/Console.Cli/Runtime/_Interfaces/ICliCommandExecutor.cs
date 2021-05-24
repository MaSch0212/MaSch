namespace MaSch.Console.Cli.Runtime
{
    public interface ICliCommandExecutor : ICliCommandExecutorBase
    {
        int ExecuteCommand();
    }

    public interface ICliCommandExecutor<TCommand> : ICliCommandExecutorBase<TCommand>
    {
        int ExecuteCommand(TCommand parameters);
    }
}
