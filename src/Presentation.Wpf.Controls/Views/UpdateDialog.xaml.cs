using MaSch.Presentation.Update;
using MaSch.Presentation.Wpf.Converter;
using MaSch.Presentation.Wpf.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Window = MaSch.Presentation.Wpf.Controls.Window;

namespace MaSch.Presentation.Wpf.Views
{
    /// <summary>
    /// Interaction logic for UpdateDialog.xaml
    /// </summary>
    public partial class UpdateDialog : Window
    {
        private readonly UpdateController _updater;
        private readonly string _currentVersion;

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
            var setupFile = Path.Combine(_downloadedFiles.Item2, _updater.VersionsInformation.SetupPath);
            var powershellPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "WindowsPowerShell", "v1.0", "powershell.exe");
            var script = new StringBuilder();
            script.Append($"$process = Get-Process -Id {Process.GetCurrentProcess().Id} -ErrorAction SilentlyContinue; ");
            script.Append("while($process -ne $null -and -not $process.HasExited) { [System.Threading.Thread]::Sleep(1000); } ");
            script.Append($"& '{setupFile}' -d'{Path.GetDirectoryName(GetType().Assembly.Location)}' -s");
            Process.Start(new ProcessStartInfo(powershellPath, $"-ExecutionPolicy ByPass -Command \"{script}\"")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
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
            catch (Exception) { AnimationHelper.SwitchViews(PageDownload, PageError, SwitchDirection.Down); }
        }

        public static bool? ShowDialog(UpdateController updater, string currentVersion)
            => ShowDialog(updater, currentVersion, null);
        public static bool? ShowDialog(UpdateController updater, string currentVersion, System.Windows.Window owner)
        {
            var dialog = new UpdateDialog(updater, currentVersion);
            if (owner != null)
                dialog.Owner = owner;
            return dialog.ShowDialog();
        }
    }
}
