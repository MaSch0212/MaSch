using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.Presentation.Wpf
{
    public interface IThemeManagerBindingFactory
    {
        IThemeManagerBinding this[string key] { get; }
    }
}
