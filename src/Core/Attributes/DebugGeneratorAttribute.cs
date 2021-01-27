using System;

namespace MaSch.Core.Attributes
{
    /// <summary>
    /// If this attribute is set to a member that is used by any of the MaSch.Generators source generators, the debugger for that generator is launched.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class DebugGeneratorAttribute : Attribute
    {
    }
}
