using System;
using System.Linq;
using System.Windows;
using MaSch.Presentation.Wpf.Markup;

namespace MaSch.Presentation.Wpf
{
    public static class ControlLanguageManager
    {
        public static void SetLanguageKeys(Application app, string translationProviderName = null)
        {
            var rd = new ResourceDictionary {Source = new Uri("/MaSch.Presentation.Wpf.Themes;component/Languages/English.xaml") };
            foreach (var key in rd.Keys.OfType<string>())
                SetLanguageKey(app, key, translationProviderName);
        }

        private static void SetLanguageKey(Application app, string key, string translationProviderName)
        {
            var value = new TranslationExtension(key, translationProviderName);
            if (app.Resources.Contains(key))
                app.Resources[key] = value;
            else
                app.Resources.Add(key, value);
        }
    }
}
