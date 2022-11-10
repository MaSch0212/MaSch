using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IEventDeclarationBuilder : ISourceBuilder
    {
        IEventDeclarationBuilder Append(Func<IEventConfigurationFactory, IEventConfiguration> createFunc);
        IEventDeclarationBuilder Append(Func<IEventConfigurationFactory, IEventConfiguration> createFunc, Action<ISourceBuilder> addMethodBuilderFunc, Action<ISourceBuilder> removeMethodBuilderFunc);
    }

    public interface IEventDeclarationBuilder<TBuilder, TConfigFactory> : IEventDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IEventDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IEventConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IEventConfiguration> createFunc);
        TBuilder Append(Func<TConfigFactory, IEventConfiguration> createFunc, Action<ISourceBuilder> addMethodBuilderFunc, Action<ISourceBuilder> removeMethodBuilderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IEventDeclarationBuilder
    {
        IEventDeclarationBuilder IEventDeclarationBuilder.Append(Func<IEventConfigurationFactory, IEventConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        IEventDeclarationBuilder IEventDeclarationBuilder.Append(Func<IEventConfigurationFactory, IEventConfiguration> createFunc, Action<ISourceBuilder> addMethodBuilderFunc, Action<ISourceBuilder> removeMethodBuilderFunc)
            => Append(createFunc(_configurationFactory), addMethodBuilderFunc, removeMethodBuilderFunc);

        private SourceBuilder Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder>? addMethodBuilderFunc, Action<ISourceBuilder>? removeMethodBuilderFunc)
        {
            if (addMethodBuilderFunc is null || removeMethodBuilderFunc is null)
                return AppendWithLineTerminator(eventConfiguration);

            eventConfiguration.WriteTo(this);
            using (AppendLine().AppendBlock())
            {
                Append(eventConfiguration.AddMethod, addMethodBuilderFunc);
                Append(eventConfiguration.RemoveMethod, removeMethodBuilderFunc);
            }

            return this;
        }

        private SourceBuilder Append(IEventMethodConfiguration eventMethodConfiguration, Action<ISourceBuilder> builderFunc)
        {
            if (eventMethodConfiguration.BodyType is MethodBodyType.Expression or MethodBodyType.ExpressionNewLine)
                return AppendAsExpression(eventMethodConfiguration, this, builderFunc, false);

            return AppendAsBlock(eventMethodConfiguration, this, builderFunc);
        }
    }
}