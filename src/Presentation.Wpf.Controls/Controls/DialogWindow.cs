using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MaSch.Core.Attributes;
using MaSch.Core.Observable;
using MaSch.Core.Observable.Modules;

namespace MaSch.Presentation.Wpf.Controls
{
    public class DialogWindow : Window, IObservableObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool IsNotifyEnabled { get; set; } = true;

        private readonly Dictionary<string, NotifyPropertyChangedAttribute> _attributes;
        private readonly ObservableObjectModule _module;

        static DialogWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogWindow), new FrameworkPropertyMetadata(typeof(DialogWindow)));
        }

        public DialogWindow()
        {
            _module = new ObservableObjectModule(this);
            _attributes = NotifyPropertyChangedAttribute.InitializeAll(this);
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
    }
}
