using MaSch.CodeAnalysis.CSharp.Extensions;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration
{
    /// <summary>
    /// Represents configuration of a constructor call code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
    public interface ISuperConstructorConfiguration : ICodeConfiguration
    {
        /// <summary>
        /// Gets the keyword (<c>base</c>/<c>this</c>) of the constructor call represented by this <see cref="ISuperConstructorConfiguration"/>.
        /// </summary>
        string SuperConstructorKeyword { get; }

        /// <summary>
        /// Gets a read-only list of parameters used for the constructor call represented by this <see cref="ICodeAttributeConfiguration"/>.
        /// </summary>
        IReadOnlyList<string> Parameters { get; }

        /// <summary>
        /// Adds a parameter to the constructor call represented by this <see cref="ICodeAttributeConfiguration"/>.
        /// </summary>
        /// <param name="value">The parameter value to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>The <paramref name="value"/> is interpreted as C# code. If you want to add a string value as a parameter call the <see cref="StringExtensions.ToCSharpLiteral(string?, bool)"/> extension method on the string.</remarks>
        ISuperConstructorConfiguration WithParameter(string value);
    }

    internal sealed class SuperConstructorConfiguration : CodeConfigurationBase, ISuperConstructorConfiguration
    {
        private readonly List<string> _parameterValues = new();

        public SuperConstructorConfiguration(string superConstructorKeyword)
        {
            SuperConstructorKeyword = superConstructorKeyword;
        }

        public string SuperConstructorKeyword { get; }
        public IReadOnlyList<string> Parameters => new ReadOnlyCollection<string>(_parameterValues);

        protected override int StartCapacity => 16;

        public ISuperConstructorConfiguration WithParameter(string value)
        {
            _parameterValues.Add(value);
            return this;
        }

        public override void WriteTo(ISourceBuilder sourceBuilder)
        {
            sourceBuilder.Append(": ").Append(SuperConstructorKeyword).Append('(');

            sourceBuilder.Indent(sourceBuilder =>
            {
                for (int i = 0; i < _parameterValues.Count; i++)
                {
                    sourceBuilder.Append(_parameterValues[i]);
                    if (i < _parameterValues.Count - 1)
                        sourceBuilder.Append(", ");
                }
            });

            sourceBuilder.Append(')');
        }
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    /// <summary>
    /// Provides extension methods for the <see cref="ISuperConstructorConfiguration"/> interface.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Extensions")]
    public static class SuperConstructorConfigurationExtensions
    {
        /// <summary>
        /// Adds a parameters to the constructor call represented by this <see cref="ICodeAttributeConfiguration"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="param1">The first parameter value to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ISuperConstructorConfiguration WithParameters(this ISuperConstructorConfiguration config, string param1)
            => config.WithParameter(param1);

        /// <summary>
        /// Adds a parameters to the constructor call represented by this <see cref="ICodeAttributeConfiguration"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="param1">The first parameter value to add.</param>
        /// <param name="param2">The second parameter value to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ISuperConstructorConfiguration WithParameters(this ISuperConstructorConfiguration config, string param1, string param2)
            => config.WithParameter(param1).WithParameter(param2);

        /// <summary>
        /// Adds a parameters to the constructor call represented by this <see cref="ICodeAttributeConfiguration"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="param1">The first parameter value to add.</param>
        /// <param name="param2">The second parameter value to add.</param>
        /// <param name="param3">The third parameter value to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ISuperConstructorConfiguration WithParameters(this ISuperConstructorConfiguration config, string param1, string param2, string param3)
            => config.WithParameter(param1).WithParameter(param2).WithParameter(param3);

        /// <summary>
        /// Adds a parameters to the constructor call represented by this <see cref="ICodeAttributeConfiguration"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="param1">The first parameter value to add.</param>
        /// <param name="param2">The second parameter value to add.</param>
        /// <param name="param3">The third parameter value to add.</param>
        /// <param name="param4">The forth parameter value to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ISuperConstructorConfiguration WithParameters(this ISuperConstructorConfiguration config, string param1, string param2, string param3, string param4)
            => config.WithParameter(param1).WithParameter(param2).WithParameter(param3).WithParameter(param4);

        /// <summary>
        /// Adds a parameters to the constructor call represented by this <see cref="ICodeAttributeConfiguration"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="params">The parameter values to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ISuperConstructorConfiguration WithParameters(this ISuperConstructorConfiguration config, params string[] @params)
        {
            foreach (var p in @params)
                config = config.WithParameter(p);
            return config;
        }
    }
}