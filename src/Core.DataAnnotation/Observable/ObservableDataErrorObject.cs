﻿using MaSch.Core.Observable.Modules;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MaSch.Core.Observable;

/// <summary>
/// Represents an observable class that also handles errors.
/// </summary>
/// <seealso cref="ObservableObject" />
/// <seealso cref="IDataErrorObject" />
[RequiresUnreferencedCode("Uses DataErrorHandler.")]
public class ObservableDataErrorObject : ObservableObject, IDataErrorObject
{
    private readonly DataErrorHandler _dataErrorHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableDataErrorObject"/> class.
    /// </summary>
    public ObservableDataErrorObject()
    {
        _dataErrorHandler = new DataErrorHandler(this);
        _dataErrorHandler.ErrorsChanged += (s, e) => NotifyPropertyChanged(nameof(HasErrors));
    }

    /// <inheritdoc />
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
    {
        add { _dataErrorHandler.ErrorsChanged += value; }
        remove { _dataErrorHandler.ErrorsChanged -= value; }
    }

    /// <inheritdoc />
    [XmlIgnore]
    public virtual bool HasErrors => _dataErrorHandler.HasErrors;

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
