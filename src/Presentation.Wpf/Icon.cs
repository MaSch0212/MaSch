using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf
{
    public class Icon : IIcon, INotifyPropertyChanged
    {
        #region Fields

        private SymbolType _type = SymbolType.Geometry;
        private string _character;
        private FontFamily _font;
        private double _fontSize;
        private Geometry _geometry;
        private bool _isGeometryFilled = true;
        private double _geometryStrokeThickness;
        private Stretch _stretch = Stretch.Uniform;
        private Transform _transform;

        #endregion
        
        #region Properties

        public SymbolType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
        public string Character
        {
            get => _character;
            set => SetProperty(ref _character, value);
        }
        public FontFamily Font
        {
            get => _font;
            set => SetProperty(ref _font, value);
        }
        public double FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }
        public Geometry Geometry
        {
            get => _geometry;
            set => SetProperty(ref _geometry, value);
        }
        public bool IsGeometryFilled
        {
            get => _isGeometryFilled;
            set => SetProperty(ref _isGeometryFilled, value);
        }
        public double GeometryStrokeThickness
        {
            get => _geometryStrokeThickness;
            set => SetProperty(ref _geometryStrokeThickness, value);
        }
        public Stretch Stretch
        {
            get => _stretch;
            set => SetProperty(ref _stretch, value);
        }
        public Transform Transform
        {
            get => _transform;
            set => SetProperty(ref _transform, value);
        }

        #endregion

        public Icon()
        {
            Type = SymbolType.Geometry;
            Geometry = null;
            IsGeometryFilled = true;
            GeometryStrokeThickness = 0;
            Stretch = Stretch.Uniform;
            Font = null;
            Character = null;
            FontSize = 12;
        }

        public Icon(Geometry geometry, bool filled = true, double strokeThickness = 0, Stretch stretch = Stretch.Uniform)
        {
            Type = SymbolType.Geometry;
            Geometry = geometry;
            IsGeometryFilled = filled;
            GeometryStrokeThickness = strokeThickness;
            Stretch = stretch;
        }

        public Icon(FontFamily font, string character, double fontSize = 12, Stretch stretch = Stretch.Uniform)
        {
            Type = SymbolType.Character;
            Font = font;
            FontSize = fontSize;
            Character = character;
            Stretch = stretch;
        }

        #region Observable Object
        public event PropertyChangedEventHandler PropertyChanged;

        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public virtual void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            property = value;
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
