using MaSch.Core.Observable;
using System;

namespace MaSch.Core.Attributes
{
    /// <summary>
    /// Skips the chage tracking on the property in a class that derives from <see cref="ObservableChangeTrackingObject"/>.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class NoChangeTrackingAttribute : Attribute { }

    /// <summary>
    /// Enables recusive change tracking for the property in a class that derives from <see cref="ObservableChangeTrackingObject"/>.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class RecursiveChangeTrackingAttribute : Attribute { }
}
