using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaSch.Presentation.Wpf.JsonConverters;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf
{
    [JsonConverter(typeof(ThemeValueJsonConverter))]
    public interface IThemeValue : INotifyPropertyChanged, ICloneable
    {
        IThemeManager ThemeManager { get; set; }
        string Key { get; set; }
        object RawValue { get; set; }
        object ValueBase { get; set; }

        TValue GetPropertyValue<TValue>(string propertyName);

        object this[string propertyName] { get; set; }
    }

    public interface IThemeValue<T> : IThemeValue
    {
        T Value { get; set; }
    }
}
