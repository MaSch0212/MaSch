using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using MaSch.Common.Observable.Modules;

namespace MaSch.Presentation.Wpf.Observable
{
    public class ObservableDataErrorDependencyObject : ObservableDependencyObject, INotifyDataErrorInfo
    {
        #region Fields

        private readonly DataErrorHandler _dataErrorHandler;

        #endregion

        #region Properties

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add => _dataErrorHandler.ErrorsChanged += value;
            remove => _dataErrorHandler.ErrorsChanged -= value;
        }

        [XmlIgnore]
        public bool HasErrors => _dataErrorHandler.HasErrors;

        #endregion

        #region Ctor

        public ObservableDataErrorDependencyObject()
        {
            _dataErrorHandler = new DataErrorHandler(this);
            _dataErrorHandler.ErrorsChanged += (s, e) => NotifyPropertyChanged(nameof(HasErrors));
        }

        #endregion

        #region Overrides

        public override void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            _dataErrorHandler.OnSetValue(value, propertyName);
            base.SetProperty(ref property, value, propertyName);
        }

        public override void NotifyPropertyChanged([CallerMemberName] string propertyName = "", bool notifyDependencies = true)
        {
            base.NotifyPropertyChanged(propertyName, notifyDependencies);
            if (_dataErrorHandler.IsPropertyExistant(propertyName))
                _dataErrorHandler.CheckForError(propertyName);
        }

        #endregion

        #region Public Methods

        public IDictionary<string, IEnumerable> GetErrors() => _dataErrorHandler.GetErrors();

        public IEnumerable GetErrors(string propertyName) => _dataErrorHandler.GetErrors(propertyName);

        public bool CheckForErrors() => _dataErrorHandler.CheckForErrors();

        public bool CheckForError(string propertyName) => _dataErrorHandler.CheckForError(propertyName);

        #endregion

        #region Serialization
        public virtual bool ShouldSerializeHasErrors() => false;
        #endregion
    }
}
