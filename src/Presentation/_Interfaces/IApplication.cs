using MaSch.Presentation.Views;

namespace MaSch.Presentation
{
    public interface IApplication : IResourceContainer
    {
        void InitializeComponent();
        int Run();
        int Run(IWindow window);
        void Shutdown();
        void Shutdown(int exitCode);
    }
}
