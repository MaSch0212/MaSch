using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MaSch.Common.Observable.Modules
{
    public class DataErrorHandler : IDataErrorHandler
    {
        #region Fields

        private readonly ICollection<PropertyInfo> _properties;
        private readonly IDictionary<string, IEnumerable<ValidationResult>> _errors;
        private readonly object _dataErrorObject;

        #endregion

        #region Properties
        
        public bool HasErrors => _errors.Count > 0;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        #endregion

        #region Ctor

        public DataErrorHandler(object dataErrorObject)
        {
            _errors = new Dictionary<string, IEnumerable<ValidationResult>>();
            _properties = dataErrorObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _dataErrorObject = dataErrorObject;
        }

        #endregion

        #region Public Methods

        public IDictionary<string, IEnumerable> GetErrors()
        {
            return _errors.ToDictionary(x => x.Key, x => (IEnumerable)x.Value.ToList());
        } 

        public IEnumerable GetErrors(string? propertyName)
        {
            return propertyName != null && _errors.ContainsKey(propertyName) ? _errors[propertyName] : Enumerable.Empty<ValidationResult>();
        }

        public void OnSetValue(object? value, string propertyName = "")
        {
            var property = _properties.FirstOrDefault(x => x.Name == propertyName);
            if (property == null) throw new ArgumentException($"A public property with the name \"{propertyName}\" could not be found", nameof(propertyName));
            CheckForError(property, value);
        }

        public bool CheckForErrors()
        {
            foreach (var propertyInfo in _properties)
            {
                CheckForError(propertyInfo, propertyInfo.GetValue(_dataErrorObject));
            }
            return HasErrors;
        }

        public bool CheckForError(string? propertyName)
        {
            var property = _properties.FirstOrDefault(x => x.Name == propertyName);
            if (property == null) throw new ArgumentException($"A public property with the name \"{propertyName}\" could not be found", nameof(propertyName));
            return CheckForError(property, property.GetValue(_dataErrorObject));
        }

        public bool IsPropertyExistant(string? propertyName) => _properties.Any(x => x.Name == propertyName);

        #endregion

        #region Private Methods

        private bool CheckForError(PropertyInfo property, object? value)
        {
            var results = new List<ValidationResult>();
            var isValid = true;

            if (property.GetCustomAttributes<ValidationAttribute>(true).Any())
            {
                isValid = Validator.TryValidateProperty(value,
                    new ValidationContext(_dataErrorObject, null, null) { MemberName = property.Name }, results);
            }

            var wasValid = !_errors.ContainsKey(property.Name);
            var notify = true;
            if (isValid && !wasValid)
                _errors.Remove(property.Name);
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

        #endregion
    }
}
