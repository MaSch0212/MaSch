using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration
{
    public interface ISuperConstructorConfiguration : ICodeConfiguration
    {
        ISuperConstructorConfiguration WithParameter(string value);
    }

    internal sealed class SuperConstructorConfiguration : CodeConfigurationBase, ISuperConstructorConfiguration
    {
        private readonly string _superConstructorKeyword;
        private readonly List<string> _parameterValues = new();

        public SuperConstructorConfiguration(string superConstructorKeyword)
        {
            _superConstructorKeyword = superConstructorKeyword;
        }

        protected override int StartCapacity => 16;

        public ISuperConstructorConfiguration WithParameter(string value)
        {
            _parameterValues.Add(value);
            return this;
        }

        public override void WriteTo(ISourceBuilder sourceBuilder)
        {
            sourceBuilder.Append(": ").Append(_superConstructorKeyword).Append('(');

            using (sourceBuilder.Indent())
            {
                for (int i = 0; i < _parameterValues.Count; i++)
                {
                    sourceBuilder.Append(_parameterValues[i]);
                    if (i < _parameterValues.Count - 1)
                        sourceBuilder.Append(", ");
                }
            }

            sourceBuilder.Append(')');
        }
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    /// <summary>
    /// Provides extension methods for the <see cref="ISuperConstructorConfiguration"/> interface.
    /// </summary>
    public static class SuperConstructorConfigurationExtensions
    {
        public static ISuperConstructorConfiguration WithParameters(this ISuperConstructorConfiguration builder, string param1)
            => builder.WithParameter(param1);

        public static ISuperConstructorConfiguration WithParameters(this ISuperConstructorConfiguration builder, string param1, string param2)
            => builder.WithParameter(param1).WithParameter(param2);

        public static ISuperConstructorConfiguration WithParameters(this ISuperConstructorConfiguration builder, string param1, string param2, string param3)
            => builder.WithParameter(param1).WithParameter(param2).WithParameter(param3);

        public static ISuperConstructorConfiguration WithParameters(this ISuperConstructorConfiguration builder, string param1, string param2, string param3, string param4)
            => builder.WithParameter(param1).WithParameter(param2).WithParameter(param3).WithParameter(param4);

        public static ISuperConstructorConfiguration WithParameters(this ISuperConstructorConfiguration builder, params string[] @params)
        {
            foreach (var p in @params)
                builder = builder.WithParameter(p);
            return builder;
        }
    }
}