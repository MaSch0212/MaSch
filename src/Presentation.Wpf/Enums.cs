#pragma warning disable SA1649 // File name should match first type name

namespace MaSch.Presentation.Wpf
{
    /// <summary>
    /// Specifies the type of rendering to use for an <see cref="IIcon"/>.
    /// </summary>
    public enum SymbolType
    {
        /// <summary>
        /// The <see cref="IIcon"/> is rendered using a <see cref="System.Windows.Media.Geometry"/> object.
        /// </summary>
        Geometry,

        /// <summary>
        /// The <see cref="IIcon"/> is rendered using a character inside a font.
        /// </summary>
        Character,
    }
}
