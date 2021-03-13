using MaSch.Core;
using MaSch.Core.Attributes;
using MaSch.Core.Observable;
using MaSch.Update.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace MaSch.Update
{
    /// <summary>
    /// Property definition for class <see cref="UpdateController"/>.
    /// </summary>
    [ObservablePropertyDefinition]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Property definition")]
    internal interface IUpdateController_Props
    {
        /// <summary>
        /// Gets the current download.
        /// </summary>
        UpdateDownload CurrentDownload { get; }

        /// <summary>
        /// Gets the current version information.
        /// </summary>
        VersionInfo? CurrentVersionInfo { get; }
    }

    /// <summary>
    /// Class to control updates of an application.
    /// </summary>
    /// <seealso cref="MaSch.Core.Observable.ObservableObject" />
    /// <seealso cref="System.IDisposable" />
    public sealed partial class UpdateController : ObservableObject, IUpdateController_Props, IDisposable
    {
        private readonly Func<string?, bool> _versionComparerFunc;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Gets the version information URI.
        /// </summary>
        public Uri VersionInfoUri { get; }

        /// <summary>
        /// Gets a value indicating whether an update for the application is available.
        /// </summary>
        [DependsOn(nameof(CurrentVersionInfo))]
        public bool IsUpdateAvailable => _versionComparerFunc(CurrentVersionInfo?.CurrentVersion);

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateController"/> class.
        /// </summary>
        /// <param name="versionInfoUri">The version information URI.</param>
        public UpdateController(Uri versionInfoUri)
            : this(versionInfoUri, AssemblyVersionType.AssemblyVersion)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateController"/> class.
        /// </summary>
        /// <param name="versionInfoUri">The version information URI.</param>
        /// <param name="assemblyVersionType">Type of assembly version to use for comparing version numbers.</param>
        public UpdateController(Uri versionInfoUri, AssemblyVersionType assemblyVersionType)
            : this(versionInfoUri, GetDefaultVersionComparer(assemblyVersionType))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateController"/> class.
        /// </summary>
        /// <param name="versionInfoUri">The version information URI.</param>
        /// <param name="versionComparerFunc">The version comparer function that determines wether a version is newer than the current version.</param>
        public UpdateController(Uri versionInfoUri, Func<string?, bool> versionComparerFunc)
        {
            _versionComparerFunc = versionComparerFunc;
            VersionInfoUri = versionInfoUri;

            _httpClient = new HttpClient();
        }

        public async Task<bool> CheckForUpdatesAsync()
        {
            var info = await GetVersionInfoAsync();
            return _versionComparerFunc(info?.CurrentVersion);
        }

        public async Task<VersionInfo?> GetVersionInfoAsync()
        {
            var strVersionInfo = await _httpClient.GetStringAsync(VersionInfoUri);
            return CurrentVersionInfo = JsonSerializer.Deserialize<VersionInfo>(strVersionInfo);
        }

        public async Task<UpdateDownload> StartDownloadFile()
            => await StartDownloadFile(null!, null!);

        public async Task<UpdateDownload> StartDownloadFile(string runtime)
            => await StartDownloadFile(null!, runtime);

        public async Task<UpdateDownload> StartDownloadFile(VersionInfo versionInfo, string runtime)
        {
            var result = await CreateDownload(versionInfo, runtime);
            result.Start();
            return result;
        }

        public async Task<bool> DownloadFileAsync()
            => await (await CreateDownload(null, null)).ExecuteAsync();

        public async Task<bool> DownloadFileAsync(string runtime)
            => await (await CreateDownload(null, runtime)).ExecuteAsync();

        public async Task<bool> DownloadFileAsync(VersionInfo versionInfo, string runtime)
            => await (await CreateDownload(versionInfo, runtime)).ExecuteAsync();

        public void ExecuteFile(string file)
        {
            // TODO
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private async Task<UpdateDownload> CreateDownload(VersionInfo? versionInfo, string? runtime)
        {
            versionInfo ??= CurrentVersionInfo ?? await GetVersionInfoAsync();
            if (versionInfo == null)
                throw new IOException("Could not retrieve any version info.");

            var fileUri = new Uri(runtime != null && versionInfo.Files.TryGetValue(runtime, out var fn) ? fn : versionInfo.Files[versionInfo.DefaultRuntime!]);
            var fileName = await GetFileName(fileUri);
            var targetFilePath = Path.Combine(Path.GetTempPath(), fileName);
            var result = new UpdateDownload(fileUri, targetFilePath);
            return CurrentDownload = result;
        }

        private static Func<string?, bool> GetDefaultVersionComparer(AssemblyVersionType assemblyVersionType)
        {
            var strCurrentVersion = assemblyVersionType.GetVersion();
            if (!Version.TryParse(strCurrentVersion, out var currentVersion))
                throw new IOException($"Could not read version from entry assembly. (assemblyVersionType: {assemblyVersionType}, version: {strCurrentVersion ?? "(null)"})");

            return (string? strVersion) =>
            {
                if (!Version.TryParse(strVersion, out var version))
                    return false;
                return version > currentVersion;
            };
        }

        private static async Task<string> GetFileName(Uri sourceUri)
        {
            var request = WebRequest.Create(sourceUri);
            request.Method = "HEAD";
            using (var response = await request.GetResponseAsync())
            {
                var contentDisposition = response.Headers["content-disposition"];
                if (contentDisposition != null)
                {
                    var elements = (from s in contentDisposition.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                    let s2 = s.Split('=', StringSplitOptions.None)
                                    let value = s2.Length < 2 ? null : s2[1].Trim().Trim('"', '\'')
                                    select (Key: s2[0].Trim().ToLowerInvariant(), Value: value)).ToDictionary(x => x.Key, x => x.Value);
                    if (elements.TryGetValue("filename*", out var fileNameStar))
                        return fileNameStar;
                    else if (elements.TryGetValue("filename", out var fileName))
                        return fileName;
                }

                return Path.GetFileName(response.ResponseUri.ToString());
            }
        }
    }
}