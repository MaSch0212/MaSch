using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MaSch.Core
{
    /// <summary>
    /// Specifies the type of version for an assembly.
    /// </summary>
    public enum AssemblyVersionType
    {
        /// <summary>
        /// The assembly version.
        /// </summary>
        AssemblyVersion,

        /// <summary>
        /// The assembly file version.
        /// </summary>
        AssemblyFileVersion,

        /// <summary>
        /// The assembly informational version.
        /// </summary>
        AssemblyInformationalVersion,
    }

    /// <summary>
    /// Provides extensions for the <see cref="AssemblyVersionType"/> enum.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "The file name matches the enum.")]
    public static class AssemblyVersionTypeExtensions
    {
        /// <summary>
        /// Gets the version from an <see cref="Assembly"/>.
        /// </summary>
        /// <param name="type">The version type to get.</param>
        /// <param name="assembly">The assembly to get the version from.</param>
        /// <returns>The version as <see cref="string"/> for the given <see cref="Assembly"/>.</returns>
        public static string? GetVersion(this AssemblyVersionType type, Assembly? assembly)
        {
            if (assembly == null)
                return null;
            return type switch
            {
                AssemblyVersionType.AssemblyFileVersion => assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version,
                AssemblyVersionType.AssemblyInformationalVersion => assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
                _ => assembly.GetName().Version?.ToString(),
            };
        }

        /// <summary>
        /// Gets the version from the entry <see cref="Assembly"/>.
        /// </summary>
        /// <param name="type">The version type to get.</param>
        /// <returns>The version as <see cref="string"/> for the entry <see cref="Assembly"/>.</returns>
        public static string? GetVersion(this AssemblyVersionType type)
        {
            return GetVersion(type, Assembly.GetEntryAssembly());
        }
    }
}
