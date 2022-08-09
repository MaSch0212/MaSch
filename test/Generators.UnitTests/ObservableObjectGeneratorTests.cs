using MaSch.Core.Observable;
using Microsoft.CodeAnalysis;
using System.ComponentModel;

namespace MaSch.Generators.UnitTests;

[TestClass]
public class ObservableObjectGeneratorTests : GeneratorTestBase<ObservableObjectGenerator>
{
    [TestMethod]
    public void GenerateNothing()
    {
        var source = @"namespace Test { public class MyClass { } }";

        var result = CreateCompilationBuilderWithBuilder()
            .WithSource(source)
            .Build();

        result.ValidateDiagnostics().HasNoWarningsOrErrors();

        Assert.AreCollectionsEqual(Enumerable.Empty<GeneratedSourceResult>(), result.GeneratedSourceResults);
    }

    [TestMethod]
    public void GenerateNotifyPropertyChanged()
    {
        var source = @"namespace Test
{
    [MaSch.Core.GenerateNotifyPropertyChangedAttribute]
    public partial class MyClass
    {
        private string _testAutoName;
        private string _testManualName;
        public string TestAutoName { get => _testAutoName; set => SetProperty(ref _testAutoName, value); }
        public string TestManualName { get => _testManualName; set => SetProperty(ref _testManualName, value, ""MyProperty""); }
        public void InvokeOnPropertyChangedAutomatic() => OnPropertyChanged();
        public void InvokeOnPropertyChangedManual() => OnPropertyChanged(""MyProperty"");
    }
}";

        var result = CreateCompilationBuilderWithBuilder()
            .WithSource(source)
            .Build();

        result.ValidateFinalCompilation()
            .HasNoWarningsOrErrors()
            .HasType("Test.MyClass", out var classTypeSymbol)
            .ValidateType(classTypeSymbol, classType => classType
                .ImplementsInterface(typeof(INotifyPropertyChanged))
                .HasMembers(MethodKind.Ordinary, 4)
                .HasMembers(SymbolKind.Property, 2)
                .HasMembers(SymbolKind.Field, 2)
                .HasMembers(SymbolKind.Event, 1)
                .HasEvent("PropertyChanged", propertyChanged => propertyChanged
                    .HasAccessibility(Accessibility.Public)
                    .CanAddAndRemoveHandler())
                .HasMethod(out var setPropertyMethodSymbol, "SetProperty")
                .ValidateMethod(setPropertyMethodSymbol, setPropertyMethod => setPropertyMethod
                    .HasAccessibility(Accessibility.Public))
                .HasMethod(out var onPropertyChangedMethodSymbol, "OnPropertyChanged")
                .ValidateMethod(onPropertyChangedMethodSymbol, onPropertyChangedMethod => onPropertyChangedMethod
                    .HasAccessibility(Accessibility.Protected)));

        var classInstance = new PrivateObject(result.GetFinalAssembly().GetType("Test.MyClass"));
        var eventMock = new Mock<PropertyChangedEventHandler>(MockBehavior.Loose);
        classInstance.AddEventHandler("PropertyChanged", eventMock.Object);

        classInstance.SetProperty("TestAutoName", "TestAutoNameValue");
        Assert.AreEqual("TestAutoNameValue", classInstance.GetProperty("TestAutoName"));
        eventMock.Verify(x => x(It.IsAny<object>(), It.IsAny<PropertyChangedEventArgs>()), Times.Once());
        eventMock.Verify(x => x(classInstance.Target, It.Is<PropertyChangedEventArgs>(e => e.PropertyName == "TestAutoName")), Times.Once());

        eventMock.Invocations.Clear();
        classInstance.SetProperty("TestManualName", "TestManualNameValue");
        Assert.AreEqual("TestManualNameValue", classInstance.GetProperty("TestManualName"));
        eventMock.Verify(x => x(It.IsAny<object>(), It.IsAny<PropertyChangedEventArgs>()), Times.Once());
        eventMock.Verify(x => x(classInstance.Target, It.Is<PropertyChangedEventArgs>(e => e.PropertyName == "MyProperty")), Times.Once());

        eventMock.Invocations.Clear();
        classInstance.Invoke("InvokeOnPropertyChangedAutomatic");
        eventMock.Verify(x => x(It.IsAny<object>(), It.IsAny<PropertyChangedEventArgs>()), Times.Once());
        eventMock.Verify(x => x(classInstance.Target, It.Is<PropertyChangedEventArgs>(e => e.PropertyName == "InvokeOnPropertyChangedAutomatic")), Times.Once());

        eventMock.Invocations.Clear();
        classInstance.Invoke("InvokeOnPropertyChangedManual");
        eventMock.Verify(x => x(It.IsAny<object>(), It.IsAny<PropertyChangedEventArgs>()), Times.Once());
        eventMock.Verify(x => x(classInstance.Target, It.Is<PropertyChangedEventArgs>(e => e.PropertyName == "MyProperty")), Times.Once());
    }

    [TestMethod]
    public void GenerateObservableObject()
    {
        var source = @"namespace Test
{
    [MaSch.Core.GenerateObservableObjectAttribute]
    public partial class MyClass
    {
        private string _testAutoName;
        private string _testManualName;
        public string TestAutoName { get => _testAutoName; set => SetProperty(ref _testAutoName, value); }
        public string TestManualName { get => _testManualName; set => SetProperty(ref _testManualName, value, ""MyProperty""); }
        public void InvokeOnPropertyChangedAutomatic() => NotifyPropertyChanged();
        public void InvokeOnPropertyChangedManual() => NotifyPropertyChanged(""MyProperty"");
    }
}";

        var result = CreateCompilationBuilderWithBuilder()
            .WithSource(source)
            .Build();

        result.ValidateFinalCompilation()
            .HasNoWarningsOrErrors()
            .HasType("Test.MyClass", out var classTypeSymbol)
            .ValidateType(classTypeSymbol, classType => classType
                .ImplementsInterface(typeof(IObservableObject))
                .HasMembers(MethodKind.Ordinary, 5)
                .HasMembers(SymbolKind.Property, 5)
                .HasMembers(SymbolKind.Field, 5)
                .HasMembers(SymbolKind.Event, 1)
                .HasEvent("PropertyChanged", propertyChanged => propertyChanged
                    .HasAccessibility(Accessibility.Public)
                    .CanAddAndRemoveHandler())
                .HasProperty("IsNotifyEnabled", isNotifyEnabled => isNotifyEnabled
                    .HasAccessibility(Accessibility.Public))
                .HasMethod(out var setPropertyMethodSymbol, "SetProperty")
                .ValidateMethod(setPropertyMethodSymbol, setPropertyMethod => setPropertyMethod
                    .HasAccessibility(Accessibility.Public))
                .HasMethod(out var notifyPropertyChangedSymbol, "NotifyPropertyChanged")
                .ValidateMethod(notifyPropertyChangedSymbol, notifyPropertyChanged => notifyPropertyChanged
                    .HasAccessibility(Accessibility.Public))
                .HasMethod(out var notifyCommandChangedSymbol, "NotifyCommandChanged")
                .ValidateMethod(notifyCommandChangedSymbol, notifyCommandChanged => notifyCommandChanged
                    .HasAccessibility(Accessibility.Public)));

        var classInstance = new PrivateObject(result.GetFinalAssembly().GetType("Test.MyClass"));
        var eventMock = new Mock<PropertyChangedEventHandler>(MockBehavior.Loose);
        classInstance.AddEventHandler("PropertyChanged", eventMock.Object);

        classInstance.SetProperty("TestAutoName", "TestAutoNameValue");
        Assert.AreEqual("TestAutoNameValue", classInstance.GetProperty("TestAutoName"));
        eventMock.Verify(x => x(It.IsAny<object>(), It.IsAny<PropertyChangedEventArgs>()), Times.Once());
        eventMock.Verify(x => x(classInstance.Target, It.Is<PropertyChangedEventArgs>(e => e.PropertyName == "TestAutoName")), Times.Once());

        eventMock.Invocations.Clear();
        classInstance.SetProperty("TestManualName", "TestManualNameValue");
        Assert.AreEqual("TestManualNameValue", classInstance.GetProperty("TestManualName"));
        eventMock.Verify(x => x(It.IsAny<object>(), It.IsAny<PropertyChangedEventArgs>()), Times.Once());
        eventMock.Verify(x => x(classInstance.Target, It.Is<PropertyChangedEventArgs>(e => e.PropertyName == "MyProperty")), Times.Once());

        eventMock.Invocations.Clear();
        classInstance.Invoke("InvokeOnPropertyChangedAutomatic");
        eventMock.Verify(x => x(It.IsAny<object>(), It.IsAny<PropertyChangedEventArgs>()), Times.Once());
        eventMock.Verify(x => x(classInstance.Target, It.Is<PropertyChangedEventArgs>(e => e.PropertyName == "InvokeOnPropertyChangedAutomatic")), Times.Once());

        eventMock.Invocations.Clear();
        classInstance.Invoke("InvokeOnPropertyChangedManual");
        eventMock.Verify(x => x(It.IsAny<object>(), It.IsAny<PropertyChangedEventArgs>()), Times.Once());
        eventMock.Verify(x => x(classInstance.Target, It.Is<PropertyChangedEventArgs>(e => e.PropertyName == "MyProperty")), Times.Once());
    }
}
