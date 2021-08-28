using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace MaSch.Console.Controls
{
    /// <summary>
    /// Control for a <see cref="IConsoleService"/> with which the progress of an action can be displayed.
    /// </summary>
    public class ProgressControl : IDisposable
    {
        private const int ProgressBarLeftMargin = 3;
        private const int ProgressBarRightMargin = 6;
        private const int TotalProgressBarMarginAndPadding = ProgressBarLeftMargin + ProgressBarRightMargin + 2;

        private readonly IConsoleService _console;
        private bool _showStatusText;

        private int? _currentPosition;
        private int? _currentX;
        private int? _currentProgressBarWidth;
        private int? _currentWidth;
        private bool? _currentUseOneLineOnly;
        private ConsoleColor? _currentProgressBarBorderColor;
        private ConsoleColor? _currentProgressBarColor;

        private ProgressControlStatus? _lastStatus;
        private int _lastIndicatorIndex = -1;
        private string? _lastStatusText;
        private ConsoleColor? _lastStatusTextColor;
        private int? _lastProgressChars;
        private int? _lastIndeterminate;
        private int _lastIndeterminateDirection = 1;
        private double? _lastProgress;
        private ConsoleColor? _lastProgressColor;

        private Task? _renderTask;
        private CancellationTokenSource? _renderCancellationSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressControl" /> class.
        /// </summary>
        /// <param name="console">The console on which the progress should be shown.</param>
        public ProgressControl(IConsoleService console)
        {
            _console = Guard.NotNull(console, nameof(console));

            IndicatorChars = _console.IsFancyConsole
                ? new[] { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" }
                : new[] { "\\", "|", "/", "-" };

            /* Other formats:
             "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏"
             "▀", "■", "▄", "■", "▀", "■", "▄", "■", "▀", "■", "▄", "■"
             "\\", "|", "/", "-", "\\", "|", "/", "-", "\\", "|", "/", "-"
             "🕐", "🕑", "🕒", "🕓", "🕔", "🕕", "🕖", "🕗", "🕘", "🕙", "🕚", "🕛"
             */
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ProgressControl"/> class.
        /// </summary>
        ~ProgressControl()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets a value indicating whether this control is visible.
        /// </summary>
        public bool IsVisible { get; private set; }

        /// <summary>
        /// Gets or sets the x position of this control.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the width of this control.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the width of the progress bar inside this control.
        /// </summary>
        public int? ProgressBarWidth { get; set; }

        /// <summary>
        /// Gets or sets the status of this control.
        /// </summary>
        public ProgressControlStatus Status { get; set; } = ProgressControlStatus.NotStarted;

        /// <summary>
        /// Gets or sets the time in miliseconds between the renders.
        /// </summary>
        public int RenderWaitTime { get; set; } = 100;

        /// <summary>
        /// Gets or sets a value indicating whether the control should only use one console row.
        /// </summary>
        public bool UseOneLineOnly { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the status text should be shown.
        /// </summary>
        public bool ShowStatusText
        {
            get => _showStatusText;
            set
            {
                if (IsVisible && _showStatusText && !value)
                {
                    using (ConsoleSynchronizer.Scope())
                    using (new ConsoleScope(_console, false, true, false))
                    {
                        _console.CursorPosition.Point = new Point(_currentX!.Value, _currentPosition!.Value + 1);
                        _console.Write(new string(' ', _currentWidth!.Value));
                    }
                }

                _showStatusText = value;
            }
        }

        /// <summary>
        /// Gets or sets the progress of this control.
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        /// Gets or sets the status text of this control.
        /// </summary>
        public string? StatusText { get; set; }

        /// <summary>
        /// Gets the maximum text length that can be displayed in this control.
        /// </summary>
        public int MaxStatusTextLength
        {
            get
            {
                var x = _currentX ?? X;
                var width = _currentWidth ?? (_console.BufferSize.Width - x - 1);
                var useOneLineOnly = _currentUseOneLineOnly ?? UseOneLineOnly;
                var progressBarWidth = _currentProgressBarWidth ?? ProgressBarWidth ?? (width - TotalProgressBarMarginAndPadding) / 2;

                return width - ProgressBarLeftMargin - 2 -
                    (useOneLineOnly ? progressBarWidth - ProgressBarRightMargin - 1 : 0);
            }
        }

        /// <summary>
        /// Gets or sets the color of the status indicator.
        /// </summary>
        public ConsoleColor IndicatorColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Gets or sets the color of the status text.
        /// </summary>
        public ConsoleColor StatusTextColor { get; set; } = ConsoleColor.Gray;

        /// <summary>
        /// Gets or sets the color of the progress bar border.
        /// </summary>
        public ConsoleColor ProgressBarBorderColor { get; set; } = ConsoleColor.Blue;

        /// <summary>
        /// Gets or sets the color of the progress bar.
        /// </summary>
        public ConsoleColor ProgressBarColor { get; set; } = ConsoleColor.Cyan;

        /// <summary>
        /// Gets or sets the color of the progress percentage.
        /// </summary>
        public ConsoleColor ProgressColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Gets or sets the indicator characters that are used for the animation when status <see cref="ProgressControlStatus.Loading"/> is set.
        /// </summary>
        public string[] IndicatorChars { get; set; }

        /// <summary>
        /// Shows this control. Please make sure to reserve enough space in the console buffer before executing this method.
        /// </summary>
        public void Show()
        {
            if (IsVisible)
                Hide(true);

            _currentPosition = _console.CursorPosition.Y;
            _currentX = X;
            _currentWidth = Math.Min(Width ?? int.MaxValue, _console.BufferSize.Width - X - 1);
            _currentProgressBarWidth = Math.Min(ProgressBarWidth ?? int.MaxValue, _currentWidth.Value - TotalProgressBarMarginAndPadding);
            if (UseOneLineOnly && ProgressBarWidth == null)
                _currentProgressBarWidth /= 2;
            _currentUseOneLineOnly = UseOneLineOnly;
            _currentProgressBarBorderColor = ProgressBarBorderColor;
            _currentProgressBarColor = ProgressBarColor;

            _lastStatus = null;
            _lastIndicatorIndex = -1;
            _lastStatusText = null;
            _lastStatusTextColor = null;
            _lastProgressChars = null;
            _lastProgress = null;
            _lastProgressColor = null;

            DrawInitial();
            IsVisible = true;
            _renderCancellationSource?.Dispose();
            _renderCancellationSource = new CancellationTokenSource();
            _renderTask = RunRender();
        }

        /// <summary>
        /// Hides this control.
        /// </summary>
        /// <param name="setCursorToControlRoot">if set to <c>true</c> the cursor is set to the root of the control.</param>
        public void Hide(bool setCursorToControlRoot) => Hide(setCursorToControlRoot, false);

        /// <summary>
        /// Hides this control.
        /// </summary>
        /// <param name="setCursorToControlRoot">if set to <c>true</c> the cursor is set to the root of the control.</param>
        /// <param name="skipConsoleSynchronization">if set to <c>true</c> no new console synchronization scope is created.</param>
        public void Hide(bool setCursorToControlRoot, bool skipConsoleSynchronization)
        {
            StopRender();

            var scope = skipConsoleSynchronization ? null : ConsoleSynchronizer.Scope();
            try
            {
                using (new ConsoleScope(_console, false, true, false))
                {
                    _console.CursorPosition.Point = new Point(_currentX!.Value, _currentPosition!.Value);
                    _console.Write(new string(' ', _currentWidth!.Value));
                    if (ShowStatusText && !_currentUseOneLineOnly!.Value)
                    {
                        _console.CursorPosition.Point = new Point(_currentX!.Value, _currentPosition!.Value + 1);
                        _console.Write(new string(' ', _currentWidth!.Value));
                    }
                }

                if (setCursorToControlRoot)
                    _console.CursorPosition.Point = new Point(_currentX!.Value, _currentPosition!.Value);
            }
            finally
            {
                scope?.Dispose();
            }

            IsVisible = false;
        }

        /// <summary>
        /// Stops the rendering without hiding this control.
        /// </summary>
        public void StopRender()
        {
            _renderCancellationSource?.Cancel();
            _renderCancellationSource?.Dispose();
            _renderTask?.Wait();
            _renderCancellationSource = null;
            _renderTask = null;
        }

        /// <summary>
        /// Renders this control once.
        /// </summary>
        public void Render()
        {
            if (!IsVisible)
                Show();

            using (ConsoleSynchronizer.Scope())
            using (new ConsoleScope(_console, false, true, false))
                OnRender();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _renderCancellationSource?.Dispose();
            }
        }

        private async Task RunRender()
        {
            try
            {
                while (!_renderCancellationSource!.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(RenderWaitTime, _renderCancellationSource.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        return;
                    }

                    using (ConsoleSynchronizer.Scope())
                    using (new ConsoleScope(_console, false, true, false))
                    {
                        if (_renderCancellationSource!.IsCancellationRequested)
                            return;
                        OnRender();
                    }
                }
            }
            catch
            {
                try
                {
                    await Task.Delay(250, _renderCancellationSource!.Token);
                }
                catch (TaskCanceledException)
                {
                    return;
                }

                Task.Run(() =>
                {
                    IsVisible = false;
                    Show();
                }).Forget();
            }
        }

        private void OnRender()
        {
            var indicatorChars = IndicatorChars;
            int indicatorStart = _currentX!.Value + 1;
            int statusTextStart = _currentX!.Value + ProgressBarLeftMargin;
            var maxTextLength = _currentUseOneLineOnly!.Value ? _currentWidth!.Value - _currentProgressBarWidth!.Value - ProgressBarRightMargin - ProgressBarLeftMargin - 1 : _currentWidth!.Value - statusTextStart;
            int progressBarStart = _currentUseOneLineOnly!.Value ? statusTextStart + maxTextLength + 2 : _currentX!.Value + ProgressBarLeftMargin + 1;
            var percentageStart = progressBarStart + _currentProgressBarWidth!.Value;
            var progressBarWidth = _currentProgressBarWidth!.Value - 2;

            _console.CursorPosition.Y = _currentPosition!.Value;
            if (Status != _lastStatus || Status == ProgressControlStatus.Loading)
            {
                _console.CursorPosition.X = indicatorStart;
                switch (Status)
                {
                    case ProgressControlStatus.Loading:
                        _lastIndicatorIndex = (_lastIndicatorIndex + 1) % indicatorChars.Length;
                        _console.WriteWithColor(indicatorChars[_lastIndicatorIndex], IndicatorColor);
                        break;
                    case ProgressControlStatus.Succeeeded:
                        _console.WriteWithColor(" ", ConsoleColor.White, ConsoleColor.Green);
                        break;
                    case ProgressControlStatus.PartiallySucceeded:
                        _console.WriteWithColor(" ", ConsoleColor.White, ConsoleColor.Yellow);
                        break;
                    case ProgressControlStatus.Failed:
                        _console.WriteWithColor(" ", ConsoleColor.White, ConsoleColor.Red);
                        break;
                    default:
                        _console.Write(" ");
                        break;
                }
            }

            _lastStatus = Status;

            if ((double.IsNaN(Progress) && _lastProgress.HasValue && !double.IsNaN(_lastProgress.Value)) ||
                (!double.IsNaN(Progress) && _lastProgress.HasValue && double.IsNaN(_lastProgress.Value)))
            {
                // Cleanup progress bar
                _console.CursorPosition.X = progressBarStart;
                _console.Write(new string(' ', progressBarWidth));
                _console.CursorPosition.X = percentageStart;
                _console.Write(new string(' ', 5));
                _lastProgressChars = null;
            }

            if (double.IsNaN(Progress))
            {
                var lip = _lastIndeterminate ?? 0;
                var nlip = lip + _lastIndeterminateDirection;
                if (nlip < 0)
                {
                    nlip += 2;
                    _lastIndeterminateDirection = 1;
                }
                else if (nlip >= progressBarWidth)
                {
                    nlip -= 2;
                    _lastIndeterminateDirection = -1;
                }

                _console.CursorPosition.X = progressBarStart + Math.Min(lip, nlip);
                _console.WriteWithColor(lip < nlip ? " ■" : "■ ", _currentProgressBarColor!.Value);
                _lastIndeterminate = nlip;
            }
            else
            {
                var progressChars = (int)Math.Round(Progress * progressBarWidth, 0);
                var lpc = _lastProgressChars ?? 0;
                if (progressChars != lpc)
                {
                    _console.CursorPosition.X = progressBarStart + Math.Min(progressChars, lpc);
                    _console.WriteWithColor(new string(progressChars > lpc ? '■' : ' ', Math.Abs(progressChars - lpc)), _currentProgressBarColor!.Value);
                }

                _lastProgressChars = progressChars;

                var lp = _lastProgress.HasValue ? (double.IsNaN(_lastProgress.Value) ? -1 : _lastProgress!.Value) : 0;
                if ((int)Math.Round(Progress * 100, 0) != (int)Math.Round(lp * 100, 0) || _lastProgressColor != ProgressColor)
                {
                    _console.CursorPosition.X = percentageStart;
                    _console.WriteWithColor($"{Progress,5:P0}", ProgressColor);
                    _lastProgressColor = ProgressColor;
                }
            }

            _lastProgress = Progress;

            if (ShowStatusText && (_lastStatusText != StatusText || _lastStatusTextColor != StatusTextColor))
            {
                _console.CursorPosition.Point = new Point(statusTextStart, _currentUseOneLineOnly!.Value ? _currentPosition!.Value : _currentPosition!.Value + 1);
                var text = (StatusText ?? string.Empty).PadRight(maxTextLength);
                if (text.Length > maxTextLength)
                    text = text.Substring(0, maxTextLength - 3) + "...";
                _console.WriteWithColor(text, StatusTextColor);
                _lastStatusText = StatusText;
                _lastStatusTextColor = StatusTextColor;
            }
        }

        private void DrawInitial()
        {
            using (ConsoleSynchronizer.Scope())
            {
                using (new ConsoleScope(_console, false, true, false))
                {
                    _console.CursorPosition.Point = new Point(_currentX!.Value, _currentPosition!.Value);
                    _console.Write(new string(' ', ProgressBarLeftMargin));
                    if (ShowStatusText && _currentUseOneLineOnly!.Value)
                        _console.Write(new string(' ', _currentWidth!.Value - _currentProgressBarWidth!.Value - ProgressBarRightMargin - ProgressBarLeftMargin));
                    _console.WriteWithColor("[" + new string(' ', _currentProgressBarWidth!.Value - 2) + "]" + new string(' ', ProgressBarRightMargin), _currentProgressBarBorderColor!.Value);
                    if (ShowStatusText && !_currentUseOneLineOnly!.Value)
                    {
                        _console.CursorPosition.Point = new Point(_currentX!.Value, _currentPosition!.Value + 1);
                        _console.Write(new string(' ', _currentWidth!.Value));
                    }

                    OnRender();
                }

                _console.Write(new string('\n', ShowStatusText && !_currentUseOneLineOnly!.Value ? 2 : 1));
            }
        }
    }
}
