using System.Xml.Serialization;
using MaSch.Core.Observable;

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
        /// Gets or sets the file to run for the special task.
        /// </summary>
        [XmlAttribute("Run")]
        public string FileToRun
        {
            get => _fileToRun;
            set => SetProperty(ref _fileToRun, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this special task should be run for future updates as well.
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
