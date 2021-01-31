using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;
using MaSch.Core.Attributes;
using MaSch.Core.Observable;
using MaSch.Core.Observable.Modules;
using MaSch.Presentation.Wpf.Attributes;

namespace MaSch.Presentation.Wpf.Observable
{
    /// <summary>
    /// Represents an observable dependency object.
    /// </summary>
    /// <seealso cref="System.Windows.DependencyObject" />
    /// <seealso cref="MaSch.Core.Observable.IObservableObject" />
    public class ObservableDependencyObject : DependencyObject, IObservableObject
    {
        private readonly Dictionary<string, NotifyPropertyChangedAttribute> _attributes;
        private readonly List<(string propertyName, NotifyDependencyPropertyChangedAttribute attribute)> _dependencyPropertyAttributes;
        private readonly ObservableObjectModule _module;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when a dependency property changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler DependencyPropertyChanged;

        /// <summary>
        /// Gets a value indicating whether this instance is in design mode.
        /// </summary>
        protected bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());

        /// <inheritdoc/>
        [XmlIgnore]
        public virtual bool IsNotifyEnabled { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDependencyObject"/> class.
        /// </summary>
        public ObservableDependencyObject()
        {
            _module = new ObservableObjectModule(this);
            _attributes = NotifyPropertyChangedAttribute.InitializeAll(this);
            _dependencyPropertyAttributes = NotifyDependencyPropertyChangedAttribute.GetAttributes(this);
        }

        /// <inheritdoc/>
        public virtual void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            if (_attributes.ContainsKey(propertyName))
                _attributes[propertyName].UnsubscribeEvent(this);
            property = value;
            NotifyPropertyChanged(propertyName);
            if (_attributes.ContainsKey(propertyName))
                _attributes[propertyName].SubscribeEvent(this);
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

        /// <inheritdoc/>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            DependencyPropertyChanged?.Invoke(this, e);

            var propertiesToNotify = from x in _dependencyPropertyAttributes
                                     where x.attribute.PropertyName == e.Property.Name && (x.attribute.OwnerType ?? GetType()) == e.Property.OwnerType
                                     select x.propertyName;
            foreach (var p in propertiesToNotify)
                NotifyPropertyChanged(p);
        }

        /// <summary>
        /// Gets a value indicating wether the <see cref="IsNotifyEnabled"/> property should be serialized.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="IsNotifyEnabled"/> property should be serialized; otherwise, <c>false</c>.</returns>
        public virtual bool ShouldSerializeIsNotifyEnabled() => false;
    }
}
