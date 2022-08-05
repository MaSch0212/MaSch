namespace MaSch.Core
{
    /// <summary>
    /// When applied to an interface, the class that implements it will be marked for the ObservableObjectGenerator from
    /// MaSch.Generators to automatically generate the properties as observable properties.
    /// </summary>
    /// <seealso cref="global::System.Attribute" />
    [global::System.AttributeUsage(global::System.AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    internal class ObservablePropertyDefinitionAttribute : global::System.Attribute
    {
    }
}