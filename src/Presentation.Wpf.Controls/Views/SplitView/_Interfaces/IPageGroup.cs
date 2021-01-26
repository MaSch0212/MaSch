using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    public interface IPageGroup
    {
        string PageGroupId { get; }
        string PageGroupName { get; }
        string TranslationProviderKey { get; }
        int PageGroupPriority { get; }
    }
}
