using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.Presentation.Translation
{
    public interface INamedTranslationProvider : ITranslationProvider
    {
        string ProviderKey { get; }
    }
}
