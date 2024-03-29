﻿using MaSch.Core.Observable.Modules;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MaSch.Presentation.Wpf.Observable;

/// <summary>
/// Represents an observable dependency object that also handles errors.
/// </summary>
/// <seealso cref="ObservableDependencyObject" />
/// <seealso cref="INotifyDataErrorInfo" />
public class ObservableDataErrorDependencyObject : ObservableDependencyObject, IDataErrorObject
{
    private readonly DataErrorHandler _dataErrorHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableDataErrorDependencyObject"/> class.
    /// </summary>
    public ObservableDataErrorDependencyObject()
    {
        _dataErrorHandler = new DataErrorHandler(this);
        _dataErrorHandler.ErrorsChanged += (s, e) => NotifyPropertyChanged(nameof(HasErrors));
    }

    /// <inheritdoc />
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
    {
        add => _dataErrorHandler.ErrorsChanged += value;
        remove => _dataErrorHandler.ErrorsChanged -= value;
    }

    /// <inheritdoc />
    [XmlIgnore]
    public bool HasErrors => _dataErrorHandler.HasErrors;

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
            _ = _dataErrorHandler.CheckForError(propertyName);
    }

    /// <inheritdoc />
    public IDictionary<string, IEnumerable> GetErrors()
    {
        return _dataErrorHandler.GetErrors();
    }

    /// <inheritdoc />
    public IEnumerable GetErrors(string? propertyName)
    {
        return _dataErrorHandler.GetErrors(propertyName);
    }

    /// <inheritdoc />
    public bool CheckForErrors()
    {
        return _dataErrorHandler.CheckForErrors();
    }

    /// <inheritdoc />
    public bool CheckForError(string? propertyName)
    {
        return _dataErrorHandler.CheckForError(propertyName);
    }

    /// <summary>
    /// Gets a value indicating wether the <see cref="HasErrors"/> property should be serialized.
    /// </summary>
    /// <returns><c>true</c> if the <see cref="HasErrors"/> property should be serialized; otherwise, <c>false</c>.</returns>
    public virtual bool ShouldSerializeHasErrors()
    {
        return false;
    }
}
