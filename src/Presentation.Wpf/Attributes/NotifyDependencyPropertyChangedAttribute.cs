using MaSch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MaSch.Presentation.Wpf.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotifyDependencyPropertyChangedAttribute : Attribute
    {
        public string PropertyName { get; }
        public Type OwnerType { get; }

        public NotifyDependencyPropertyChangedAttribute(string propertyName) : this (propertyName, null) { }
        public NotifyDependencyPropertyChangedAttribute(string propertyName, Type ownerType)
        {
            PropertyName = propertyName;
            OwnerType = ownerType;
        }

        private static readonly Dictionary<Type, List<(string propertyName, NotifyDependencyPropertyChangedAttribute attribute)>> AttributeCache = new Dictionary<Type, List<(string propertyName, NotifyDependencyPropertyChangedAttribute attribute)>>();
        public static List<(string propertyName, NotifyDependencyPropertyChangedAttribute attribute)> GetAttributes(object classObject)
        {
            Guard.NotNull(classObject, nameof(classObject));
            var classType = classObject.GetType();

            List<(string propertyName, NotifyDependencyPropertyChangedAttribute attribute)> result;
            if (AttributeCache.ContainsKey(classType))
            {
                result = AttributeCache[classType];
            }
            else
            {
                result = (from x in classType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                          let att = x.GetCustomAttribute<NotifyDependencyPropertyChangedAttribute>()
                          where att != null
                          select (x.Name, att)).ToList();
                AttributeCache[classType] = result;
            }
            return result;
        }
    }
}
