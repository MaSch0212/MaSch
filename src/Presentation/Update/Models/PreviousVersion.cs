using System.Collections.ObjectModel;
using System.Xml.Serialization;
using MaSch.Common.Observable;

namespace MaSch.Presentation.Update.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a previous version.
    /// </summary>
    public class PreviousVersion : ObservableObject
    {
        private string _versionNumber;
        private ObservableCollection<SpecialTask> _specialTasks;
        
        /// <summary>
        /// The version number of this version.
        /// </summary>
        [XmlAttribute("Number")]
        public string VersionNumber
        {
            get => _versionNumber;
            set => SetProperty(ref _versionNumber, value);
        }
        /// <summary>
        /// The special tasks to execute when update to the next version.
        /// </summary>
        [XmlArray("SpecialTasks")]
        public ObservableCollection<SpecialTask> SpecialTasks
        {
            get => _specialTasks;
            set => SetProperty(ref _specialTasks, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviousVersion"/> class.
        /// </summary>
        public PreviousVersion()
        {
            VersionNumber = "0.0.0.0";
            SpecialTasks = new ObservableCollection<SpecialTask>();
        }
    }
}
