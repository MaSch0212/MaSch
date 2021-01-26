using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Win10
{
    public class Win10Icon : Icon
    {
        public Win10IconCode Icon
        {
            get => Character == null ? 0 : Character.GetWin10IconCode();
            set => Character = value.GetChar();
        }

        public Win10Icon()
        {
            Font = Win10IconExtension.FontFamily;
            Type = SymbolType.Character;
        }
        public Win10Icon(Win10IconCode icon) : this() => Icon = icon;
        public Win10Icon(Win10IconCode icon, Stretch stretch) : this(icon) => Stretch = stretch;
        public Win10Icon(Win10IconCode icon, Stretch stretch, double fontSize) : this(icon, stretch) => FontSize = fontSize;

        internal Win10Icon(Win10IconCode icon, Stretch? stretch, double? fontSize) : this(icon)
        {
            if (stretch.HasValue)
                Stretch = stretch.Value;
            if (fontSize.HasValue)
                FontSize = fontSize.Value;
        }
    }
}
