using System.ComponentModel;

namespace MaSch.Presentation.Wpf
{
    /// <summary>
    /// Specifies a binding to a <see cref="IThemeValue"/> inside a <see cref="IThemeManager"/>.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public interface IThemeManagerBinding : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the key to bind to.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets the value to bind to.
        /// </summary>
        IThemeValue Value { get; }
    }
}
