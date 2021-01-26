using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    public interface IPage
    {
        bool IsPageSelectedByDefault { get; }
        string PageGroupId { get; }
        string PageName { get; }
        string TranslationProviderKey { get; }
        int PagePriority { get; }
        IIcon PageIcon { get; }
        FrameworkElement PageContent { get; }
    }
}
