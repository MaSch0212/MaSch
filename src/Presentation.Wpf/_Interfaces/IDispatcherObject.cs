using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MaSch.Presentation.Wpf
{
    public interface IDispatcherObject
    {
        Dispatcher Dispatcher { get; }
    }
}
