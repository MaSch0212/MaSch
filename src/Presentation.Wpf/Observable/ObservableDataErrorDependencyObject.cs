using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using MaSch.Core.Observable.Modules;

namespace MaSch.Presentation.Wpf.Observable
{
    /// <summary>
    /// Represents an observable dependency object that also handles errors.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.Observable.ObservableDependencyObject" />
    /// <seealso cref="System.ComponentModel.INotifyDataErrorInfo" />
    public class ObservableDataErrorDependencyObject : ObservableDependencyObject, IDataErrorObject
    {
        private readonly DataErrorHandler _dataErrorHandler;

        /// <inheritdoc />
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add => _dataErrorHandler.ErrorsChanged += value;
            remove => _dataErrorHandler.ErrorsChanged -= value;
        }

        /// <inheritdoc />
        [XmlIgnore]
        public bool HasErrors => _dataErrorHandler.HasErrors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDataErrorDependencyObject"/> class.
        /// </summary>
        public ObservableDataErrorDependencyObject()
        {
            _dataErrorHandler = new DataErrorHandler(this);
            _dataErrorHandler.ErrorsChanged += (s, e) => NotifyPropertyChanged(nameof(HasErrors));
        }

        /// <inheritdoc />
        public override void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            _dataErrorHandler.OnSetValue(value, propertyName);
            base.SetProperty(ref property, value, propertyName);
        }

        /// <inheritdoc />
        public override void NotifyPropertyChanged([CallerMemberName] string propertyName = "", bool notifyDependencies = true)
        {
            base.NotifyPropertyChanged(propertyName, notifyDependencies);
            if (_dataErrorHandler.IsPropertyExistant(propertyName))
                _dataErrorHandler.CheckForError(propertyName);
        }

        /// <inheritdoc />
        public IDictionary<string, IEnumerable> GetErrors() => _dataErrorHandler.GetErrors();

        /// <inheritdoc />
        public IEnumerable GetErrors(string propertyName) => _dataErrorHandler.GetErrors(propertyName);

        /// <inheritdoc />
        public bool CheckForErrors() => _dataErrorHandler.CheckForErrors();

        /// <inheritdoc />
        public bool CheckForError(string propertyName) => _dataErrorHandler.CheckForError(propertyName);

        /// <summary>
        /// Gets a value indicating wether the <see cref="HasErrors"/> property should be serialized.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="HasErrors"/> property should be serialized; otherwise, <c>false</c>.</returns>
        public virtual bool ShouldSerializeHasErrors() => false;
    }
}
