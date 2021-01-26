using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using MaSch.Common;
using MaSch.Common.Extensions;
using MaSch.Common.Observable;
using MaSch.Presentation.Observable.Collections;
using MaSch.Presentation.Wpf.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MaSch.Presentation.Wpf
{
    [JsonConverter(typeof(NoJsonConverter))]
    public class Theme : ObservableObject, ITheme
    {
        private string _name;
        private string _description;
        private ObservableDictionary<string, IThemeValue> _values;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public ObservableDictionary<string, IThemeValue> Values
        {
            get => _values;
            set
            {
                Guard.NotNull(value, nameof(value));

                if (_values != null)
                {
                    _values.CollectionChanged += ValuesOnCollectionChanged;
                    _values.DictionaryItemChanged += ValuesOnDictionaryItemChanged;
                }

                value.CollectionChanged += ValuesOnCollectionChanged;
                value.DictionaryItemChanged += ValuesOnDictionaryItemChanged;
                _values = value;
            }
        }

        public Theme()
        {
            Values = new ObservableDictionary<string, IThemeValue>();
        }

        public void SaveToFile(string filePath) => File.WriteAllText(filePath, ToJson());
        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter());

        private void ValuesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                e.NewItems.Cast<KeyValuePair<string, IThemeValue>>().ForEach(x => ValidateValue(x.Key, x.Value));
        }

        private void ValuesOnDictionaryItemChanged(object sender, DictionaryItemChangedEventArgs<string, IThemeValue> e)
        {
            ValidateValue(e.Key, e.NewValue);
        }

        private void ValidateValue(string key, IThemeValue value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("A theme value key cannot be empty.");
            if (value != null && key != value.Key)
                value.Key = key;
        }

        public static ITheme FromJson(string json, string baseUri)
        {
            json = ThemeJsonConverter.AddBaseUriToJson(json, baseUri);
            var theme = JsonConvert.DeserializeObject<ITheme>(json);
            theme.Values.RemoveWhere(x => x.Value == null);
            return theme;
        }

        public static ITheme FromDefaultTheme(DefaultTheme defaultTheme)
        {
            return FromJson(ThemeJsonConverter.DownloadString(new Uri($"#DefaultThemes/{defaultTheme}", UriKind.RelativeOrAbsolute), null, out var absoluteUri), new Uri(absoluteUri, ".").ToString());
        }

        public static ITheme FromFile(string filePath)
        {
            var uri = new Uri(filePath, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
                uri = new Uri(new Uri(Environment.CurrentDirectory), uri);
            return FromJson(File.ReadAllText(uri.LocalPath), new Uri(uri, ".").ToString());
        }

        public static ITheme FromUri(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                throw new NotSupportedException($"A relative uri is not supported.");
            return FromJson(ThemeJsonConverter.DownloadString(uri, null, out _), new Uri(uri, ".").ToString());
        }

        public static ITheme FromPackUrl(string packUrl)
        {
            var uri = new Uri(packUrl, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
                uri = new Uri(new Uri("pack://application:,,,"), uri);
            if (!string.Equals(uri.Scheme, PackUriHelper.UriSchemePack, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Please provide a pack url (relative or absolute). Scheme was {uri.Scheme}.");
            return FromJson(ThemeJsonConverter.DownloadString(uri, null, out _), new Uri(uri, ".").ToString());
        }
    }
}
