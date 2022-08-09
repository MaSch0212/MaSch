using MaSch.Generators.Generators.ObservableObject.Models;
using MaSch.Generators.Support;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.Generators.Generators.ObservableObject.Generation;

[MemberGenerator(typeof(ObservableObjectGeneratorContext))]
internal readonly partial struct ClassGenerator
{
    public void NotifyPropertyChanged(INamedTypeSymbol typeSymbol)
    {
    }

    public void ObservableProperty(INamedTypeSymbol typeSymbol)
    {
    }
}
