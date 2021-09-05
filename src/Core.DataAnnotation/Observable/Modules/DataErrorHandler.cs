using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MaSch.Core.Observable.Modules
{
    /// <summary>
    /// Handler for data errors in classes that implement the <see cref="IDataErrorObject"/> interface.
    /// </summary>
    /// <seealso cref="IDataErrorHandler" />
    public class DataErrorHandler : IDataErrorHandler
    {
        private readonly ICollection<PropertyInfo> _properties;
        private readonly IDictionary<string, IEnumerable<ValidationResult>> _errors;
        private readonly object _dataErrorObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataErrorHandler"/> class.
        /// </summary>
        /// <param name="dataErrorObject">The data error object.</param>
        public DataErrorHandler(object dataErrorObject)
        {
            _errors = new Dictionary<string, IEnumerable<ValidationResult>>();
            _properties = dataErrorObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _dataErrorObject = dataErrorObject;
        }

        /// <inheritdoc />
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <inheritdoc />
        public bool HasErrors => _errors.Count > 0;

        /// <inheritdoc />
        public IDictionary<string, IEnumerable> GetErrors()
        {
            return _errors.ToDictionary(x => x.Key, x => (IEnumerable)x.Value.ToList());
        }

        /// <inheritdoc />
        public IEnumerable GetErrors(string? propertyName)
        {
            return propertyName != null && _errors.ContainsKey(propertyName) ? _errors[propertyName] : Enumerable.Empty<ValidationResult>();
        }

        /// <inheritdoc />
        public void OnSetValue(object? value, string propertyName = "")
        {
            var property = _properties.FirstOrDefault(x => x.Name == propertyName);
            if (property == null)
                throw new ArgumentException($"A public property with the name \"{propertyName}\" could not be found", nameof(propertyName));
            _ = CheckForError(property, value);
        }

        /// <inheritdoc />
        public bool CheckForErrors()
        {
            foreach (var propertyInfo in _properties)
            {
                _ = CheckForError(propertyInfo, propertyInfo.GetValue(_dataErrorObject));
            }

            return HasErrors;
        }

        /// <inheritdoc />
        public bool CheckForError(string? propertyName)
        {
            var property = _properties.FirstOrDefault(x => x.Name == propertyName);
            if (property == null)
                throw new ArgumentException($"A public property with the name \"{propertyName}\" could not be found", nameof(propertyName));
            return CheckForError(property, property.GetValue(_dataErrorObject));
        }

        /// <inheritdoc />
        public bool IsPropertyExistant(string? propertyName)
        {
            return _properties.Any(x => x.Name == propertyName);
        }

        private bool CheckForError(PropertyInfo property, object? value)
        {
            var results = new List<ValidationResult>();
            var isValid = true;

            if (property.GetCustomAttributes<ValidationAttribute>(true).Any())
            {
                isValid = Validator.TryValidateProperty(value, new ValidationContext(_dataErrorObject, null, null) { MemberName = property.Name }, results);
            }

            var wasValid = !_errors.ContainsKey(property.Name);
            var notify = true;
            if (isValid && !wasValid)
                _ = _errors.Remove(property.Name);
            else if (!isValid && wasValid)
                _errors.Add(property.Name, results);
            else if (!isValid)
                _errors[property.Name] = results;
            else
                notify = false;
            if (notify)
                ErrorsChanged?.Invoke(_dataErrorObject, new DataErrorsChangedEventArgs(property.Name));
            return isValid;
        }
    }
}
