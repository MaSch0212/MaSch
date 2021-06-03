using MaSch.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;

namespace MaSch.Core.Observable.Modules
{
    /// <summary>
    /// The module that handles the calling of events of an <see cref="IObservableObject"/> class.
    /// </summary>
    public class ObservableObjectModule
    {
        private static readonly Dictionary<Type, Dictionary<string, List<string>>> PropertyDependencyCache = new();
        private static readonly object FillCacheLock = new();

        private readonly Type _objectType;
        private readonly IObservableObject _observableObject;
        private readonly Dictionary<string, List<string>> _propertyDependencies;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableObjectModule"/> class.
        /// </summary>
        /// <param name="observableObject">The observable object.</param>
        public ObservableObjectModule(IObservableObject observableObject)
        {
            _observableObject = observableObject;
            _objectType = _observableObject.GetType();

            _propertyDependencies = FillCache(_objectType);
        }

        private static Dictionary<string, List<string>> FillCache(Type type)
        {
            lock (FillCacheLock)
            {
                if (!PropertyDependencyCache.ContainsKey(type))
                {
                    var dependencies = new Dictionary<string, List<string>>();
                    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var p in properties)
                    {
                        var attr = p.GetCustomAttribute<DependsOnAttribute>();
                        if (attr != null)
                        {
                            string prefix = string.Empty;
                            if (typeof(ICommand).IsAssignableFrom(p.PropertyType))
                                prefix = "command:";
                            foreach (var dp in attr.PropertyNames)
                            {
                                if (!dependencies.ContainsKey(dp))
                                    dependencies.Add(dp, new List<string>());
                                dependencies[dp].Add(prefix + p.Name);
                            }
                        }
                    }

                    PropertyDependencyCache.Add(type, dependencies);
                    return dependencies;
                }

                return PropertyDependencyCache[type];
            }
        }

        /// <summary>
        /// Notifies the subscribers that a command has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <exception cref="ArgumentException">A property with the name <paramref name="propertyName"/> does not exist in the object. - <paramref name="propertyName"/>.</exception>
        public void NotifyCommandChanged(string propertyName)
        {
            var property = _objectType.GetProperty(propertyName);
            if (property == null)
                throw new ArgumentException($"A property with the name {propertyName} does not exist in {_objectType.FullName}.", nameof(propertyName));
            var propertyValue = property.GetValue(_observableObject);
            if (propertyValue != null)
            {
                var method = propertyValue.GetType().GetMethod("RaiseCanExecuteChanged", Type.EmptyTypes);
                if (method != null)
                    method.Invoke(propertyValue, null);
            }
        }

        /// <summary>
        /// Notifies the subscribers about changes in dependent properties.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void NotifyDependentProperties(string propertyName)
        {
            if (_propertyDependencies.ContainsKey(propertyName))
            {
                foreach (var p in _propertyDependencies[propertyName])
                {
                    var split = p.Split(':');
                    if (split.Length > 1)
                    {
                        if (split[0] == "command")
                            NotifyCommandChanged(split[1]);
                    }
                    else
                    {
                        _observableObject.NotifyPropertyChanged(p);
                    }
                }
            }
        }
    }
}
