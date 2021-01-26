using MaSch.Core.Observable;

namespace MaSch.Presentation.Wpf
{
    public class ThemeManagerBinding : ObservableObject, IThemeManagerBinding
    {
        private readonly IThemeManager _themeManager;

        public string Key { get; }
        public IThemeValue Value => _themeManager.GetValue(Key);

        public ThemeManagerBinding(IThemeManager themeManager, string key)
        {
            _themeManager = themeManager;
            Key = key;

            _themeManager.ThemeValueChanged += ThemeManagerOnThemeValueChanged;
        }

        private void ThemeManagerOnThemeValueChanged(object sender, ThemeValueChangedEventArgs e)
        {
            if (e.HasChangeForKey(Key))
                NotifyPropertyChanged(nameof(Value));
        }
    }
}
