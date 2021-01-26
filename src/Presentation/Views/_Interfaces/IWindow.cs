
namespace MaSch.Presentation.Views
{
    public interface IWindow : IResourceContainer
    {
        WindowVisualState WindowState { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        double Top { get; set; }
        double Left { get; set; }
        bool Topmost { get; set; }

        void Close();
        void Hide();
        bool Activate();
        void Show();
        bool? ShowDialog();
    }
}
