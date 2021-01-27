using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.MaterialDesign
{
    /// <summary>
    /// Represents a icon using the material design icon library.
    /// </summary>
    /// <seealso cref="Wpf.Icon" />
    public class MaterialDesignIcon : Icon
    {
        /// <summary>
        /// The font family to use for the <see cref="MaterialDesignIcon"/> class.
        /// </summary>
        public static readonly FontFamily FontFamily;

        static MaterialDesignIcon()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                FontFamily = new FontFamily("Material Design Icons");
            }
            else
            {
                FontFamily = Application.Current != null
                    ? new FontFamily(new Uri("pack://application:,,,/MaSch.Presentation.Wpf.MaterialDesign;component/"), "./#Material Design Icons")
                    : new FontFamily("Material Design Icons");
            }
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDesignIcon"/> class.
        /// </summary>
        public MaterialDesignIcon()
        {
            Font = FontFamily;
            Type = SymbolType.Character;
            Transform = new ScaleTransform(1.3, 1.3);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDesignIcon"/> class.
        /// </summary>
        /// <param name="icon">The icon to use.</param>
        public MaterialDesignIcon(MaterialDesignIconCode icon)
            : this()
            => Icon = icon;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDesignIcon"/> class.
        /// </summary>
        /// <param name="icon">The icon to use.</param>
        /// <param name="stretch">The stretch mode.</param>
        public MaterialDesignIcon(MaterialDesignIconCode icon, Stretch stretch)
            : this(icon)
            => Stretch = stretch;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDesignIcon"/> class.
        /// </summary>
        /// <param name="icon">The icon to use.</param>
        /// <param name="stretch">The stretch mode.</param>
        /// <param name="fontSize">Size of the font.</param>
        public MaterialDesignIcon(MaterialDesignIconCode icon, Stretch stretch, double fontSize)
            : this(icon, stretch)
            => FontSize = fontSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDesignIcon"/> class.
        /// </summary>
        /// <param name="icon">The icon to use.</param>
        /// <param name="stretch">The stretch mode.</param>
        /// <param name="fontSize">Size of the font.</param>
        internal MaterialDesignIcon(MaterialDesignIconCode icon, Stretch? stretch, double? fontSize)
            : this(icon)
        {
            if (stretch.HasValue)
                Stretch = stretch.Value;
            if (fontSize.HasValue)
                FontSize = fontSize.Value;
        }
    }
}
