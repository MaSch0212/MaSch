using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Win10
{
    /// <summary>
    /// Represents a icon using the Windows 10 MDL2 Assets icon font.
    /// </summary>
    /// <seealso cref="Wpf.Icon" />
    public class Win10Icon : Icon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Win10Icon"/> class.
        /// </summary>
        public Win10Icon()
        {
            Font = Win10IconExtension.FontFamily;
            Type = SymbolType.Character;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Win10Icon"/> class.
        /// </summary>
        /// <param name="icon">The icon to use.</param>
        public Win10Icon(Win10IconCode icon)
            : this()
        {
            Icon = icon;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Win10Icon"/> class.
        /// </summary>
        /// <param name="icon">The icon to use.</param>
        /// <param name="stretch">The stretch mode.</param>
        public Win10Icon(Win10IconCode icon, Stretch stretch)
            : this(icon)
        {
            Stretch = stretch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Win10Icon"/> class.
        /// </summary>
        /// <param name="icon">The icon to use.</param>
        /// <param name="stretch">The stretch mode.</param>
        /// <param name="fontSize">Size of the font.</param>
        public Win10Icon(Win10IconCode icon, Stretch stretch, double fontSize)
            : this(icon, stretch)
        {
            FontSize = fontSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Win10Icon"/> class.
        /// </summary>
        /// <param name="icon">The icon to use.</param>
        /// <param name="stretch">The stretch mode.</param>
        /// <param name="fontSize">Size of the font.</param>
        internal Win10Icon(Win10IconCode icon, Stretch? stretch, double? fontSize)
            : this(icon)
        {
            if (stretch.HasValue)
                Stretch = stretch.Value;
            if (fontSize.HasValue)
                FontSize = fontSize.Value;
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public Win10IconCode Icon
        {
            get => Character == null ? 0 : Character.GetWin10IconCode();
            set => Character = value.GetChar();
        }
    }
}
