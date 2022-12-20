namespace MaSch.Generators.ObservableObject;

internal static class TypeNames
{
    internal const string INotifyPropertyChanged = "global::System.ComponentModel.INotifyPropertyChanged";
    internal const string IObservableObject = "global::MaSch.Core.Observable.IObservableObject";
    internal const string NotifyPropertyChangedAttribute = "global::MaSch.Core.Attributes.NotifyPropertyChangedAttribute";
    internal const string ObservableObjectModule = "global::MaSch.Core.Observable.Modules.ObservableObjectModule";
    internal const string PropertyChangedEventHandler = "global::System.ComponentModel.PropertyChangedEventHandler";
    internal const string CallerMemberNameAttribute = "global::System.Runtime.CompilerServices.CallerMemberNameAttribute";
    internal const string PropertyChangedEventArgs = "global::System.ComponentModel.PropertyChangedEventArgs";

    internal static string Dictionary(string keyType, string valueType) => $"global::System.Collections.Generic.Dictionary<{keyType}, {valueType}>";
}
