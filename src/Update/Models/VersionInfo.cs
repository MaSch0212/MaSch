using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace MaSch.Update.Models
{
    public class VersionInfo
    {
        public string? DisplayName { get; }
        public string? CurrentVersion { get; }
        public string? DefaultRuntime { get; }
        public string DefaultLanguage { get; }
        public IReadOnlyDictionary<string, string> Files { get; }
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> ReleaseNotes { get; }
        public IReadOnlyDictionary<string, string> ReleaseNotesUrl { get; }

        [JsonConstructor]
        public VersionInfo(
            string? displayName,
            string? currentVersion,
            string? defaultRuntime,
            string? defaultLanguage,
            IReadOnlyDictionary<string?, string?>? files,
            IReadOnlyDictionary<string?, IReadOnlyDictionary<string?, string?>?>? releaseNotes,
            IReadOnlyDictionary<string?, string?> releaseNotesUrl)
        {
            DisplayName = displayName;
            CurrentVersion = currentVersion;
            DefaultRuntime = defaultRuntime;
            DefaultLanguage = defaultLanguage ?? "en";

            Files = new ReadOnlyDictionary<string, string>(
                files?
                    .Where(x => x.Key != null && x.Value != null)
                    .ToDictionary(x => x.Key!, x => x.Value!)
                ?? new Dictionary<string, string>());

            ReleaseNotes = new ReadOnlyDictionary<string, IReadOnlyDictionary<string, string>>(
                releaseNotes?
                    .Where(x => x.Key != null && x.Value != null)
                    .ToDictionary(x => x.Key!, x => (IReadOnlyDictionary<string, string>)new ReadOnlyDictionary<string, string>(
                        x.Value!
                            .Where(x => x.Key != null && x.Value != null)
                            .ToDictionary(x => x.Key!, x => x.Value!)))
                ?? new Dictionary<string, IReadOnlyDictionary<string, string>>());

            ReleaseNotesUrl = new ReadOnlyDictionary<string, string>(
                releaseNotesUrl?
                    .Where(x => x.Key != null && x.Value != null)
                    .ToDictionary(x => x.Key!, x => x.Value!)
                ?? new Dictionary<string, string>());
        }
    }
}
