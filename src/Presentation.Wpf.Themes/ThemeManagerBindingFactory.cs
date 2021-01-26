using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.Presentation.Wpf
{
    public class ThemeManagerBindingFactory : IThemeManagerBindingFactory
    {
        private readonly IThemeManager _themeManager;

        public ThemeManagerBindingFactory(IThemeManager themeManager)
        {
            _themeManager = themeManager;
        }

        public IThemeManagerBinding this[string key] => new ThemeManagerBinding(_themeManager, key);
    }
}
