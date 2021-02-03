﻿using MaSch.Core.Attributes;
using MaSch.Core.Observable;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.ViewModels.MessageBox
{
    /// <summary>
    /// Observable properties of the <see cref="BrushGeometry"/> class.
    /// </summary>
    [ObservablePropertyDefinition]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Property definition interface should be first in file.")]
    internal interface IBrushGeometry_Props
    {
        /// <summary>
        /// Gets or sets the brush that is used for filling the icon.
        /// </summary>
        Brush FillBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush that is used for the stroke of the icon.
        /// </summary>
        Brush StrokeBrush { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        Icon Icon { get; set; }
    }

    /// <summary>
    /// Represents an icon with defined brushes.
    /// </summary>
    /// <seealso cref="MaSch.Core.Observable.ObservableObject" />
    /// <seealso cref="MaSch.Presentation.Wpf.ViewModels.MessageBox.IBrushGeometry_Props" />
    public partial class BrushGeometry : ObservableObject, IBrushGeometry_Props
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrushGeometry"/> class.
        /// </summary>
        public BrushGeometry()
        {
            _fillBrush = new SolidColorBrush(Colors.Black);
            _strokeBrush = new SolidColorBrush(Colors.Transparent);
        }
    }
}
