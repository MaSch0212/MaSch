using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using Win = System.Windows.Controls;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    /// <summary>
    /// Provides dependency properties for the <see cref="System.Windows.Controls.TextBlock"/> control.
    /// </summary>
    public static class TextBlock
    {
        /// <summary>
        /// Dependency property. Gets or sets the inline elements of the text block.
        /// </summary>
        public static readonly DependencyProperty RichTextProperty =
            DependencyProperty.RegisterAttached(
                "RichText",
                typeof(IEnumerable<Inline>),
                typeof(TextBlock),
                new PropertyMetadata(0.0, OnRichTextPropertyChanged));

        /// <summary>
        /// Gets the value of the <see cref="RichTextProperty"/>.
        /// </summary>
        /// <param name="depObj">The element to get the value from.</param>
        /// <returns>The value of the <see cref="RichTextProperty"/>.</returns>
        public static IEnumerable<Inline> GetRichText(DependencyObject depObj)
        {
            return (IEnumerable<Inline>)depObj.GetValue(RichTextProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="RichTextProperty"/>.
        /// </summary>
        /// <param name="depObj">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetRichText(DependencyObject depObj, IEnumerable<Inline> value)
        {
            depObj.SetValue(RichTextProperty, value);
        }

        private static void OnRichTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Win.TextBlock tb)
            {
                void CollectionChanged(object? s, NotifyCollectionChangedEventArgs ea)
                {
                    if (ea.Action == NotifyCollectionChangedAction.Reset)
                    {
                        tb.Inlines.Clear();
                    }
                    else
                    {
                        if (ea.OldItems != null)
                        {
                            for (int i = 0; i < ea.OldItems.Count; i++)
                                tb.Inlines.Remove(tb.Inlines.ElementAt(ea.OldStartingIndex));
                        }

                        Inline? current = null;
                        bool last = false;

                        if (ea.NewItems != null)
                        {
                            foreach (Inline item in ea.NewItems)
                            {
                                if (current == null)
                                {
                                    if (ea.NewStartingIndex == 0)
                                    {
                                        current = tb.Inlines.FirstInline;
                                    }
                                    else if (ea.NewStartingIndex >= tb.Inlines.Count)
                                    {
                                        last = true;
                                        current = tb.Inlines.LastInline;
                                    }
                                    else
                                    {
                                        current = tb.Inlines.ElementAt(ea.NewStartingIndex);
                                    }
                                }

                                if (last)
                                    tb.Inlines.InsertAfter(current, item);
                                else
                                    tb.Inlines.InsertBefore(current, item);
                            }
                        }
                    }
                }

                if (e.OldValue is INotifyCollectionChanged oncc)
                {
                    oncc.CollectionChanged -= CollectionChanged;
                }

                if (e.NewValue is IEnumerable<Inline> inlines)
                {
                    tb.Inlines.Clear();
                    tb.Inlines.AddRange(inlines);
                }

                if (e.NewValue is INotifyCollectionChanged nncc)
                {
                    nncc.CollectionChanged += CollectionChanged;
                }
            }
        }
    }
}
