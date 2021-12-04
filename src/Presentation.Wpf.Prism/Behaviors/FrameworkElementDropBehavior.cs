using MaSch.Presentation.Wpf.Common;
using System.Windows;
using System.Windows.Interactivity;

namespace MaSch.Presentation.Wpf.Behaviors;

/// <summary>
/// A behavior that handles drop in drag &amp; drop.
/// </summary>
public class FrameworkElementDropBehavior : Behavior<FrameworkElement>
{
    private Type? _dataType;

    /// <summary>
    /// Is executed when the behavior is attached to a <see cref="DependencyObject"/>.
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.AllowDrop = true;
        AssociatedObject.DragEnter += AssociatedObject_DragEnter;
        AssociatedObject.DragOver += AssociatedObject_DragOver;
        AssociatedObject.DragLeave += AssociatedObject_DragLeave;
        AssociatedObject.Drop += AssociatedObject_Drop;
    }

    /// <summary>
    /// Is executed when the behavior is detaching from a <see cref="DependencyObject"/>.
    /// </summary>
    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.DragEnter -= AssociatedObject_DragEnter;
        AssociatedObject.DragOver -= AssociatedObject_DragOver;
        AssociatedObject.DragLeave -= AssociatedObject_DragLeave;
        AssociatedObject.Drop -= AssociatedObject_Drop;
    }

    private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
    {
        if (_dataType == null && AssociatedObject.DataContext is IDropable dropObject)
        {
            _dataType = dropObject.DataType;
        }

        e.Handled = true;
    }

    private void AssociatedObject_DragOver(object sender, DragEventArgs e)
    {
        if (_dataType != null && e.Data.GetDataPresent(_dataType))
        {
            SetDragDropEffects(e);
        }
    }

    private void SetDragDropEffects(DragEventArgs e)
    {
        e.Effects = DragDropEffects.None;

        if (e.Data.GetDataPresent(_dataType))
        {
            e.Effects = DragDropEffects.Move;
        }
    }

    private void AssociatedObject_DragLeave(object sender, DragEventArgs e)
    {
        e.Handled = true;
    }

    private void AssociatedObject_Drop(object sender, DragEventArgs e)
    {
        if (_dataType != null && e.Data.GetDataPresent(_dataType))
        {
            var target = AssociatedObject.DataContext as IDropable;
            target?.Drop(e.Data.GetData(_dataType));

            var source = e.Data.GetData(_dataType) as IDragable;
            source?.Remove(e.Data.GetData(_dataType));
        }
    }
}
