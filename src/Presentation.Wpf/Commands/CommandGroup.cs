using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Commands;

/// <summary>
/// Represents a group of instances of type <see cref="ICommand"/>.
/// </summary>
/// <seealso cref="DependencyObject" />
/// <seealso cref="ICommand" />
[ContentProperty(nameof(Commands))]
[DefaultProperty(nameof(Commands))]
public class CommandGroup : DependencyObject, ICommand
{
    /// <summary>
    /// Dependency property. Gets or sets the commands inside this group.
    /// </summary>
    public static readonly DependencyProperty CommandsProperty =
        DependencyProperty.Register("Commands", typeof(ObservableCollection<CommandGroupItem>), typeof(CommandGroup), new PropertyMetadata(null, OnCommandsChanged));

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandGroup"/> class.
    /// </summary>
    public CommandGroup()
    {
        Commands = new ObservableCollection<CommandGroupItem>();
    }

    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Gets or sets the commands inside this group.
    /// </summary>
    [Bindable(true)]
    public ObservableCollection<CommandGroupItem> Commands
    {
        get => (ObservableCollection<CommandGroupItem>)GetValue(CommandsProperty);
        set => SetValue(CommandsProperty, value);
    }

    /// <summary>
    /// Defines the method that determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
    /// <returns>
    ///   <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
    /// </returns>
    public bool CanExecute(object? parameter)
    {
        if (Commands?.Any(x => x.Command != null) != true)
            return false;
        foreach (var command in Commands.Where(x => x.Command != null))
        {
            if (!command.CanExecute())
                return false;
        }

        return true;
    }

    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
    public void Execute(object? parameter)
    {
        if (Commands?.Any(x => x.Command != null) != true)
            return;
        foreach (var command in Commands.Where(x => x.Command != null))
        {
            command.Execute();
        }
    }

    private static void OnCommandsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is CommandGroup command)
        {
            if (e.OldValue is ObservableCollection<CommandGroupItem> oldList)
            {
                oldList.CollectionChanged -= command.Commands_CollectionChanged;
                oldList.Where(x => x.Command != null).ForEach(x => x.Command.CanExecuteChanged -= command.RaiseCanExecuteChanged);
                oldList.ForEach(x => x.CommandChanged -= command.CommandEventHandler);
            }

            if (e.NewValue is ObservableCollection<CommandGroupItem> newList)
            {
                newList.CollectionChanged += command.Commands_CollectionChanged;
                newList.Where(x => x.Command != null).ForEach(x => x.Command.CanExecuteChanged += command.RaiseCanExecuteChanged);
                newList.ForEach(x => x.CommandChanged += command.CommandEventHandler);
            }

            command.RaiseCanExecuteChanged(command, new EventArgs());
        }
    }

    private void RaiseCanExecuteChanged(object? sender, EventArgs e)
    {
        CanExecuteChanged?.Invoke(sender, e);
    }

    private void CommandEventHandler(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is ICommand oldCommand)
            oldCommand.CanExecuteChanged -= RaiseCanExecuteChanged;
        if (e.NewValue is ICommand newCommand)
            newCommand.CanExecuteChanged += RaiseCanExecuteChanged;
    }

    private void Commands_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (var entry in e.OldItems.OfType<CommandGroupItem>())
            {
                if (entry.Command != null)
                    entry.Command.CanExecuteChanged -= RaiseCanExecuteChanged;
                entry.CommandChanged -= CommandEventHandler;
            }
        }

        if (e.NewItems != null)
        {
            foreach (var entry in e.NewItems.OfType<CommandGroupItem>())
            {
                if (entry.Command != null)
                    entry.Command.CanExecuteChanged += RaiseCanExecuteChanged;
                entry.CommandChanged += CommandEventHandler;
            }
        }

        RaiseCanExecuteChanged(this, new EventArgs());
    }
}
