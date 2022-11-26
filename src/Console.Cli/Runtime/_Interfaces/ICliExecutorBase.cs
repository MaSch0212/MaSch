namespace MaSch.Console.Cli.Runtime;

/// <summary>
/// Base interface for the <see cref="ICliExecutor{TCommand}"/> and <see cref="ICliExecutorBase{TCommand}"/> interfaces.
/// </summary>
/// <typeparam name="TCommand">The type of commands the executor can execute.</typeparam>
[SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "Fine here, it is just a base interface. Actual members are in derived interfaces.")]
public interface ICliExecutorBase<TCommand>
{
}
