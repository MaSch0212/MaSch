using System;
using System.Collections.Generic;
using System.Text;

namespace MaSch.Presentation
{
    public interface IResourceContainer
    {
        object FindResource(object resourceKey);
        object TryFindResource(object resourceKey);
    }
}
