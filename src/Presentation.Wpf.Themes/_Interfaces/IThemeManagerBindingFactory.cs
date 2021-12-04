namespace MaSch.Presentation.Wpf;

/// <summary>
/// Represents a factory that creates <see cref="IThemeManagerBinding"/>s.
/// </summary>
public interface IThemeManagerBindingFactory
{
    /// <summary>
    /// Creates an <see cref="IThemeManagerBinding"/> for the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>A binding to a theme value with the specified key.</returns>
    IThemeManagerBinding this[string key] { get; }
}
