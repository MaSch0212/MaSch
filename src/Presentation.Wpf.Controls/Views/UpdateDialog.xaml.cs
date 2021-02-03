using MaSch.Presentation.Update;
using MaSch.Presentation.Wpf.Converter;
using MaSch.Presentation.Wpf.Helper;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using Window = MaSch.Presentation.Wpf.Controls.Window;

namespace MaSch.Presentation.Wpf.Views
{
    /// <summary>
    /// Dialog that can be used to update the software using a <see cref="UpdateController"/>.
    /// </summary>
    public partial class UpdateDialog : Window
    {
        private readonly UpdateController _updater;
        private readonly string _currentVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDialog"/> class.
        /// </summary>
        /// <param name="updater">The updater to use.</param>
        /// <param name="currentVersion">The current version.</param>
        public UpdateDialog(UpdateController updater, string currentVersion)
        {
            DataContext = _updater = updater;
            _currentVersion = currentVersion;
            updater.PropertyChanged += Updater_PropertyChanged;
            InitializeComponent();
        }

        private void Updater_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UpdateController.Progress))
            {
                Dispatcher.Invoke(() =>
                {
                    var (speed, speedSuffix) = ByteSizeToStringConverter.Format(_updater.DownloadSpeed);
                    DownloadProgressBar.CurrentSpeedFormat = "{0:0.00}";
                    DownloadProgressBar.CurrentSpeedUnit = speedSuffix + "/s";
                    if (_updater.Progress > DownloadProgressBar.Value)
                        DownloadProgressBar.SetProgress(_updater.Progress, speed);
                });
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void CancelDownload_Click(object sender, RoutedEventArgs e)
        {
            _updater.CancelDownload();
            DialogResult = false;
            Close();
        }

        private void StartSetupButton_Click(object sender, RoutedEventArgs e)
        {
            var setupFile = Path.Combine(_downloadedFiles.targetDir, _updater.VersionsInformation.SetupPath);
            var powershellPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "WindowsPowerShell", "v1.0", "powershell.exe");
            var script = new StringBuilder();

#if NETSTANDARD || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NETFRAMEWORK
            var pid = Process.GetCurrentProcess().Id;
#else
            var pid = Environment.ProcessId;
#endif

            script.Append($"$process = Get-Process -Id {pid} -ErrorAction SilentlyContinue; ");
            script.Append("while($process -ne $null -and -not $process.HasExited) { [System.Threading.Thread]::Sleep(1000); } ");
            script.Append($"& '{setupFile}' -d'{Path.GetDirectoryName(GetType().Assembly.Location)}' -s");
            Process.Start(new ProcessStartInfo(powershellPath, $"-ExecutionPolicy ByPass -Command \"{script}\"")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            });
            DialogResult = true;
            Application.Current.Shutdown();
            Close();
        }

        private (string[] files, string targetDir) _downloadedFiles;

        private async void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            AnimationHelper.SwitchViews(PageChangelog, PageDownload, SwitchDirection.Right);
            try
            {
                _downloadedFiles = await _updater.DownloadAllNeededFiles(_currentVersion);
                AnimationHelper.SwitchViews(PageDownload, PageFinish, SwitchDirection.Right);
            }
            catch (Exception)
            {
                AnimationHelper.SwitchViews(PageDownload, PageError, SwitchDirection.Down);
            }
        }

        /// <summary>
        /// Shows a new <see cref="UpdateDialog"/> as a dialog.
        /// </summary>
        /// <param name="updater">The updater to use.</param>
        /// <param name="currentVersion">The current version.</param>
        /// <returns>The result of the dialog.</returns>
        public static bool? ShowDialog(UpdateController updater, string currentVersion)
            => ShowDialog(updater, currentVersion, null);

        /// <summary>
        /// Shows a new <see cref="UpdateDialog"/> as a dialog.
        /// </summary>
        /// <param name="updater">The updater to use.</param>
        /// <param name="currentVersion">The current version.</param>
        /// <param name="owner">The owner window of the dialog to show.</param>
        /// <returns>The result of the dialog.</returns>
        public static bool? ShowDialog(UpdateController updater, string currentVersion, System.Windows.Window owner)
        {
            var dialog = new UpdateDialog(updater, currentVersion);
            if (owner != null)
                dialog.Owner = owner;
            return dialog.ShowDialog();
        }
    }
}
