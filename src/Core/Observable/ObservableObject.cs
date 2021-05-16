using MaSch.Core.Attributes;
using MaSch.Core.Observable.Modules;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace MaSch.Core.Observable
{
    /// <summary>
    /// Represents an observable class.
    /// </summary>
    /// <seealso cref="IObservableObject" />
    public abstract class ObservableObject : IObservableObject
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc/>
        [XmlIgnore]
        public virtual bool IsNotifyEnabled { get; set; } = true;

        private readonly Dictionary<string, NotifyPropertyChangedAttribute> _attributes;
        private readonly ObservableObjectModule _module;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableObject"/> class.
        /// </summary>
        protected ObservableObject()
        {
            _module = new ObservableObjectModule(this);
            _attributes = NotifyPropertyChangedAttribute.InitializeAll(this);
        }

        /// <inheritdoc/>
        public virtual void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            if (IsNotifyEnabled && _attributes.ContainsKey(propertyName))
            {
                _attributes[propertyName].UnsubscribeEvent(this);
            }

            property = value;

            if (IsNotifyEnabled)
            {
                NotifyPropertyChanged(propertyName);
                if (_attributes.ContainsKey(propertyName))
                    _attributes[propertyName].SubscribeEvent(this);
            }
        }

        /// <inheritdoc/>
        public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "", bool notifyDependencies = true)
        {
            if (!IsNotifyEnabled)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (notifyDependencies)
                _module.NotifyDependentProperties(propertyName);
        }

        /// <inheritdoc/>
        public virtual void NotifyCommandChanged([CallerMemberName] string propertyName = "")
        {
            if (!IsNotifyEnabled)
                return;

            _module.NotifyCommandChanged(propertyName);
        }

        /// <summary>
        /// Gets a value indicating wether the <see cref="IsNotifyEnabled"/> property should be serialized.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="IsNotifyEnabled"/> property should be serialized; otherwise, <c>false</c>.</returns>
        public virtual bool ShouldSerializeIsNotifyEnabled() => false;
    }
}
