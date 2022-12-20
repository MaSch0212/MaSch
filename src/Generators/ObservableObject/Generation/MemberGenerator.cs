using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.Generators.ObservableObject.Models;

namespace MaSch.Generators.ObservableObject.Generation;

internal static class MemberGenerator
{
    public static T AppendObservableObjectProperties<T>(this T builder)
        where T : IFieldDeclarationBuilder, IPropertyDeclarationBuilder
    {
        builder
            .Append(Field(TypeNames.Dictionary("string", TypeNames.NotifyPropertyChangedAttribute), "__attributes").AsPrivate())
            .Append(Field(TypeNames.ObservableObjectModule, "__module").AsPrivate());
        builder
            .Append(
                Property(TypeNames.Dictionary("string", TypeNames.NotifyPropertyChangedAttribute), "_attributes").AsPrivate().AsReadOnly().AsExpression(true),
                builder => builder.Append($"__attributes ??= {TypeNames.NotifyPropertyChangedAttribute}.InitializeAll(this)"))
            .Append(
                Property(TypeNames.ObservableObjectModule, "_module").AsPrivate().AsReadOnly().AsExpression(true),
                builder => builder.Append($"__module ??= new {TypeNames.ObservableObjectModule}(this)"));

        return builder;
    }

    public static T AppendPropertyChangedEvent<T>(this T builder)
        where T : IEventDeclarationBuilder
    {
        builder.Append(
            Event(TypeNames.PropertyChangedEventHandler, "PropertyChanged")
                .AsPublic()
                .WithDocComment("<inheritdoc/>"));

        return builder;
    }

    public static T AppendIsNotifyEnabledProperty<T>(this T builder)
        where T : IPropertyDeclarationBuilder
    {
        builder.Append(
            Property("bool", "IsNotifyEnabled")
                .AsPublicVirtual()
                .WithValue("true")
                .WithDocComment("<inheritdoc/>"));

        return builder;
    }

    public static T AppendSetPropertyMethod<T>(this T builder, InterfaceType interfaceType)
        where T : IMethodDeclarationBuilder
    {
        var methodConfig = Method("SetProperty")
            .AsPublicVirtual()
            .WithGenericParameter("T")
            .WithParameter("ref T", "property")
            .WithParameter("T", "value")
            .WithParameter("string", "propertyName", p => p.WithDefaultValue("null").WithCodeAttribute(TypeNames.CallerMemberNameAttribute));

        if (interfaceType == InterfaceType.ObservableObject)
        {
            methodConfig.WithDocComment("<inheritdoc/>");
        }
        else
        {
            methodConfig.WithDocComment(
                """
                <summary>Sets the specified property and notifies subscribers about the change.</summary>
                <typeparam name="T">The type of the property to set.</typeparam>
                <param name="property">The property backing field.</param>
                <param name="value">The value to set.</param>
                <param name="propertyName">Name of the property.</param>
                """);
        }

        builder.Append(
            methodConfig,
            builder =>
            {
                if (interfaceType == InterfaceType.ObservableObject)
                {
                    builder
                        .AppendBlock("if (_attributes.ContainsKey(propertyName))", builder =>
                            builder.AppendLine("_attributes[propertyName].UnsubscribeEvent(this);"))
                        .AppendLine("property = value;")
                        .AppendLine("NotifyPropertyChanged(propertyName);")
                        .AppendBlock("if (_attributes.ContainsKey(propertyName))", builder =>
                            builder.AppendLine("_attributes[propertyName].SubscribeEvent(this);"));
                }
                else
                {
                    builder
                        .AppendLine("property = value;")
                        .AppendLine("OnPropertyChanged(propertyName);");
                }
            });

        return builder;
    }

    public static T AppendNotifyPropertyChangedMethod<T>(this T builder)
        where T : IMethodDeclarationBuilder
    {
        builder.Append(
            Method("NotifyPropertyChanged")
                .AsPublicVirtual()
                .WithParameter("string", "propertyName", p => p.WithDefaultValue("\"\"").WithCodeAttribute(TypeNames.CallerMemberNameAttribute))
                .WithParameter("bool", "notifyDependencies", p => p.WithDefaultValue("true"))
                .WithDocComment("<inheritdoc/>"),
            builder => builder
                .AppendBlock("if (IsNotifyEnabled)", builder => builder
                    .AppendLine("PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));")
                    .AppendBlock("if (notifyDependencies)", builder => builder
                        .AppendLine("_module.NotifyDependentProperties(propertyName);"))));

        return builder;
    }

    public static T AppendNotifyCommandChangedMethod<T>(this T builder)
        where T : IMethodDeclarationBuilder
    {
        builder.Append(
            Method("NotifyCommandChanged")
                .AsPublicVirtual()
                .WithParameter("string", "propertyName", p => p.WithDefaultValue("\"\"").WithCodeAttribute(TypeNames.CallerMemberNameAttribute))
                .WithDocComment("<inheritdoc/>"),
            builder => builder
                .AppendBlock("if (IsNotifyEnabled)", builder => builder
                    .AppendLine("_module.NotifyCommandChanged(propertyName);")));

        return builder;
    }

    public static T AppendOnPropertyChangedMethod<T>(this T builder)
        where T : IMethodDeclarationBuilder
    {
        builder.Append(
            Method("OnPropertyChanged")
                .AsProtectedVirtual()
                .WithParameter("string", "propertyName", p => p.WithDefaultValue("\"\"").WithCodeAttribute(TypeNames.CallerMemberNameAttribute)),
            builder => builder
                .AppendLine("PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));"));

        return builder;
    }
}
