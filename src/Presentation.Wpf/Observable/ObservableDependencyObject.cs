using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
    public class ObservableDependencyObject : DependencyObject, IObservableObject
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event DependencyPropertyChangedEventHandler DependencyPropertyChanged;

        private Dictionary<string, NotifyPropertyChangedAttribute> _attributes;
        private List<(string propertyName, NotifyDependencyPropertyChangedAttribute attribute)> _dependencyPropertyAttributes;
        private ObservableObjectModule _module;
        protected bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());

        [XmlIgnore]
        public virtual bool IsNotifyEnabled { get; set; } = true;

        public ObservableDependencyObject()
        {
            _module = new ObservableObjectModule(this);
            _attributes = NotifyPropertyChangedAttribute.InitializeAll(this);
            _dependencyPropertyAttributes = NotifyDependencyPropertyChangedAttribute.GetAttributes(this);
        }

        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public virtual void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            if (_attributes.ContainsKey(propertyName))
                _attributes[propertyName].UnsubscribeEvent(this);
            property = value;
            NotifyPropertyChanged(propertyName);
            if (_attributes.ContainsKey(propertyName))
                _attributes[propertyName].SubscribeEvent(this);
        }

        public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "", bool notifyDependencies = true)
        {
            if (!IsNotifyEnabled)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (notifyDependencies)
                _module.NotifyDependentProperties(propertyName);
        }

        public virtual void NotifyCommandChanged([CallerMemberName] string propertyName = "")
        {
            if (!IsNotifyEnabled)
                return;

            _module.NotifyCommandChanged(propertyName);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            var propertiesToNotify = from x in _dependencyPropertyAttributes
                                     where x.attribute.PropertyName == e.Property.Name && (x.attribute.OwnerType ?? GetType()) == e.Property.OwnerType
                                     select x.propertyName;
            foreach (var p in propertiesToNotify)
                DependencyPropertyChanged?.Invoke(this, e);
        }


        public virtual bool ShouldSerializeIsNotifyEnabled() => false;
    }
}
