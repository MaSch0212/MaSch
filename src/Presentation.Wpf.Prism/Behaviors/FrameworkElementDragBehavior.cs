using MaSch.Presentation.Wpf.Common;
using System.Windows;
using System.Windows.Interactivity;

namespace MaSch.Presentation.Wpf.Behaviors
{
    /// <summary>
    /// A behavior that handles drag in drag &amp; drop.
    /// </summary>
    public class FrameworkElementDragBehavior : Behavior<FrameworkElement>
    {
        private bool _isMouseClicked;

        /// <summary>
        /// Is executed when the behavior is attached to a <see cref="DependencyObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
        }

        /// <summary>
        /// Is executed when the behavior is detaching from a <see cref="DependencyObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
            AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isMouseClicked = true;
        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isMouseClicked = false;
        }

        private void AssociatedObject_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isMouseClicked && AssociatedObject.DataContext is IDragable dragObject)
            {
                var data = new DataObject();
                data.SetData(dragObject.DataType, AssociatedObject.DataContext);
                DragDrop.DoDragDrop(AssociatedObject, data, DragDropEffects.Move);
            }

            _isMouseClicked = false;
        }
    }
}
