using System.Windows.Media;

namespace MaSch.Presentation.Wpf
{
    public interface IIcon
    {
        SymbolType Type { get; set; }

        // SymbolType Character -->
        string Character { get; set; }
        FontFamily Font { get; set; }
        double FontSize { get; set; }

        // SymbolType Geometry -->
        Geometry Geometry { get; set; }
        bool IsGeometryFilled { get; set; }
        double GeometryStrokeThickness { get; set; }

        // Common -->
        Stretch Stretch { get; set; }
        Transform Transform { get; set; }
    }
}
