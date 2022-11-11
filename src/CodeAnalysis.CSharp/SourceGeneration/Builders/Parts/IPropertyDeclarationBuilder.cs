using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IPropertyDeclarationBuilder : ISourceBuilder
    {
        IPropertyDeclarationBuilder Append(Func<IPropertyConfigurationFactory, IPropertyConfiguration> createFunc);
        IPropertyDeclarationBuilder Append(Func<IPropertyConfigurationFactory, IPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
        IPropertyDeclarationBuilder Append(Func<IPropertyConfigurationFactory, IReadOnlyPropertyConfiguration> createFunc);
        IPropertyDeclarationBuilder Append(Func<IPropertyConfigurationFactory, IReadOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc);
        IPropertyDeclarationBuilder Append(Func<IPropertyConfigurationFactory, IWriteOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc);
    }

    public interface IPropertyDeclarationBuilder<TBuilder, TConfigFactory> : IPropertyDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IPropertyDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IPropertyConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IPropertyConfiguration> createFunc);
        TBuilder Append(Func<TConfigFactory, IPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
        TBuilder Append(Func<TConfigFactory, IReadOnlyPropertyConfiguration> createFunc);
        TBuilder Append(Func<TConfigFactory, IReadOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc);
        TBuilder Append(Func<TConfigFactory, IWriteOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IPropertyDeclarationBuilder
    {
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(Func<IPropertyConfigurationFactory, IPropertyConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(Func<IPropertyConfigurationFactory, IPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, setBuilderFunc);

        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(Func<IPropertyConfigurationFactory, IReadOnlyPropertyConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(Func<IPropertyConfigurationFactory, IReadOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, null);

        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(Func<IPropertyConfigurationFactory, IWriteOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), null, setBuilderFunc);

        private SourceBuilder Append(IPropertyConfigurationBase propertyConfiguration, Action<ISourceBuilder>? getBuilderFunc, Action<ISourceBuilder>? setBuilderFunc)
        {
            var readOnlyPropertyConfiguration = propertyConfiguration as IReadOnlyPropertyConfigurationBase;
            if (readOnlyPropertyConfiguration?.GetBodyType is PropertyGetMethodType.Expression or PropertyGetMethodType.ExpressionNewLine &&
                getBuilderFunc is not null)
            {
                return AppendAsExpression(propertyConfiguration, this, getBuilderFunc, readOnlyPropertyConfiguration.GetBodyType is PropertyGetMethodType.ExpressionNewLine);
            }

            propertyConfiguration.WriteTo(this);
            var getConfig = (propertyConfiguration as IPropertyHasGetConfiguration)?.GetMethod;
            var setConfig = (propertyConfiguration as IPropertyHasSetConfiguration)?.SetMethod;
            var multiline =
                getConfig.ShouldBeOnItsOwnLine ||
                setConfig.ShouldBeOnItsOwnLine ||
                getBuilderFunc is not null ||
                setBuilderFunc is not null;

            SourceBuilderCodeBlock block;
            if (multiline)
            {
                block = AppendLine().AppendBlock();
            }
            else
            {
                Append("{ ");
                block = Indent();
            }

            using (block)
            {
                if (getConfig is not null && readOnlyPropertyConfiguration?.GetBodyType is not PropertyGetMethodType.Initialize)
                    Append(getConfig, getBuilderFunc, multiline);
                if (setConfig is not null)
                    Append(setConfig, getBuilderFunc, multiline);
            }

            if (!multiline)
                Append(" }");

            if (readOnlyPropertyConfiguration?.GetBodyType is PropertyGetMethodType.Initialize && getBuilderFunc is not null)
            {
                Append(" = ");
                getBuilderFunc(this);
                Append(';');
            }

            return this;
        }

        private SourceBuilder Append(IPropertyMethodConfiguration propertyMethodConfiguration, Action<ISourceBuilder>? builderFunc, bool multiline)
        {
            if (builderFunc is null)
                return AppendWithLineTerminator(propertyMethodConfiguration, multiline);

            if (propertyMethodConfiguration.BodyType is MethodBodyType.Expression or MethodBodyType.ExpressionNewLine)
                return AppendAsExpression(propertyMethodConfiguration, this, builderFunc, false);

            return AppendAsBlock(propertyMethodConfiguration, this, builderFunc);
        }
    }
}