namespace MaSch.Presentation.Wpf
{
    /// <summary>
    /// Default implementation of the <see cref="IThemeManagerBindingFactory"/>.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.IThemeManagerBindingFactory" />
    public class ThemeManagerBindingFactory : IThemeManagerBindingFactory
    {
        private readonly IThemeManager _themeManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeManagerBindingFactory"/> class.
        /// </summary>
        /// <param name="themeManager">The theme manager.</param>
        public ThemeManagerBindingFactory(IThemeManager themeManager)
        {
            _themeManager = themeManager;
        }

        /// <inheritdoc/>
        public IThemeManagerBinding this[string key] => new ThemeManagerBinding(_themeManager, key);
    }
}
