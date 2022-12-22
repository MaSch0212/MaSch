using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration
{
    /// <summary>
    /// Represents configuration of a generic parameter code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
    public interface IGenericParameterConfiguration
    {
        /// <summary>
        /// Gets the name of the generic parameter represented by this <see cref="IGenericParameterConfiguration"/>.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the variance of the generic parameter represented by this <see cref="IGenericParameterConfiguration"/>.
        /// </summary>
        GenericParameterVariance Variance { get; }

        /// <summary>
        /// Gets a read-only list of constraints for the generic parameter represented by this <see cref="IGenericParameterConfiguration"/>.
        /// </summary>
        IReadOnlyList<string> Constraints { get; }

        /// <summary>
        /// Sets the variance of the generic parameter represented by this <see cref="IGenericParameterConfiguration"/>.
        /// </summary>
        /// <param name="variance">The variance to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IGenericParameterConfiguration WithVariance(GenericParameterVariance variance);

        /// <summary>
        /// Adds a constraint to the generic parameter represented by this <see cref="IGenericParameterConfiguration"/>.
        /// </summary>
        /// <param name="constraint">The constraint to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IGenericParameterConfiguration WithConstraint(string constraint);

        /// <summary>
        /// Writes the parameter part of the generic parameter represented by this <see cref="IGenericParameterConfiguration"/> to the target <see cref="ISourceBuilder"/>.
        /// </summary>
        /// <param name="sourceBuilder">The <see cref="ISourceBuilder"/> to write the code to.</param>
        void WriteParameterTo(ISourceBuilder sourceBuilder);

        /// <summary>
        /// Writes the constraints of the generic parameter represented by this <see cref="IGenericParameterConfiguration"/> to the target <see cref="ISourceBuilder"/>.
        /// </summary>
        /// <param name="sourceBuilder">The <see cref="ISourceBuilder"/> to write the code to.</param>
        void WriteConstraintTo(ISourceBuilder sourceBuilder);
    }

    internal sealed class GenericParameterConfiguration : IGenericParameterConfiguration
    {
        private readonly List<string> _constraints = new();

        public GenericParameterConfiguration(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public GenericParameterVariance Variance { get; private set; } = GenericParameterVariance.None;
        public IReadOnlyList<string> Constraints => new ReadOnlyCollection<string>(_constraints);

        public IGenericParameterConfiguration WithVariance(GenericParameterVariance variance)
        {
            Variance = variance;
            return this;
        }

        public IGenericParameterConfiguration WithConstraint(string constraint)
        {
            _constraints.Add(constraint);
            return this;
        }

        public void WriteParameterTo(ISourceBuilder sourceBuilder)
        {
            if (Variance is not GenericParameterVariance.None)
                sourceBuilder.Append(Variance.ToParameterPrefix());
            sourceBuilder.Append(Name);
        }

        /// <inheritdoc/>
        public void WriteConstraintTo(ISourceBuilder sourceBuilder)
        {
            if (_constraints.Count == 0)
                return;

            sourceBuilder.Append($"where {Name} : ");

            bool isFirst = true;
            foreach (var constraint in _constraints)
            {
                if (!isFirst)
                    sourceBuilder.Append(", ");
                sourceBuilder.Append(constraint);

                isFirst = false;
            }
        }
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    /// <summary>
    /// Provides extension methods for the <see cref="IGenericParameterConfiguration"/> interface.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Extensions")]
    public static class GenericParameterConfigurationExtensions
    {
        /// <summary>
        /// Sets the variance of the generic parameter represented by this <see cref="IGenericParameterConfiguration"/> to <see cref="GenericParameterVariance.Covariant"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IGenericParameterConfiguration AsCovariant(this IGenericParameterConfiguration config)
            => config.WithVariance(GenericParameterVariance.Covariant);

        /// <summary>
        /// Sets the variance of the generic parameter represented by this <see cref="IGenericParameterConfiguration"/> to <see cref="GenericParameterVariance.Contravariant"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IGenericParameterConfiguration AsContravariant(this IGenericParameterConfiguration config)
            => config.WithVariance(GenericParameterVariance.Contravariant);
    }
}