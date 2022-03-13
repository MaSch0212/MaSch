using MaSch.Core;
using MaSch.Core.Observable;
using MaSch.Core.Observable.Collections;
using MaSch.Presentation.Wpf.JsonConverters;
using Newtonsoft.Json.Converters;
using System.Collections.Specialized;
using System.IO.Packaging;

namespace MaSch.Presentation.Wpf;

/// <summary>
/// Default implementation of the <see cref="ITheme"/> interface.
/// </summary>
/// <seealso cref="ObservableObject" />
/// <seealso cref="ITheme" />
[JsonConverter(typeof(NoJsonConverter))]
public class Theme : ObservableObject, ITheme
{
    private string? _name;
    private string? _description;
    private ObservableDictionary<string, IThemeValue> _values = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="Theme"/> class.
    /// </summary>
    public Theme()
    {
        Values = new ObservableDictionary<string, IThemeValue>();
    }

    /// <inheritdoc/>
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    /// <inheritdoc/>
    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    /// <inheritdoc/>
    public ObservableDictionary<string, IThemeValue> Values
    {
        get => _values;
        set
        {
            _ = Guard.NotNull(value);

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

    /// <summary>
    /// Creates a <see cref="ITheme"/> from a json document.
    /// </summary>
    /// <param name="json">The json.</param>
    /// <param name="baseUri">The base URI.</param>
    /// <returns>The created <see cref="ITheme"/>.</returns>
    public static ITheme FromJson(string json, string baseUri)
    {
        json = ThemeJsonConverter.AddBaseUriToJson(json, baseUri);
        var theme = JsonConvert.DeserializeObject<ITheme>(json);
        _ = theme.Values.RemoveWhere(x => x.Value == null);
        return theme;
    }

    /// <summary>
    /// Creates a <see cref="ITheme"/> from a default theme.
    /// </summary>
    /// <param name="defaultTheme">The default theme.</param>
    /// <returns>The created <see cref="ITheme"/>.</returns>
    public static ITheme FromDefaultTheme(DefaultTheme defaultTheme)
    {
        return FromJson(ThemeJsonConverter.DownloadString(new Uri($"#DefaultThemes/{defaultTheme}", UriKind.RelativeOrAbsolute), null, out var absoluteUri), new Uri(absoluteUri, ".").ToString());
    }

    /// <summary>
    /// Creates a <see cref="ITheme"/> from a json file.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <returns>The created <see cref="ITheme"/>.</returns>
    public static ITheme FromFile(string filePath)
    {
        var uri = new Uri(filePath, UriKind.RelativeOrAbsolute);
        if (!uri.IsAbsoluteUri)
            uri = new Uri(new Uri(Environment.CurrentDirectory), uri);
        return FromJson(File.ReadAllText(uri.LocalPath), new Uri(uri, ".").ToString());
    }

    /// <summary>
    /// Creates a <see cref="ITheme"/> from an url. Supported schemes are: http, https, file and pack.
    /// </summary>
    /// <param name="uri">The URI.</param>
    /// <returns>The created <see cref="ITheme"/>.</returns>
    /// <exception cref="NotSupportedException">A relative uri is not supported.</exception>
    public static ITheme FromUri(Uri uri)
    {
        if (!uri.IsAbsoluteUri)
            throw new NotSupportedException($"A relative uri is not supported.");
        return FromJson(ThemeJsonConverter.DownloadString(uri, null, out _), new Uri(uri, ".").ToString());
    }

    /// <summary>
    /// Creates a <see cref="ITheme"/> from a pack url.
    /// </summary>
    /// <param name="packUrl">The pack URL.</param>
    /// <returns>The created <see cref="ITheme"/>.</returns>
    /// <exception cref="InvalidOperationException">Please provide a pack url (relative or absolute). Scheme was {uri.Scheme}.</exception>
    [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Pack Uris always start with the \"pack://application:,,,\" prefix.")]
    public static ITheme FromPackUrl(string packUrl)
    {
        var uri = new Uri(packUrl, UriKind.RelativeOrAbsolute);
        if (!uri.IsAbsoluteUri)
            uri = new Uri(new Uri("pack://application:,,,"), uri);
        if (!string.Equals(uri.Scheme, PackUriHelper.UriSchemePack, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"Please provide a pack url (relative or absolute). Scheme was {uri.Scheme}.");
        return FromJson(ThemeJsonConverter.DownloadString(uri, null, out _), new Uri(uri, ".").ToString());
    }

    /// <inheritdoc/>
    public void SaveToFile(string filePath)
    {
        File.WriteAllText(filePath, ToJson());
    }

    /// <inheritdoc/>
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter());
    }

    private static void ValidateValue(string key, IThemeValue? value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException("A theme value key cannot be empty.");
        if (value != null && key != value.Key)
            value.Key = key;
    }

    private void ValuesOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            e.NewItems.Cast<KeyValuePair<string, IThemeValue>>().ForEach(x => ValidateValue(x.Key, x.Value));
    }

    private void ValuesOnDictionaryItemChanged(object sender, DictionaryItemChangedEventArgs<string, IThemeValue> e)
    {
        ValidateValue(e.Key, e.NewValue);
    }
}
