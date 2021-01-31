using MaSch.Core.Attributes;
using MaSch.Core.Observable;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.ViewModels.MessageBox
{
    [ObservablePropertyDefinition]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Property definition interface should be first in file.")]
    internal interface IBrushGeometry_Props
    {
        Brush FillBrush { get; set; }
        Brush StrokeBrush { get; set; }
        Icon Icon { get; set; }
    }

    public partial class BrushGeometry : ObservableObject, IBrushGeometry_Props
    {
        public BrushGeometry()
        {
            _fillBrush = new SolidColorBrush(Colors.Black);
            _strokeBrush = new SolidColorBrush(Colors.Transparent);
        }
    }
}
