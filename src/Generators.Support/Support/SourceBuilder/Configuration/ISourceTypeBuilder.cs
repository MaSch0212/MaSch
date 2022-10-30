namespace MaSch.Generators.Support
{
    public interface ISourceTypeBuilder<T>
        where T : ISourceTypeBuilder<T>
    {
        T DrivesFrom(string typeName);
    }
}