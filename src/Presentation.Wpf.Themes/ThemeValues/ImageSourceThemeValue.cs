using System.Windows.Media;
using MaSch.Core;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    public class ImageSourceThemeValue : ThemeValueBase<ImageSource>
    {
        #region Properties
        [JsonConverter(typeof(ThemeValuePropertyJsonConverter<ImageSource>))]
        public override object RawValue
        {
            get => base.RawValue;
            set => base.RawValue = Guard.OfType(value, nameof(value), typeof(ThemeValueReference), typeof(ImageSource));
        }
        #endregion

        #region Static Members
        public static ImageSourceThemeValue Create(ImageSource value) => CreateInternal(value);
        public static ImageSourceThemeValue Create(ThemeValueReference valueRef) => CreateInternal(valueRef);
        private static ImageSourceThemeValue CreateInternal(object value)
        {
            return new ImageSourceThemeValue
            {
                RawValue = value
            };
        }

        public static implicit operator ImageSource(ImageSourceThemeValue themeValue) => themeValue.Value;
        #endregion
    }
}
