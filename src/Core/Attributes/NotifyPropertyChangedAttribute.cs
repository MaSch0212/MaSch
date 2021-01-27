using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using MaSch.Core.Extensions;

namespace MaSch.Core.Attributes
{
    /// <summary>
    /// When applied to a property, the given method is executed if the property have changed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NotifyPropertyChangedAttribute : Attribute
    {
        private readonly Dictionary<object, EventCacheItem> _eventCache = new Dictionary<object, EventCacheItem>();

        /// <summary>
        /// Gets a value indicating whether the <see cref="NotifyPropertyChangedAttribute"/> is initialized.
        /// </summary>
        public bool IsInitialized => _propertyInfo != null && _callbackMethod != null;

        private readonly string _callbackMethodName;
        private PropertyInfo? _propertyInfo;
        private MethodInfo? _callbackMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyPropertyChangedAttribute"/> class.
        /// </summary>
        /// <param name="methodName">The name of the method which is executed when the property have changed.</param>
        public NotifyPropertyChangedAttribute(string methodName)
        {
            Guard.NotNull(methodName, nameof(methodName));

            _callbackMethodName = methodName;
        }

        /// <summary>
        /// Initializes the attribute.
        /// </summary>
        /// <param name="property">The property on which the attribute is applied.</param>
        /// <param name="reinitialize">Determines wether the attribute should do the initialization even if it already is initialized.</param>
        /// <exception cref="InvalidOperationException">
        /// The NotifyPropertyChangedAttribute can only be used on Properties which type implements the INotifyPropertyChanged interface.
        /// or
        /// The method could not be found in the class.
        /// </exception>
        public void Initialize(PropertyInfo property, bool reinitialize = false)
        {
            Guard.NotNull(property, nameof(property));

            if (IsInitialized && !reinitialize)
                return;
            if (!typeof(INotifyPropertyChanged).IsAssignableFrom(property.PropertyType))
            {
                throw new InvalidOperationException("The NotifyPropertyChangedAttribute can only be used on Properties which type implements the INotifyPropertyChanged interface. Property: " +
                    property.DeclaringType?.FullName + "." + property.Name);
            }

            _propertyInfo = property;
            _callbackMethod = _propertyInfo.DeclaringType?.GetMethod(
                _callbackMethodName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static,
                null,
                new[] { typeof(object), typeof(PropertyChangedEventArgs) },
                null);
            if (_callbackMethod == null)
               throw new InvalidOperationException($"The method \"{_callbackMethodName}(object, PropertyChangedEventArgs)\" could not be found in the class \"{_propertyInfo.PropertyType.FullName}\"");
        }

        /// <summary>
        /// Subscribes the <see cref="INotifyPropertyChanged.PropertyChanged"/> event of the property value of the provided object.
        /// </summary>
        /// <param name="classObject">An instance of the class in which the property, on which this attribute is applied, is contained in.</param>
        /// <exception cref="InvalidOperationException">You first need to initialize this attribute.</exception>
        /// <exception cref="ArgumentException">The declaring type of the property does not match the type of the given object.</exception>
        public void SubscribeEvent(object classObject)
        {
            Guard.NotNull(classObject, nameof(classObject));

            if (!IsInitialized || _propertyInfo == null)
                throw new InvalidOperationException("You first need to initialize this attribute.");
            if (_propertyInfo.DeclaringType != classObject.GetType())
                throw new ArgumentException($"The declaring type of the property does not match the type of the given object. Expected: {_propertyInfo?.DeclaringType?.FullName ?? "(null)"}, Actual: {classObject.GetType().FullName}");

            var value = (INotifyPropertyChanged?)_propertyInfo.GetValue(classObject);
            if (value != null)
            {
                if (!_eventCache.ContainsKey(classObject))
                    _eventCache.Add(classObject, new EventCacheItem(GetOnPropertyChanged(classObject)));
                value.PropertyChanged += _eventCache[classObject].EventHandler;
                _eventCache[classObject].SubscribeCount++;
            }
        }

        /// <summary>
        /// Unsubscribes the <see cref="INotifyPropertyChanged.PropertyChanged"/> event of the property value of the provided object.
        /// </summary>
        /// <param name="classObject">An instance of the class in which the property, on which this attribute is applied, is contained in.</param>
        /// <exception cref="InvalidOperationException">You first need to initialize this attribute.</exception>
        /// <exception cref="ArgumentException">The declaring type of the property does not match the type of the given object.</exception>
        public void UnsubscribeEvent(object classObject)
        {
            Guard.NotNull(classObject, nameof(classObject));
            if (!IsInitialized || _propertyInfo == null)
                throw new InvalidOperationException("You first need to initialize this attribute.");
            if (_propertyInfo.DeclaringType != classObject.GetType())
                throw new ArgumentException("The declaring type of the property does not match the type of the given object.");

            var value = (INotifyPropertyChanged?)_propertyInfo.GetValue(classObject);
            if (value != null && _eventCache.ContainsKey(classObject))
            {
                var cache = _eventCache[classObject];
                value.PropertyChanged -= cache.EventHandler;
                if (--cache.SubscribeCount <= 0)
                    _eventCache.Remove(classObject);
            }
        }

        private PropertyChangedEventHandler GetOnPropertyChanged(object classObject)
        {
            return (s, e) => _callbackMethod?.Invoke(classObject, new[] { s, e });
        }

        private static readonly Dictionary<Type, Dictionary<PropertyInfo, NotifyPropertyChangedAttribute>> AttributeCache = new Dictionary<Type, Dictionary<PropertyInfo, NotifyPropertyChangedAttribute>>();
        private static Dictionary<PropertyInfo, NotifyPropertyChangedAttribute> GetAttributes(Type classType)
        {
            Dictionary<PropertyInfo, NotifyPropertyChangedAttribute> result;
            if (AttributeCache.ContainsKey(classType))
            {
                result = AttributeCache[classType];
            }
            else
            {
                result = (from x in classType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                          let att = x.GetCustomAttribute<NotifyPropertyChangedAttribute>()
                          where att != null
                          select new { property = x, attribute = att }).ToDictionary(x => x.property, x => x.attribute);
                AttributeCache[classType] = result;
            }

            return result;
        }

        /// <summary>
        /// Initializes all properties that have an <see cref="NotifyParentPropertyAttribute"/> attached.
        /// </summary>
        /// <param name="classObject">The object on which to initialize the <see cref="NotifyParentPropertyAttribute"/>s to.</param>
        /// <param name="reinitialize">Determines wether the attributes should do the initialization even if they already are initialized.</param>
        /// <returns>Returns a dictionary that contains the <see cref="NotifyParentPropertyAttribute"/> of all properties.</returns>
        public static Dictionary<string, NotifyPropertyChangedAttribute> InitializeAll(object classObject, bool reinitialize = false)
        {
            Guard.NotNull(classObject, nameof(classObject));

            var result = GetAttributes(classObject.GetType());
            result.ForEach(x => x.Value.Initialize(x.Key, reinitialize));
            return result.ToDictionary(x => x.Key.Name, x => x.Value);
        }

        /// <summary>
        /// Subscribes to all properties that have an <see cref="NotifyParentPropertyAttribute"/> attached.
        /// </summary>
        /// <param name="classObject">The object on which to subscribe to.</param>
        public static void SubscribeAll(object classObject)
        {
            Guard.NotNull(classObject, nameof(classObject));

            var result = GetAttributes(classObject.GetType());
            result.ForEach(x => x.Value.SubscribeEvent(classObject));
        }

        /// <summary>
        /// Unsubscribes from all properties that have an <see cref="NotifyParentPropertyAttribute"/> attached.
        /// </summary>
        /// <param name="classObject">The object on which to unsubscribe from.</param>
        public static void UnsubscribeAll(object classObject)
        {
            Guard.NotNull(classObject, nameof(classObject));

            var result = GetAttributes(classObject.GetType());
            result.ForEach(x => x.Value.UnsubscribeEvent(classObject));
        }

        private class EventCacheItem
        {
            public PropertyChangedEventHandler EventHandler { get; }
            public int SubscribeCount { get; set; }

            public EventCacheItem(PropertyChangedEventHandler handler)
            {
                EventHandler = handler;
                SubscribeCount = 0;
            }
        }
    }
}
