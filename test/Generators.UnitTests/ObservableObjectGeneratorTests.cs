using MaSch.Core.Observable;
using MaSch.Generators.ObservableObject;
using System.ComponentModel;

namespace MaSch.Generators.UnitTests;

[TestClass]
public class ObservableObjectGeneratorTests : GeneratorTestBase<Generator>
{
    protected override string[] FixedSourceFiles { get; } = new[] { "GenerateNotifyPropertyChangedAttribute.g.cs", "GenerateObservableObjectAttribute.g.cs" };

    [TestMethod]
    public async Task NormalClass()
    {
        await CompileAndVerify(
            """
            namespace Test
            {
                public class MyClass { }
            }
            """,
            new CompilationVerifyOptions
            {
                ExpectedSourceFiles = FixedSourceFiles,
            });
    }

    [TestMethod]
    public async Task GenerateNotifyPropertyChanged()
    {
        await CompileAndVerify(
            """
            namespace Test
            {
                [MaSch.Core.GenerateNotifyPropertyChangedAttribute]
                public partial class MyClass
                {
                    private string _testAutoName;
                    private string _testManualName;
                    public string TestAutoName { get => _testAutoName; set => SetProperty(ref _testAutoName, value); }
                    public string TestManualName { get => _testManualName; set => SetProperty(ref _testManualName, value, "MyProperty"); }
                    public void InvokeOnPropertyChangedAutomatic() => OnPropertyChanged();
                    public void InvokeOnPropertyChangedManual() => OnPropertyChanged("MyProperty");
                }
            }
            """,
            new CompilationVerifyOptions
            {
                ExpectedSourceFiles = GetExpectedSourceFiles("Test.MyClass.g.cs"),
                SkipVerifyForFiles = FixedSourceFiles,
            });
    }

    [TestMethod]
    public async Task GenerateObservableObject()
    {
        await CompileAndVerify(
            """
            namespace Test
            {
                [MaSch.Core.GenerateObservableObjectAttribute]
                public partial class MyClass
                {
                    private string _testAutoName;
                    private string _testManualName;
                    public string TestAutoName { get => _testAutoName; set => SetProperty(ref _testAutoName, value); }
                    public string TestManualName { get => _testManualName; set => SetProperty(ref _testManualName, value, "MyProperty"); }
                    public void InvokeOnPropertyChangedAutomatic() => NotifyPropertyChanged();
                    public void InvokeOnPropertyChangedManual() => NotifyPropertyChanged("MyProperty");
                }
            }
            """,
            new CompilationVerifyOptions
            {
                ExpectedSourceFiles = GetExpectedSourceFiles("Test.MyClass.g.cs"),
                SkipVerifyForFiles = FixedSourceFiles,
            });
    }

    [TestMethod]
    public async Task GenerateObservableObjectAndNotifyPropertyChanged()
    {
        await CompileAndVerify(
            """
            namespace Test
            {
                [MaSch.Core.GenerateNotifyPropertyChangedAttribute]
                [MaSch.Core.GenerateObservableObjectAttribute]
                public partial class MyClass
                {
                    private string _testAutoName;
                    private string _testManualName;
                    public string TestAutoName { get => _testAutoName; set => SetProperty(ref _testAutoName, value); }
                    public string TestManualName { get => _testManualName; set => SetProperty(ref _testManualName, value, "MyProperty"); }
                    public void InvokeOnPropertyChangedAutomatic() => NotifyPropertyChanged();
                    public void InvokeOnPropertyChangedManual() => NotifyPropertyChanged("MyProperty");
                }
            }
            """,
            new CompilationVerifyOptions
            {
                ExpectedSourceFiles = GetExpectedSourceFiles("Test.MyClass.g.cs"),
                SkipVerifyForFiles = FixedSourceFiles,
            });
    }

    protected override ICreateCompilationBuilder OnCreateCompilationBuilder(ICreateCompilationBuilder builder)
    {
        return base.OnCreateCompilationBuilder(builder)
            .WithReference(typeof(IObservableObject).Assembly)
            .WithReference(typeof(INotifyPropertyChanged).Assembly);
    }
}
