using System.Windows;
using System.Windows.Input;

namespace MaSch.Presentation.Wpf.Commands
{
    /// <summary>
    /// Represents an item inside a <see cref="CommandGroup"/>.
    /// </summary>
    /// <seealso cref="DependencyObject" />
    public class CommandGroupItem : DependencyObject
    {
        /// <summary>
        /// Dependency property. Gets or sets the command that the <see cref="CommandGroupItem"/> represents.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandGroupItem), new PropertyMetadata(null, OnCommandChanged));

        /// <summary>
        /// Dependency property. Gets or sets the parameter to use to execute the command inside the <see cref="CommandProperty"/>.
        /// </summary>
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register("Parameter", typeof(object), typeof(CommandGroupItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command that the <see cref="CommandGroupItem"/> represents.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the parameter to use to execute the command inside the <see cref="CommandProperty"/>.
        /// </summary>
        public object Parameter
        {
            get => GetValue(ParameterProperty);
            set => SetValue(ParameterProperty, value);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Command"/> property changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler CommandChanged;

        /// <summary>
        /// Determines whether this instance can execute.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExecute()
        {
            return Command?.CanExecute(Parameter) ?? false;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Execute()
        {
            Command?.Execute(Parameter);
        }

        private static void OnCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is CommandGroupItem entry)
            {
                entry.CommandChanged?.Invoke(entry, e);
            }
        }
    }
}
