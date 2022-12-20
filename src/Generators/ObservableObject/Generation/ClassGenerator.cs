using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.Generators.ObservableObject.Models;

namespace MaSch.Generators.ObservableObject.Generation;

internal static class ClassGenerator
{
    public static T AppendNotifyPropertyChangedImplementationClass<T>(this T builder, string typeName)
        where T : IClassDeclarationBuilder
    {
        builder.Append(Class(typeName).AsPartial().Implements(TypeNames.INotifyPropertyChanged), builder =>
        {
            builder
                .AppendPropertyChangedEvent()
                .AppendSetPropertyMethod(InterfaceType.NotifyPropertyChanged)
                .AppendOnPropertyChangedMethod();
        });

        return builder;
    }

    public static T AppendObservableObjectImplementationClass<T>(this T builder, string typeName)
        where T : IClassDeclarationBuilder
    {
        builder.Append(Class(typeName).AsPartial().Implements(TypeNames.IObservableObject), builder =>
        {
            builder
                .AppendObservableObjectProperties()
                .AppendPropertyChangedEvent()
                .AppendIsNotifyEnabledProperty()
                .AppendSetPropertyMethod(InterfaceType.ObservableObject)
                .AppendNotifyPropertyChangedMethod()
                .AppendNotifyCommandChangedMethod();
        });

        return builder;
    }
}
