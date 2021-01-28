using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MaSch.Core;
using MaSch.Core.Observable;
using MaSch.Presentation.Update.Models;

namespace MaSch.Presentation.Update
{
    /// <inheritdoc />
    /// <summary>
    /// Is used for updating the current application.
    /// </summary>
    public class UpdateController : ObservableObject
    {
        private readonly ICache _cache = new Cache();

        private Uri ChangeLogsUri => _cache.GetValue(() => new Uri(RootUri, "Changelogs/"));
        private Uri VersionsInformationUri => _cache.GetValue(() => new Uri(RootUri, "versions.xml"));

        #region Fields

        private readonly WebClient _webClient;
        private bool _wasCanceled;
        private float _progress = float.NaN;
        private string _currentDownloadedFile = string.Empty;
        private bool _hasFinished;
        private bool _isDownloading;
        private double _downloadSpeed = double.NaN;
        private bool _isPreparingDownload;
        private long _downloadedBytes;
        private long _bytesToDownload;
        private bool _isChecking;
        private string _changelog;
        private bool _isUpdateAvailable;
        private Versions _versionsInformation;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root url at which the updater retrieves its information.
        /// </summary>
        public Uri RootUri { get; }

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
        public string CurrentDownloadedFile
        {
            get => _currentDownloadedFile;
            private set => SetProperty(ref _currentDownloadedFile, value);
        }

        /// <summary>
        /// Gets a value indicating whether the download of this object has been finished.
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

        /// <summary>
        /// Gets a value indicating whether a download is currently being prepared.
        /// </summary>
        public bool IsPreparingDownload
        {
            get => _isPreparingDownload;
            private set => SetProperty(ref _isPreparingDownload, value);
        }

        /// <summary>
        /// Gets the number of bytes already downloaded.
        /// </summary>
        public long DownloadedBytes
        {
            get => _downloadedBytes;
            private set => SetProperty(ref _downloadedBytes, value);
        }

        /// <summary>
        /// Gets the number of bytes to downloaded.
        /// </summary>
        public long BytesToDownload
        {
            get => _bytesToDownload;
            private set => SetProperty(ref _bytesToDownload, value);
        }

        /// <summary>
        /// Gets a value indicating whether a check for a new version is currently being executed.
        /// </summary>
        public bool IsChecking
        {
            get => _isChecking;
            private set => SetProperty(ref _isChecking, value);
        }

        /// <summary>
        /// Gets the changelog.
        /// </summary>
        public string Changelog
        {
            get => _changelog;
            private set => SetProperty(ref _changelog, value);
        }

        /// <summary>
        /// Gets a value indicating whether a new version is available.
        /// </summary>
        public bool IsUpdateAvailable
        {
            get => _isUpdateAvailable;
            private set => SetProperty(ref _isUpdateAvailable, value);
        }

        /// <summary>
        /// Gets the version information which were downloaded from the server.
        /// </summary>
        public Versions VersionsInformation
        {
            get => _versionsInformation;
            private set => SetProperty(ref _versionsInformation, value);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateController"/> class.
        /// </summary>
        /// <param name="rootUri">The root url to the remote server to retrieve version informations from.</param>
        public UpdateController(Uri rootUri)
        {
            RootUri = rootUri;
            _webClient = new WebClient();
        }

        /// <summary>
        /// Downloads the versions information from the remote server asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DownloadVersionsInformation()
        {
            while (_webClient.IsBusy)
                await Task.Delay(100);
            await Task.Run(() =>
            {
                VersionsInformation = XmlLoader.DownloadXml<Versions>(VersionsInformationUri, _webClient);
            });
        }

        /// <summary>
        /// Downloads the versions information asynchronously if necessary and saves them into the <see cref="VersionsInformation"/> property.
        /// </summary>
        /// <returns><c>true</c> if the versions information was retrieved; otherwise, <c>false</c>.</returns>
        private async Task<bool> EnsureVersionsInformationAvailable()
        {
            if (VersionsInformation == null)
                await DownloadVersionsInformation();
            return VersionsInformation != null;
        }

        /// <summary>
        /// Downloads the versions information asynchronously if necessary and checks whether the current application is the newest version.
        /// </summary>
        /// <param name="assembly">The assembly to check the version number on.</param>
        /// <param name="versionType">The type of version to check on the assembly.</param>
        /// <returns><c>true</c> if a new version is available; otherwise, <c>false</c>.</returns>
        public async Task<bool> CheckForUpdates(Assembly assembly, AssemblyVersionType versionType)
            => await CheckForUpdates(versionType.GetVersion(assembly));

        /// <summary>
        /// Downloads the versions information asynchronously if necessary and checks whether the current application is the newest version.
        /// </summary>
        /// <param name="currentVersion">The version number of the current application.</param>
        /// <returns><c>true</c> if a new version is available; otherwise, <c>false</c>.</returns>
        public async Task<bool> CheckForUpdates(string currentVersion)
        {
            IsChecking = true;
            bool result;
            try
            {
                try
                {
                    await DownloadVersionsInformation();
                }
                catch (WebException)
                {
                    IsChecking = false;
                    return false;
                }

                result = IsNewUpdateAvailable(currentVersion);
                if (result)
                {
                    var changelog = await GetCompleteChangelog(currentVersion);
                    Changelog = ChangelogToString(changelog);
                }
            }
            finally
            {
                IsChecking = false;
            }

            return result;
        }

        /// <summary>
        /// Downloads the changelog for the given version asynchronously.
        /// </summary>
        /// <param name="toVersion">The version for which the changelog should be downloaded.</param>
        /// <returns>The changelog for the given version.</returns>
        public async Task<string> GetSingleChangelog(string toVersion)
        {
            if (!await EnsureVersionsInformationAvailable())
                return null;
            if (toVersion != VersionsInformation.CurrentVersion && VersionsInformation.PreviousVersions.All(x => x.VersionNumber != toVersion))
                throw new ArgumentException("The version number " + toVersion + " does not exists!");
            return await DownloadChangelogForVersion(toVersion);
        }

        /// <summary>
        /// Downloads the complete changelog from the given version to the newest version asynchronously.
        /// </summary>
        /// <param name="currentVersion">The version from which the changelog should be downloaded.</param>
        /// <returns>A dirctionary of versions as key and the changelog as value.</returns>
        public async Task<Dictionary<string, string>> GetCompleteChangelog(string currentVersion)
        {
            if (!await EnsureVersionsInformationAvailable() || (!string.IsNullOrEmpty(currentVersion) && !IsNewUpdateAvailable(currentVersion)))
                return new Dictionary<string, string>();
            var changelog = new Dictionary<string, string>();
            var log = await DownloadChangelogForVersion(VersionsInformation.CurrentVersion);
            changelog.Add(VersionsInformation.CurrentVersion, log);
            foreach (var prevVersion in VersionsInformation.PreviousVersions)
            {
                if (prevVersion.VersionNumber == currentVersion)
                    break;
                log = await DownloadChangelogForVersion(prevVersion.VersionNumber);
                if (!changelog.ContainsKey(prevVersion.VersionNumber))
                    changelog.Add(prevVersion.VersionNumber, log);
            }

            return changelog;
        }

        /// <summary>
        /// Downloads all needed files for the update to the newest version asynchronously.
        /// </summary>
        /// <param name="assembly">The assembly to check the version number on.</param>
        /// <param name="versionType">The type of version to check on the assembly.</param>
        /// <returns>The names of the downloaded files and the directory in which they were stored to.</returns>
        public async Task<(string[] downloadedFiles, string targetDirectory)> DownloadAllNeededFiles(Assembly assembly, AssemblyVersionType versionType) => await DownloadAllNeededFiles(versionType.GetVersion(assembly));

        /// <summary>
        /// Downloads all needed files for the update to the newest version asynchronously.
        /// </summary>
        /// <param name="currentVersion">The version number of the current application.</param>
        /// <returns>The names of the downloaded files and the directory in which they were stored to.</returns>
        public async Task<(string[] downloadedFiles, string targetDirectory)> DownloadAllNeededFiles(string currentVersion)
        {
            if (IsDownloading)
                throw new NotSupportedException("The Download is running!");
            IsPreparingDownload = true;
            _wasCanceled = false;
            var filesToDownload = GetFilesToDownload(currentVersion);
            var localDownloadedFiles = new List<string>();
            var targetDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            long totalByteSize = 0;
            long downloadedBytes = 0;
            long downloadedBytesAfterDownload = 0;
            var byteSizes = new Dictionary<Uri, long>();
            foreach (var file in filesToDownload)
            {
                using (await _webClient.OpenReadTaskAsync(file))
                {
                    long byteSize = Convert.ToInt64(_webClient.ResponseHeaders["Content-Length"]);
                    byteSizes.Add(file, byteSize);
                    totalByteSize += byteSize;
                }
            }

            BytesToDownload = totalByteSize;

            void DownloadPropertyChanged(object s, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "DownloadSpeed" && s is Download download)
                    DownloadSpeed = download.DownloadSpeed;
            }

            void DownloadProgressChanged(object s, DownloadProgressChangedEventArgs e)
            {
                DownloadedBytes = downloadedBytes + e.BytesReceived;
                Progress = (float)((double)(downloadedBytes + e.BytesReceived) / totalByteSize);
            }

            IsPreparingDownload = false;
            IsDownloading = true;
            try
            {
                foreach (var file in byteSizes)
                {
                    CurrentDownloadedFile = file.Key.MakeRelativeUri(RootUri).ToString();

                    var dl = new Download(RootUri, file.Key, _webClient, targetDir);
                    dl.PropertyChanged += DownloadPropertyChanged;
                    dl.DownloadProgressChanged += DownloadProgressChanged;
                    await dl.StartDownload();
                    dl.PropertyChanged -= DownloadPropertyChanged;
                    dl.DownloadProgressChanged -= DownloadProgressChanged;

                    DownloadedBytes = downloadedBytes = downloadedBytesAfterDownload += file.Value;
                    Progress = (float)((double)downloadedBytes / totalByteSize);
                    localDownloadedFiles.Add(dl.LocalFilePath);

                    if (_wasCanceled)
                    {
                        if (Directory.Exists(targetDir))
                            Directory.Delete(targetDir, true);
                        localDownloadedFiles = new List<string>();
                        break;
                    }
                }

                DownloadedBytes = BytesToDownload;
                DownloadSpeed = 0;
                Progress = 1;
            }
            finally
            {
                IsDownloading = false;
                DownloadedBytes = 0;
                BytesToDownload = 0;
                DownloadSpeed = 0;
                Progress = 0;
            }

            HasFinished = true;

            return (localDownloadedFiles.ToArray(), targetDir);
        }

        /// <summary>
        /// Cancels the current download.
        /// </summary>
        public void CancelDownload()
        {
            if (_webClient.IsBusy)
                _webClient.CancelAsync();
            _wasCanceled = true;
        }

        private bool IsNewUpdateAvailable(string currentVersion)
        {
            if (!EnsureVersionsInformationAvailable().Result)
                return false;
            IsUpdateAvailable = new Version(VersionsInformation.CurrentVersion) > new Version(currentVersion);
            return IsUpdateAvailable;
        }

        private IEnumerable<Uri> GetFilesToDownload(string currentVersion)
        {
            if (!IsNewUpdateAvailable(currentVersion))
                return Array.Empty<Uri>();

            var filesToDownload = new List<string> { VersionsInformation.SetupPath };
            foreach (var prevVersion in VersionsInformation.PreviousVersions)
            {
                if (prevVersion.VersionNumber == currentVersion)
                {
                    filesToDownload.AddRange(prevVersion.SpecialTasks.Select(x => x.FileToRun));
                    break;
                }

                if (prevVersion.SpecialTasks != null)
                    filesToDownload.AddRange(prevVersion.SpecialTasks.Where(x => x.RunForPrevious).Select(x => x.FileToRun));
            }

            return filesToDownload.Select(x => new Uri(RootUri, new Uri(x, UriKind.Relative))).ToArray();
        }

        private async Task<string> DownloadChangelogForVersion(string version)
        {
            try
            {
                return await _webClient.DownloadStringTaskAsync(new Uri(ChangeLogsUri, $"{version}.txt"));
            }
            catch (Exception ex)
            {
                return $"[Error getting change log: {ex.Message}]";
            }
        }

        /// <summary>
        /// Converts a changelog dictionary (which is retrieved by the <see cref="GetCompleteChangelog"/> method) to a readable string.
        /// </summary>
        /// <param name="changelog">The changelog to convert.</param>
        /// <returns>A readable string which represents the given changelog.</returns>
        public static string ChangelogToString(Dictionary<string, string> changelog)
        {
            var sb = new StringBuilder();
            foreach (var change in changelog)
            {
                sb.AppendFormat("{0}:\n{1}\n\n", change.Key, change.Value);
            }

            return sb.ToString();
        }
    }
}
