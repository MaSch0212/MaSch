#nullable enable

namespace MaSch.Core
{
    /// <summary>
    /// When applied to an assembly, the MaSch.Generators reference will generate shims for that assembly.
    /// </summary>
    /// <seealso cref="global::System.Attribute" />
    [global::System.AttributeUsage(global::System.AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    internal class ShimsAttribute : global::System.Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShimsAttribute"/> class.
        /// </summary>
        /// <param name="shims">The shims to generate.</param>
        public ShimsAttribute(Shims shims)
        {
            Shims = shims;
        }

        /// <summary>
        /// Gets the shims to generate.
        /// </summary>
        public Shims Shims { get; }
    }
}