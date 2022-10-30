namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IMethodConfiguration<T> : IMemberConfiguration<T>
    where T : IMemberConfiguration<T>
{
}

public interface IMethodConfiguration : IMethodConfiguration<IMethodConfiguration>
{
}

public class MethodConfiguration<T> : MemberConfiguration<T>, IMethodConfiguration<T>
    where T : IMethodConfiguration<T>
{

}

public sealed class MethodConfiguration : MethodConfiguration<IMethodConfiguration>, IMethodConfiguration
{

}