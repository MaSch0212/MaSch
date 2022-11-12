using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.Test.UnitTests.Extensions;
using VerifyMSTest;
using VerifyTests;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration;

[TestClass]
public class SourceBuilderTestBase<T> : VerifyBase
    where T : ISourceBuilder
{
    protected T Builder { get; private set; } = default!;

    [TestInitialize]
    public void InitializeTest()
    {
        OnTestInitialize();
    }

    [TestCleanup]
    public void CleanupTest()
    {
        OnTestCleanup();
    }

    protected virtual void OnTestInitialize()
    {
        var optionsAttribute = TestContext.GetTestMethod().GetCustomAttribute<SourceBuilderOptionsAttribute>() ?? SourceBuilderOptionsAttribute.Default;
        var options = optionsAttribute.CreateOptions();

        Builder = SourceBuilder.Create(options).As<T>();
    }

    protected virtual void OnTestCleanup()
    {
    }

    protected virtual SourceBuilderOptions GetBuilderOptions()
    {
        return SourceBuilderOptions.Default;
    }

    protected async Task VerifyBuilder()
    {
        await Verify(Builder.ToString());
    }

    protected async Task VerifyBuilder(params object?[] parameters)
    {
        var settings = new VerifySettings();
        settings.UseParameters(parameters);
        await Verify(Builder.ToString(), settings);
    }
}
