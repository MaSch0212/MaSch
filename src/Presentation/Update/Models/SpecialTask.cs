using System.Xml.Serialization;
using MaSch.Common.Observable;

namespace MaSch.Presentation.Update.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a special task to execute on an update.
    /// </summary>
    public class SpecialTask : ObservableObject
    {
        private string _fileToRun;
        private bool _runForPrevious;

        /// <summary>
        /// The file to run for the special task.
        /// </summary>
        [XmlAttribute("Run")]
        public string FileToRun
        {
            get => _fileToRun;
            set => SetProperty(ref _fileToRun, value);
        }
        /// <summary>
        /// To be defined.
        /// </summary>
        [XmlAttribute]
        public bool RunForPrevious
        {
            get => _runForPrevious;
            set => SetProperty(ref _runForPrevious, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialTask"/> class.
        /// </summary>
        public SpecialTask()
        {
            FileToRun = string.Empty;
            RunForPrevious = false;
        }
    }
}
