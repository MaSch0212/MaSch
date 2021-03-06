﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    /// <summary>
    /// Provides dependency properties for the <see cref="ListBox"/> and <see cref="MultiSelector"/> controls. Mostly copied from https://stackoverflow.com/a/29457423.
    /// </summary>
    /// <seealso cref="DependencyObject" />
    public class MultiSelect : DependencyObject
    {
        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether the <see cref="Selector.SelectionChanged"/> event should be subscribed to update the items in the <see cref="SelectedItemsProperty"/>.
        /// </summary>
        public static readonly DependencyProperty IsSubscribedToSelectionChangedProperty =
               DependencyProperty.RegisterAttached(
                   "IsSubscribedToSelectionChanged",
                   typeof(bool),
                   typeof(MultiSelect),
                   new PropertyMetadata(default(bool)));

        /// <summary>
        /// Dependency property. Gets or sets a collection of selected items. For this to work, set the <see cref="IsSubscribedToSelectionChangedProperty"/> to <c>true</c>.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItems",
                typeof(IList),
                typeof(MultiSelect),
                new PropertyMetadata(default(IList), OnSelectedItemsChanged));

        /// <summary>
        /// Sets the value of the <see cref="IsSubscribedToSelectionChangedProperty"/>.
        /// </summary>
        /// <param name="element">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIsSubscribedToSelectionChanged(DependencyObject element, bool value)
            => element.SetValue(IsSubscribedToSelectionChangedProperty, value);

        /// <summary>
        /// Gets the value of the <see cref="IsSubscribedToSelectionChangedProperty"/>.
        /// </summary>
        /// <param name="element">The element to get the value from.</param>
        /// <returns>The value of the <see cref="IsSubscribedToSelectionChangedProperty"/>.</returns>
        public static bool GetIsSubscribedToSelectionChanged(DependencyObject element)
            => (bool)element.GetValue(IsSubscribedToSelectionChangedProperty);

        /// <summary>
        /// Sets the value of the <see cref="SelectedItemsProperty"/>.
        /// </summary>
        /// <param name="element">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetSelectedItems(DependencyObject element, IList value)
            => element.SetValue(SelectedItemsProperty, value);

        /// <summary>
        /// Gets the value of the <see cref="SelectedItemsProperty"/>.
        /// </summary>
        /// <param name="element">The element to get the value from.</param>
        /// <returns>The value of the <see cref="SelectedItemsProperty"/>.</returns>
        public static IList GetSelectedItems(DependencyObject element)
            => (IList)element.GetValue(SelectedItemsProperty);

        /// <summary>
        /// Attaches a list or observable collection to the grid or listbox, syncing both lists (one way sync for simple lists).
        /// </summary>
        /// <param name="d">The DataGrid or ListBox.</param>
        /// <param name="e">The list to sync to.</param>
        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ListBox || d is MultiSelector))
                throw new ArgumentException("Somehow this got attached to an object I don't support. ListBoxes and Multiselectors (DataGrid), people. Geesh =P!");

            var selector = (Selector)d;
            if (e.OldValue is IList oldList)
            {
                if (oldList is INotifyCollectionChanged obs)
                {
                    obs.CollectionChanged -= OnCollectionChanged;
                }

                // If we're orphaned, disconnect lb/dg events.
                if (e.NewValue == null)
                {
                    selector.SelectionChanged -= OnSelectorSelectionChanged;
                    SetIsSubscribedToSelectionChanged(selector, false);
                }
            }

            var newList = (IList?)e.NewValue;
            if (newList != null)
            {
                if (newList is INotifyCollectionChanged obs)
                {
                    obs.CollectionChanged += OnCollectionChanged;
                }

                PushCollectionDataToSelectedItems(newList, selector);
                var isSubscribed = GetIsSubscribedToSelectionChanged(selector);
                if (!isSubscribed)
                {
                    selector.SelectionChanged += OnSelectorSelectionChanged;
                    SetIsSubscribedToSelectionChanged(selector, true);
                }
            }
        }

        /// <summary>
        /// Initially set the selected items to the items in the newly connected collection,
        /// unless the new collection has no selected items and the listbox/grid does, in which case
        /// the flow is reversed. The data holder sets the state. If both sides hold data, then the
        /// bound IList wins and dominates the helpless wpf control.
        /// </summary>
        /// <param name="obs">The list to sync to.</param>
        /// <param name="selector">The grid or listbox.</param>
        private static void PushCollectionDataToSelectedItems(IList obs, DependencyObject selector)
        {
            if (selector is ListBox listBox)
            {
                if (obs.Count > 0)
                {
                    listBox.SelectedItems.Clear();
                    foreach (var ob in obs)
                    {
                        listBox.SelectedItems.Add(ob);
                    }
                }
                else
                {
                    foreach (var ob in listBox.SelectedItems)
                    {
                        obs.Add(ob);
                    }
                }

                return;
            }

            // Maybe other things will use the multiselector base... who knows =P
            if (selector is MultiSelector grid)
            {
                if (obs.Count > 0)
                {
                    grid.SelectedItems.Clear();
                    foreach (var ob in obs)
                    {
                        grid.SelectedItems.Add(ob);
                    }
                }
                else
                {
                    foreach (var ob in grid.SelectedItems)
                    {
                        obs.Add(ob);
                    }
                }

                return;
            }

            throw new ArgumentException("Somehow this got attached to an object I don't support. ListBoxes and Multiselectors (DataGrid), people. Geesh =P!");
        }

        /// <summary>
        /// When the listbox or grid fires a selectionChanged even, we update the attached list to
        /// match it.
        /// </summary>
        /// <param name="sender">The listbox or grid.</param>
        /// <param name="e">Items added and removed.</param>
        private static void OnSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dep = (DependencyObject)sender;
            var items = GetSelectedItems(dep);
            var col = items as INotifyCollectionChanged;

            // Remove the events so we don't fire back and forth, then re-add them.
            if (col != null)
                col.CollectionChanged -= OnCollectionChanged;
            foreach (var oldItem in e.RemovedItems)
                items.Remove(oldItem);
            foreach (var newItem in e.AddedItems)
                items.Add(newItem);
            if (col != null)
                col.CollectionChanged += OnCollectionChanged;
        }

        /// <summary>
        /// When the attached object implements INotifyCollectionChanged, the attached listbox
        /// or grid will have its selectedItems adjusted by this handler.
        /// </summary>
        /// <param name="sender">The listbox or grid.</param>
        /// <param name="e">The added and removed items.</param>
        private static void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Push the changes to the selected item.
            if (sender is ListBox listbox)
            {
                listbox.SelectionChanged -= OnSelectorSelectionChanged;
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    listbox.SelectedItems.Clear();
                }
                else
                {
                    if (e.OldItems != null)
                    {
                        foreach (var oldItem in e.OldItems)
                            listbox.SelectedItems.Remove(oldItem);
                    }

                    if (e.NewItems != null)
                    {
                        foreach (var newItem in e.NewItems)
                            listbox.SelectedItems.Add(newItem);
                    }
                }

                listbox.SelectionChanged += OnSelectorSelectionChanged;
            }

            if (sender is MultiSelector grid)
            {
                grid.SelectionChanged -= OnSelectorSelectionChanged;
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    grid.SelectedItems.Clear();
                }
                else
                {
                    if (e.OldItems != null)
                    {
                        foreach (var oldItem in e.OldItems)
                            grid.SelectedItems.Remove(oldItem);
                    }

                    if (e.NewItems != null)
                    {
                        foreach (var newItem in e.NewItems)
                            grid.SelectedItems.Add(newItem);
                    }
                }

                grid.SelectionChanged += OnSelectorSelectionChanged;
            }
        }
    }
}
