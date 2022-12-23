using MaSch.CodeAnalysis.CSharp.Extensions;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration
{
    /// <summary>
    /// Represents configuration of a code attribute code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
    public interface ICodeAttributeConfiguration : ICodeConfiguration
    {
        /// <summary>
        /// Gets the type name of the code attribute represented by this <see cref="ICodeAttributeConfiguration"/>.
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Gets a read-only list of parameters used for initialization of the code attribute represented by this <see cref="ICodeAttributeConfiguration"/>.
        /// </summary>
        IReadOnlyList<string> Parameters { get; }

        /// <summary>
        /// Gets the target for the code attribute represented by this <see cref="ICodeAttributeConfiguration"/>.
        /// </summary>
        CodeAttributeTarget Target { get; }

        /// <summary>
        /// Sets the target of the code attribute represented by this <see cref="ICodeAttributeConfiguration"/>.
        /// </summary>
        /// <param name="target">The target to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        ICodeAttributeConfiguration OnTarget(CodeAttributeTarget target);

        /// <summary>
        /// Adds a parameter to the parameters used for initialization of the code attribute represented by this <see cref="ICodeAttributeConfiguration"/>.
        /// </summary>
        /// <param name="value">The parameter value to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>The <paramref name="value"/> is interpreted as C# code. If you want to add a string value as a parameter call the <see cref="StringExtensions.ToCSharpLiteral(string?, bool)"/> extension method on the string.</remarks>
        ICodeAttributeConfiguration WithParameter(string value);
    }

    internal sealed class CodeAttributeConfiguration : CodeConfigurationBase, ICodeAttributeConfiguration
    {
        private readonly List<string> _parameters = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeAttributeConfiguration"/> class.
        /// </summary>
        /// <param name="typeName">The type name of the code attribute.</param>
        public CodeAttributeConfiguration(string typeName)
        {
            TypeName = typeName;
        }

        public string TypeName { get; }
        public IReadOnlyList<string> Parameters => new ReadOnlyCollection<string>(_parameters);
        public CodeAttributeTarget Target { get; private set; }

        protected override int StartCapacity => 128;

        public static CodeAttributeConfiguration AddCodeAttribute(IList<ICodeAttributeConfiguration> codeAttributes, string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
        {
            var codeAttribute = new CodeAttributeConfiguration(attributeTypeName);
            attributeConfiguration?.Invoke(codeAttribute);
            codeAttributes.Add(codeAttribute);
            return codeAttribute;
        }

        public ICodeAttributeConfiguration OnTarget(CodeAttributeTarget target)
        {
            Target = target;
            return this;
        }

        public ICodeAttributeConfiguration WithParameter(string value)
        {
            _parameters.Add(value);
            return this;
        }

        public override void WriteTo(ISourceBuilder sourceBuilder)
        {
            sourceBuilder.Append('[');

            if (Target is not CodeAttributeTarget.Default)
                sourceBuilder.Append(Target.ToAttributePrefix());

            sourceBuilder.Append(TypeName);

            if (_parameters.Count > 0)
            {
                sourceBuilder.Append('(');
                bool isFirst = true;
                foreach (var p in _parameters)
                {
                    if (!isFirst)
                        sourceBuilder.Append(", ");
                    sourceBuilder.Append(p);
                    isFirst = false;
                }

                sourceBuilder.Append(')');
            }

            sourceBuilder.Append(']');
        }
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    /// <summary>
    /// Provides extension methods for the <see cref="ICodeAttributeConfiguration"/> interface.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Extensions")]
    public static class CodeAttributeConfigurationExtensions
    {
        /// <summary>
        /// Sets the target of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="CodeAttributeTarget.Assembly"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration OnAssembly(this ICodeAttributeConfiguration config)
            => config.OnTarget(CodeAttributeTarget.Assembly);

        /// <summary>
        /// Sets the target of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="CodeAttributeTarget.Module"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration OnModule(this ICodeAttributeConfiguration config)
            => config.OnTarget(CodeAttributeTarget.Module);

        /// <summary>
        /// Sets the target of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="CodeAttributeTarget.Field"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration OnField(this ICodeAttributeConfiguration config)
            => config.OnTarget(CodeAttributeTarget.Field);

        /// <summary>
        /// Sets the target of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="CodeAttributeTarget.Event"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration OnEvent(this ICodeAttributeConfiguration config)
            => config.OnTarget(CodeAttributeTarget.Event);

        /// <summary>
        /// Sets the target of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="CodeAttributeTarget.Method"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration OnMethod(this ICodeAttributeConfiguration config)
            => config.OnTarget(CodeAttributeTarget.Method);

        /// <summary>
        /// Sets the target of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="CodeAttributeTarget.Parameter"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration OnParameter(this ICodeAttributeConfiguration config)
            => config.OnTarget(CodeAttributeTarget.Parameter);

        /// <summary>
        /// Sets the target of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="CodeAttributeTarget.Property"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration OnProperty(this ICodeAttributeConfiguration config)
            => config.OnTarget(CodeAttributeTarget.Property);

        /// <summary>
        /// Sets the target of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="CodeAttributeTarget.Return"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration OnReturn(this ICodeAttributeConfiguration config)
            => config.OnTarget(CodeAttributeTarget.Return);

        /// <summary>
        /// Sets the target of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="CodeAttributeTarget.Type"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration OnType(this ICodeAttributeConfiguration config)
            => config.OnTarget(CodeAttributeTarget.Type);

        /// <summary>
        /// Adds parameters to the parameters used for initialization of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="param1">The first parameter value to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration WithParameters(this ICodeAttributeConfiguration config, string param1)
            => config.WithParameter(param1);

        /// <summary>
        /// Adds parameters to the parameters used for initialization of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="param1">The first parameter value to add.</param>
        /// <param name="param2">The second parameter value to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration WithParameters(this ICodeAttributeConfiguration config, string param1, string param2)
            => config.WithParameter(param1).WithParameter(param2);

        /// <summary>
        /// Adds parameters to the parameters used for initialization of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="param1">The first parameter value to add.</param>
        /// <param name="param2">The second parameter value to add.</param>
        /// <param name="param3">The third parameter value to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration WithParameters(this ICodeAttributeConfiguration config, string param1, string param2, string param3)
            => config.WithParameter(param1).WithParameter(param2).WithParameter(param3);

        /// <summary>
        /// Adds parameters to the parameters used for initialization of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="param1">The first parameter value to add.</param>
        /// <param name="param2">The second parameter value to add.</param>
        /// <param name="param3">The third parameter value to add.</param>
        /// <param name="param4">The forth parameter value to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration WithParameters(this ICodeAttributeConfiguration config, string param1, string param2, string param3, string param4)
            => config.WithParameter(param1).WithParameter(param2).WithParameter(param3).WithParameter(param4);

        /// <summary>
        /// Adds parameters to the parameters used for initialization of the code attribute represented by this <see cref="ISupportsAccessModifierConfiguration"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="params">The parameter values to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ICodeAttributeConfiguration WithParameters(this ICodeAttributeConfiguration config, params string[] @params)
        {
            foreach (var p in @params)
                config = config.WithParameter(p);
            return config;
        }
    }
}