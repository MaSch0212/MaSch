using MaSch.Core.Attributes;
using MaSch.Core.Extensions;
using MaSch.Core.Observable;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MaSch.Update
{
    /// <summary>
    /// Property definition for class <see cref="UpdateDownload"/>.
    /// </summary>
    [ObservablePropertyDefinition]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Property definition")]
    internal interface IUpdateDownload_Props
    {
        /// <summary>
        /// Gets the progress of the currently running download.
        /// </summary>
        double Progress { get; }

        /// <summary>
        /// Gets a value indicating whether the last download has been finished.
        /// </summary>
        bool? HasFinished { get; }

        /// <summary>
        /// Gets a value indicating whether a download is executed.
        /// </summary>
        bool IsDownloading { get; }

        /// <summary>
        /// Gets the current download speed in bytes/s.
        /// </summary>
        double DownloadSpeed { get; }
    }

    /// <summary>
    /// Represents a download for the <see cref="UpdateController"/>.
    /// </summary>
    /// <seealso cref="MaSch.Core.Observable.ObservableObject" />
    /// <seealso cref="System.IDisposable" />
    public sealed partial class UpdateDownload : ObservableObject, IUpdateDownload_Props, IDisposable
    {
        private readonly WebClient _webClient;
        private readonly NetSpeedCounter _counter;

        /// <summary>
        /// Is raised when the progress of the download have changed.
        /// </summary>
        public event DownloadProgressChangedEventHandler? DownloadProgressChanged;

        /// <summary>
        /// Is raised when the download has completed.
        /// </summary>
        public event AsyncCompletedEventHandler? DownloadCompleted;

        /// <summary>
        /// Gets the <see cref="Uri"/> from which to download the file.
        /// </summary>
        public Uri SourceUri { get; }

        /// <summary>
        /// Gets the file path to which the file should be downloaded to.
        /// </summary>
        public string TargetFilePath { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDownload"/> class.
        /// </summary>
        /// <param name="sourceUri">The <see cref="Uri"/> from which to download the file.</param>
        /// <param name="targetFilePath">The file path to which the file should be downloaded to.</param>
        public UpdateDownload(Uri sourceUri, string targetFilePath)
        {
            SourceUri = sourceUri;
            TargetFilePath = targetFilePath;

            _webClient = new WebClient();
            _counter = new NetSpeedCounter(_webClient);
            _counter.SpeedChanged += (s, e) => DownloadSpeed = e;
        }

        /// <summary>
        /// Starts the download in the background.
        /// </summary>
        public void Start()
        {
            ExecuteAsync().Forget();
        }

        /// <summary>
        /// Executes the download asynchronously.
        /// </summary>
        /// <returns>A value inidcating whether the download has been completed successfully.</returns>
        /// <exception cref="InvalidOperationException">The downloadload is already being executed. You can cancel the current operation by using the Cancel method.</exception>
        public async Task<bool> ExecuteAsync()
        {
            bool cancelled = false;

            if (Progress > 0)
                throw new InvalidOperationException("The download is already being executed. You can cancel the current operation by using the Cancel method.");
            IsDownloading = true;
            HasFinished = false;

            var targetDirPath = Path.GetDirectoryName(TargetFilePath);
            if (!string.IsNullOrEmpty(targetDirPath))
                Directory.CreateDirectory(targetDirPath);

            _webClient.DownloadProgressChanged += OnProgressChanged;
            _webClient.DownloadFileCompleted += OnCompleted;
            try
            {
                _counter.Reset();
                await _webClient.DownloadFileTaskAsync(SourceUri, TargetFilePath);
            }
            finally
            {
                _webClient.DownloadProgressChanged -= OnProgressChanged;
                _webClient.DownloadFileCompleted -= OnCompleted;
                IsDownloading = false;
                HasFinished = true;
                DownloadSpeed = 0;
            }

            return !cancelled;

            void OnProgressChanged(object sender, DownloadProgressChangedEventArgs e)
            {
                Progress = (double)e.BytesReceived / e.TotalBytesToReceive;
                DownloadProgressChanged?.Invoke(this, e);
            }

            void OnCompleted(object? sender, AsyncCompletedEventArgs e)
            {
                cancelled = e.Cancelled;
                Progress = cancelled ? 0 : 1;
                DownloadCompleted?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Cancels the current download operation.
        /// </summary>
        public void Cancel()
        {
            _webClient.CancelAsync();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _webClient.Dispose();
            _counter.Dispose();
        }

        // https://stackoverflow.com/questions/20794682/accurate-measurement-of-download-speed-of-a-webclient
        private sealed class NetSpeedCounter : IDisposable
        {
            private readonly Stopwatch _stopwatch = new ();
            private readonly WebClient _webClient;
            private long _prevBytes = 0;

            public event EventHandler<double>? SpeedChanged;

            public double Speed { get; private set; }

            public NetSpeedCounter(WebClient webClient)
            {
                _webClient = webClient;
                _webClient.DownloadProgressChanged += WebClientOnDownloadProgressChanged;
            }

            public void Reset()
            {
                _prevBytes = 0;
                _stopwatch.Stop();
                _stopwatch.Reset();
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                _webClient.DownloadProgressChanged -= WebClientOnDownloadProgressChanged;
            }

            private void WebClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
            {
                if (!_stopwatch.IsRunning)
                {
                    _stopwatch.Restart();
                    return;
                }

                if (_stopwatch.ElapsedMilliseconds < 100)
                    return;

                _stopwatch.Stop();

                var speed = (e.BytesReceived - _prevBytes) / _stopwatch.Elapsed.TotalSeconds;
                _prevBytes = e.BytesReceived;

                Speed = speed;
                SpeedChanged?.Invoke(this, Speed);

                _stopwatch.Restart();
            }
        }
    }
}
