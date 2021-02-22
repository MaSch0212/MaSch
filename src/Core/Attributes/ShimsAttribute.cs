using System;

namespace MaSch.Core.Attributes
{
    /// <summary>
    /// When applied to an assembly, the MaSch.Generators reference will generate shims for that assembly.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class ShimsAttribute : Attribute
    {
        /// <summary>
        /// Gets the shims to generate.
        /// </summary>
        public Shims Shims { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShimsAttribute"/> class.
        /// </summary>
        /// <param name="shims">The shims to generate.</param>
        public ShimsAttribute(Shims shims)
        {
            Shims = shims;
        }
    }
}
