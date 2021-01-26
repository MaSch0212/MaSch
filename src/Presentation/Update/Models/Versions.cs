using System.Collections.ObjectModel;
using System.Xml.Serialization;
using MaSch.Common.Observable;

namespace MaSch.Presentation.Update.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Contains informations for all versions.
    /// </summary>
    public class Versions : ObservableObject
    {
        private string _currentVersion;
        private string _setupPath;
        private ObservableCollection<PreviousVersion> _previousVersions;

        /// <summary>
        /// Determines the latest version of the application.
        /// </summary>
        [XmlElement]
        public string CurrentVersion
        {
            get => _currentVersion;
            set => SetProperty(ref _currentVersion, value);
        }
        /// <summary>
        /// Determines the path to the setup file for the latest version.
        /// </summary>
        [XmlElement]
        public string SetupPath
        {
            get => _setupPath;
            set => SetProperty(ref _setupPath, value);
        }
        /// <summary>
        /// The previous versions.
        /// </summary>
        [XmlArray("PreviousVersions")]
        [XmlArrayItem("Version")]
        public ObservableCollection<PreviousVersion> PreviousVersions
        {
            get => _previousVersions;
            set => SetProperty(ref _previousVersions, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Versions"/> class.
        /// </summary>
        public Versions()
        {
            CurrentVersion = "0.0.0.0";
            PreviousVersions = new ObservableCollection<PreviousVersion>();
        }
    }
}
