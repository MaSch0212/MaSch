using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MaSch.Core;
using MaSch.Core.Extensions;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    /// <summary>
    /// Provides dependency properties for the <see cref="System.Windows.Controls.DataGrid"/> control.
    /// </summary>
    public class DataGrid
    {
        /// <summary>
        /// Dependency property. Gets or sets <see cref="DataGridColumn"/>s that can be bound using data bindings.
        /// </summary>
        public static readonly DependencyProperty BindableColumnsProperty =
            DependencyProperty.RegisterAttached(
                "BindableColumns",
                typeof(ObservableCollection<DataGridColumn>),
                typeof(DataGrid),
                new UIPropertyMetadata(null, BindableColumnsPropertyChanged));

        private static void BindableColumnsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = Guard.OfType<System.Windows.Controls.DataGrid>(source, nameof(source));

            dataGrid.Columns.Clear();
            if (e.NewValue is not ObservableCollection<DataGridColumn> columns)
            {
                return;
            }

            foreach (var column in columns)
            {
                dataGrid.Columns.Add(column);
            }

            columns.CollectionChanged += (sender, e2) =>
            {
                switch (e2.Action)
                {
                    case NotifyCollectionChangedAction.Reset:
                        dataGrid.Columns.Clear();
                        dataGrid.Columns.Add(e2.NewItems.OfType<DataGridColumn>());
                        break;
                    case NotifyCollectionChangedAction.Add:
                        dataGrid.Columns.Add(e2.NewItems.OfType<DataGridColumn>());
                        break;
                    case NotifyCollectionChangedAction.Move:
                        dataGrid.Columns.Move(e2.OldStartingIndex, e2.NewStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        dataGrid.Columns.Remove(e2.OldItems.OfType<DataGridColumn>());
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        dataGrid.Columns[e2.NewStartingIndex] = e2.NewItems[0] as DataGridColumn;
                        break;
                }
            };
        }

        /// <summary>
        /// Sets the value of the <see cref="BindableColumnsProperty"/>.
        /// </summary>
        /// <param name="element">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetBindableColumns(DependencyObject element, ObservableCollection<DataGridColumn> value)
        {
            element.SetValue(BindableColumnsProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="BindableColumnsProperty"/>.
        /// </summary>
        /// <param name="element">The element to get the value from.</param>
        /// <returns>The value of the <see cref="BindableColumnsProperty"/>.</returns>
        public static ObservableCollection<DataGridColumn> GetBindableColumns(DependencyObject element)
        {
            return (ObservableCollection<DataGridColumn>)element.GetValue(BindableColumnsProperty);
        }
    }
}
