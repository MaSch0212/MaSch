using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MaSch.Core;
using MaSch.Core.Extensions;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    public class DataGrid
    {
        public static readonly DependencyProperty BindableColumnsProperty =
            DependencyProperty.RegisterAttached("BindableColumns",
                                                typeof(ObservableCollection<DataGridColumn>),
                                                typeof(DataGrid),
                                                new UIPropertyMetadata(null, BindableColumnsPropertyChanged));
        private static void BindableColumnsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = source as System.Windows.Controls.DataGrid;
            var columns = e.NewValue as ObservableCollection<DataGridColumn>;
            
            Guard.NotNull(dataGrid, nameof(dataGrid));

            dataGrid.Columns.Clear();
            if (columns == null)
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
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
        }
        public static void SetBindableColumns(DependencyObject element, ObservableCollection<DataGridColumn> value)
        {
            element.SetValue(BindableColumnsProperty, value);
        }
        public static ObservableCollection<DataGridColumn> GetBindableColumns(DependencyObject element)
        {
            return (ObservableCollection<DataGridColumn>)element.GetValue(BindableColumnsProperty);
        }
    }
}
