using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.Presentation.Wpf
{
    public interface IThemeManagerBinding : INotifyPropertyChanged
    {
        string Key { get; }
        IThemeValue Value { get; }
    }
}
