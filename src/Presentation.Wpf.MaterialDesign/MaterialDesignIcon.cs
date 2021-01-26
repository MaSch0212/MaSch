using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.MaterialDesign
{
    public class MaterialDesignIcon : Icon
    {
        public static readonly FontFamily FontFamily;

        static MaterialDesignIcon()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                FontFamily = new FontFamily("Material Design Icons");
            else
            {
                FontFamily = Application.Current != null
                    ? new FontFamily(new Uri("pack://application:,,,/MaSch.Presentation.Wpf.MaterialDesign;component/"), "./#Material Design Icons")
                    : new FontFamily("Material Design Icons");
            }
        }

        public MaterialDesignIconCode Icon
        {
            get => Character == null ? 0 : Character.GetMaterialDesignIconCode();
            set
            {
                if (value.IsGeometry(out var geom))
                {
                    Character = null;
                    Geometry = Geometry.Parse(geom);
                    Type = SymbolType.Geometry;
                }
                else
                {
                    Character = value.GetChar();
                    Geometry = null;
                    Type = SymbolType.Character;
                }
            }
        }

        public MaterialDesignIcon()
        {
            Font = FontFamily;
            Type = SymbolType.Character;
            Transform = new ScaleTransform(1.3, 1.3);
        }
        public MaterialDesignIcon(MaterialDesignIconCode icon) : this() => Icon = icon;
        public MaterialDesignIcon(MaterialDesignIconCode icon, Stretch stretch) : this(icon) => Stretch = stretch;
        public MaterialDesignIcon(MaterialDesignIconCode icon, Stretch stretch, double fontSize) : this(icon, stretch) => FontSize = fontSize;

        internal MaterialDesignIcon(MaterialDesignIconCode icon, Stretch? stretch, double? fontSize) : this(icon)
        {
            if (stretch.HasValue)
                Stretch = stretch.Value;
            if (fontSize.HasValue)
                FontSize = fontSize.Value;
        }
    }
}
