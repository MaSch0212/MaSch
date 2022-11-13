using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.Test.UnitTests.Extensions;
using VerifyMSTest;
using VerifyTests;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration;

public class SourceBuilderTestBase<T> : VerifyBase
    where T : ISourceBuilder
{
    private static readonly Regex NewVerifyErrorMessageRegex = new("New:\\s*Received:", RegexOptions.Compiled);

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
        await Verify(Builder, null);
    }

    protected async Task VerifyBuilder(params object?[] parameters)
    {
        var settings = new VerifySettings();
        settings.UseParameters(parameters);
        await Verify(Builder, settings);
    }

    protected async Task Verify(ISourceBuilder builder, VerifySettings? settings)
    {
        try
        {
            await Verify(builder.ToString(), settings);
        }
        catch (Exception ex) when (ex.GetType().Name == "VerifyException" && NewVerifyErrorMessageRegex.IsMatch(ex.Message))
        {
            throw new AssertInconclusiveException("A verify failed, because no verified file existed.", ex);
        }
    }
}
