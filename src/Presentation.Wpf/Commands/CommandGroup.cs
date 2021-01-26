using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using MaSch.Common.Extensions;

namespace MaSch.Presentation.Wpf.Commands
{
    [ContentProperty(nameof(Commands))]
    [DefaultProperty(nameof(Commands))]
    public class CommandGroup : DependencyObject, ICommand
    {
        public static readonly DependencyProperty CommandsProperty =
            DependencyProperty.Register("Commands", typeof(ObservableCollection<CommandGroupItem>), typeof(CommandGroup), new PropertyMetadata(null, OnCommandsChanged));

        [Bindable(true)]
        public ObservableCollection<CommandGroupItem> Commands
        {
            get => (ObservableCollection<CommandGroupItem>)GetValue(CommandsProperty);
            set => SetValue(CommandsProperty, value);
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

        public CommandGroup()
        {
            Commands = new ObservableCollection<CommandGroupItem>();
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
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

        public void Execute(object parameter)
        {
            if (Commands?.Any(x => x.Command != null) != true)
                return;
            foreach(var command in Commands.Where(x => x.Command != null))
            {
                command.Execute();
            }
        }

        private void RaiseCanExecuteChanged(object sender, EventArgs e)
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

        private void Commands_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var entry in e.OldItems.OfType<CommandGroupItem>())
                {
                    if(entry.Command != null)
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

    public class CommandGroupItem : DependencyObject
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandGroupItem), new PropertyMetadata(null, OnCommandChanged));
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register("Parameter", typeof(object), typeof(CommandGroupItem), new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public object Parameter
        {
            get => GetValue(ParameterProperty);
            set => SetValue(ParameterProperty, value);
        }

        private static void OnCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if(obj is CommandGroupItem entry)
            {
                entry.CommandChanged?.Invoke(entry, e);
            }
        }

        public event DependencyPropertyChangedEventHandler CommandChanged;

        public bool CanExecute()
        {
            return Command?.CanExecute(Parameter) ?? false;
        }

        public void Execute()
        {
            Command?.Execute(Parameter);
        }
    }
}
