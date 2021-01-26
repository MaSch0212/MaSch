using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace MaSch.Core.Observable.Modules
{
    public interface IDataErrorHandler : INotifyDataErrorInfo, IObservableObjectModule
    {
        IDictionary<string, IEnumerable> GetErrors(); 
        bool CheckForErrors();
        bool CheckForError(string? propertyName);
        bool IsPropertyExistant(string? propertyName);
    }

    public interface IDataErrorObject : INotifyDataErrorInfo
    {
        bool CheckForErrors();
        bool CheckForError(string? propertyName);
        IDictionary<string, IEnumerable> GetErrors();
    }
}
