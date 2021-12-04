using MaSch.Core.Observable;

namespace MaSch.Presentation.Wpf;

/// <summary>
/// Default implementation of the <see cref="IThemeManagerBinding"/>.
/// </summary>
/// <seealso cref="ObservableObject" />
/// <seealso cref="IThemeManagerBinding" />
public class ThemeManagerBinding : ObservableObject, IThemeManagerBinding
{
    private readonly IThemeManager _themeManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeManagerBinding"/> class.
    /// </summary>
    /// <param name="themeManager">The theme manager.</param>
    /// <param name="key">The key.</param>
    public ThemeManagerBinding(IThemeManager themeManager, string key)
    {
        _themeManager = themeManager;
        Key = key;

        _themeManager.ThemeValueChanged += ThemeManagerOnThemeValueChanged;
    }

    /// <inheritdoc/>
    public string Key { get; }

    /// <inheritdoc/>
    public IThemeValue? Value => _themeManager.GetValue(Key);

    private void ThemeManagerOnThemeValueChanged(object? sender, ThemeValueChangedEventArgs e)
    {
        if (e.HasChangeForKey(Key))
            NotifyPropertyChanged(nameof(Value));
    }
}
