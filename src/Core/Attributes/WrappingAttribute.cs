using System;

namespace MaSch.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class WrappingAttribute : Attribute
    {
        public Type TypeToWrap { get; }
        public string? WrappingPropName { get; set; }

        public WrappingAttribute(Type typeToWrap)
        {
            TypeToWrap = typeToWrap;
        }
    }
}
