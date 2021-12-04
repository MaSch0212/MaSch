using MaSch.Presentation.Wpf.Markup;
using System.Windows;

namespace MaSch.Presentation.Wpf;

/// <summary>
/// Class that manages the translations for the various controls in MaSch.Presentation.Wpf.
/// </summary>
public static class ControlLanguageManager
{
    /// <summary>
    /// Sets the language keys to a translation manager.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="translationProviderName">Name of the translation provider.</param>
    public static void SetLanguageKeys(Application app, string? translationProviderName = null)
    {
        var rd = new ResourceDictionary
        {
            Source = new Uri("/MaSch.Presentation.Wpf.Themes;component/Languages/English.xaml"),
        };

        foreach (var key in rd.Keys.OfType<string>())
            SetLanguageKey(app, key, translationProviderName);
    }

    private static void SetLanguageKey(Application app, string key, string? translationProviderName)
    {
        var value = new TranslationExtension(key, translationProviderName);
        if (app.Resources.Contains(key))
            app.Resources[key] = value;
        else
            app.Resources.Add(key, value);
    }
}
