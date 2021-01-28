using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MaSch.Core.Observable;

namespace MaSch.Presentation.Update
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a download.
    /// </summary>
    public class Download : ObservableObject
    {
        /// <summary>
        /// Is raised when the progress of the download have changed.
        /// </summary>
        public event DownloadProgressChangedEventHandler DownloadProgressChanged;

        /// <summary>
        /// Is raised when the download has completed.
        /// </summary>
        public event AsyncCompletedEventHandler DownloadCompleted;

        #region Private fields

        private readonly Uri _baseUri;
        private readonly Uri _downloadUri;
        private readonly WebClient _webClient;
        private readonly string _targetDirectory;
        private readonly NetSpeedCounter _counter;

        private float _progress = float.NaN;
        private string _localFilePath = string.Empty;
        private bool _hasFinished;
        private bool _isDownloading;
        private double _downloadSpeed = double.NaN;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Progress of this download.
        /// </summary>
        public float Progress
        {
            get => _progress;
            private set => SetProperty(ref _progress, value);
        }

        /// <summary>
        /// Gets the Target path for the file to download.
        /// </summary>
        public string LocalFilePath
        {
            get => _localFilePath;
            private set => SetProperty(ref _localFilePath, value);
        }

        /// <summary>
        /// Gets a value indicating whether the download of this object has finished.
        /// </summary>
        public bool HasFinished
        {
            get => _hasFinished;
            private set => SetProperty(ref _hasFinished, value);
        }

        /// <summary>
        /// Gets a value indicating whether this object is downloading a file.
        /// </summary>
        public bool IsDownloading
        {
            get => _isDownloading;
            private set => SetProperty(ref _isDownloading, value);
        }

        /// <summary>
        /// Gets the download speed in Bytes/s.
        /// </summary>
        public double DownloadSpeed
        {
            get => _downloadSpeed;
            private set => SetProperty(ref _downloadSpeed, value);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Download"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="downloadUri">The download URI.</param>
        /// <param name="webClient">The web client.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <exception cref="ArgumentException">The download url is not a parent of the base uri. BaseUrl = {baseUri}, DownloadUrl = {downloadUri}.</exception>
        internal Download(Uri baseUri, Uri downloadUri, WebClient webClient, string targetDirectory)
        {
            if (!downloadUri.IsAbsoluteUri)
                downloadUri = new Uri(baseUri, downloadUri);
            else if (!downloadUri.ToString().StartsWith(baseUri.ToString()))
                throw new ArgumentException($"The download url is not a parent of the base uri. BaseUrl = {baseUri}, DownloadUrl = {downloadUri}");
            _baseUri = baseUri;
            _downloadUri = downloadUri;
            _targetDirectory = targetDirectory;
            _webClient = webClient;
            _counter = new NetSpeedCounter(_webClient);
            _counter.SpeedChanged += (s, e) => DownloadSpeed = e;
        }

        /// <summary>
        /// Starts the download.
        /// </summary>
        /// <exception cref="InvalidOperationException">The download has not finished yet. You can stop the download with the method StopDownload.</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        internal async Task StartDownload()
        {
            bool cancelled = false;

            if (Progress > 0)
                throw new InvalidOperationException("The download has not finished yet. You can stop the download with the method StopDownload.");
            IsDownloading = true;
            var relative = _downloadUri.ToString()[_baseUri.ToString().Length..];

            var filePath = Path.Combine(new[] { _targetDirectory }.Concat(relative.Split('/', '\\')).ToArray());
            var dirPath = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dirPath) && !Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            var progch = new DownloadProgressChangedEventHandler((s, e) =>
            {
                Progress = (float)((double)e.BytesReceived / e.TotalBytesToReceive);
                DownloadProgressChanged?.Invoke(this, e);
            });
            var progcpl = new AsyncCompletedEventHandler((s, e) =>
            {
                cancelled = e.Cancelled;
                Progress = 1;
                DownloadCompleted?.Invoke(this, e);
            });

            _webClient.DownloadProgressChanged += progch;
            _webClient.DownloadFileCompleted += progcpl;
            try
            {
                _counter.Reset();
                await _webClient.DownloadFileTaskAsync(_downloadUri.ToString(), filePath);
            }
            finally
            {
                _webClient.DownloadProgressChanged -= progch;
                _webClient.DownloadFileCompleted -= progcpl;
                IsDownloading = false;
            }

            if (!cancelled)
            {
                LocalFilePath = filePath;
                HasFinished = true;
            }
        }

        // Credit: https://stackoverflow.com/questions/20794682/accurate-measurement-of-download-speed-of-a-webclient
        private class NetSpeedCounter
        {
            private readonly Stopwatch _stopwatch = new Stopwatch();
            private long _prevBytes = 0;

            public event EventHandler<double> SpeedChanged;

            public double Speed { get; private set; }

            public NetSpeedCounter(WebClient webClient)
            {
                webClient.DownloadProgressChanged += WebClientOnDownloadProgressChanged;
            }

            public void Reset()
            {
                _prevBytes = 0;
                _stopwatch.Stop();
                _stopwatch.Reset();
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
