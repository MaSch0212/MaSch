using System;

namespace MaSch.Generators.ObservableObject.Models;

[Flags]
internal enum InterfaceType
{
    None = 0,
    ObservableObject = 1,
    NotifyPropertyChanged = 2,
}
