﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using MaSch.Common.Observable.Modules;

namespace MaSch.Presentation.Wpf.Observable
{
    public class ObservableChangeTrackingDependencyObject : ObservableDependencyObject, IChangeTrackedObject
    {
        #region Properties

        [XmlIgnore]
        public virtual IChangeTracker ChangeTracker { get; private set; }
        [XmlIgnore]
        public bool HasChanges => ChangeTracker.HasChanges;
        [XmlIgnore]
        public virtual bool ImplicitlyRecurse => true;

        #endregion

        #region Ctor

        public ObservableChangeTrackingDependencyObject()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            ChangeTracker = new ChangeTracker(GetType(), ImplicitlyRecurse);
        }

        protected ObservableChangeTrackingDependencyObject(IChangeTracker changeTracker)
        {
            ChangeTracker = changeTracker ?? throw new ArgumentNullException(nameof(changeTracker));
        }

        #endregion

        #region Public Methods

        public void TrackChange<T>(T value, bool notifyChange = true, [CallerMemberName] string propertyName = "")
        {
            ChangeTracker.OnSetValue(value, propertyName);
            NotifyPropertyChanged(propertyName);
        }

        public override void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            ChangeTracker.OnSetValue(value, propertyName);
            base.SetProperty(ref property, value, propertyName);
        }

        public virtual void ResetChangeTracking()
        {
            foreach (var property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = property.GetValue(this);
                ChangeTracker.SetBaseValue(value, property.Name);
            }
            ChangeTracker.ResetChangeTracking();
        }

        #endregion

        #region Serialization
        public virtual bool ShouldSerializeChangeTracker() => false;
        public virtual bool ShouldSerializeHasChanges() => false;
        public virtual bool ShouldSerializeImplicitlyRecurse() => false;
        #endregion
    }
}
