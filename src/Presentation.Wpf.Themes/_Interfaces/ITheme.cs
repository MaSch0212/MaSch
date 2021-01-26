using MaSch.Core.Observable.Collections;
using MaSch.Presentation.Wpf.JsonConverters;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf
{
    [JsonConverter(typeof(ThemeJsonConverter))]
    public interface ITheme
    {
        string Name { get; set; }
        string Description { get; set; }
        ObservableDictionary<string, IThemeValue> Values { get; }

        void SaveToFile(string filePath);
        string ToJson();
    }
}
