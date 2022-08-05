using MaSch.Core;
using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

[assembly: XmlnsDefinition("http://schemas.masch212.de/MaSch/Wpf/Controls", "MaSch.Presentation.Wpf.Controls")]
[assembly: XmlnsDefinition("http://schemas.masch212.de/MaSch/Wpf", "MaSch.Presentation.Wpf.Markup")]
[assembly: XmlnsDefinition("http://schemas.masch212.de/MaSch/Wpf", "MaSch.Presentation.Wpf.DependencyProperties")]
[assembly: XmlnsDefinition("http://schemas.masch212.de/MaSch/Wpf", "MaSch.Presentation.Wpf.Models")]

[assembly: Shims(Shims.NullableReferenceTypes)]