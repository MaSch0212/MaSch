using MaSch.Common;
using MaSch.Common.Extensions;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace MaSch.Console.Controls
{
    public enum ProgressControlStatus
    {
        NotStarted,
        Loading,
        Succeeeded,
        PartiallySucceeded,
        Failed
    }

    public class ProgressControl
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

        public bool IsVisible { get; private set; }
        public int X { get; set; }
        public int? Width { get; set; }
        public int? ProgressBarWidth { get; set; }
        public bool ShowStatusText
        {
            get => _showStatusText;
            set
            {
                if (IsVisible && _showStatusText == true && !value)
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
        public ProgressControlStatus Status { get; set; } = ProgressControlStatus.NotStarted;
        public int RenderWaitTime { get; set; } = 100;
        public bool UseOneLineOnly { get; set; } = true;

        public double Progress { get; set; }
        public string? StatusText { get; set; }
        public int MaxStatusTextLength => (_currentWidth ?? (_console.BufferSize.Width - (_currentX ?? X) - 1)) - ProgressBarLeftMargin - 2 - ((_currentUseOneLineOnly ?? UseOneLineOnly) ? (_currentProgressBarWidth ?? ProgressBarWidth ?? ((_currentWidth!.Value - TotalProgressBarMarginAndPadding) / 2)) - ProgressBarRightMargin - 1 : 0);
        public ConsoleColor IndicatorColor { get; set; } = ConsoleColor.White;
        public ConsoleColor StatusTextColor { get; set; } = ConsoleColor.Gray;
        public ConsoleColor ProgressBarBorderColor { get; set; } = ConsoleColor.Blue;
        public ConsoleColor ProgressBarColor { get; set; } = ConsoleColor.Cyan;
        public ConsoleColor ProgressColor { get; set; } = ConsoleColor.White;

        public string[] IndicatorChars { get; set; }


        public ProgressControl(IConsoleService console)
        {
            _console = Guard.NotNull(console, nameof(console));

            IndicatorChars = _console.IsFancyConsole
                ? new[] { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" }
                : new[] { "\\", "|", "/", "-" };

            //new[] { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };
            // new[] { "▀", "■", "▄", "■", "▀", "■", "▄", "■", "▀", "■", "▄", "■" };
            // new[] { "\\", "|", "/", "-", "\\", "|", "/", "-", "\\", "|", "/", "-" };
            /*_console.IsFancyConsole
                ? new[] { "🕐", "🕑", "🕒", "🕓", "🕔", "🕕", "🕖", "🕗", "🕘", "🕙", "🕚", "🕛" }
                : */
        }

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
            _renderCancellationSource = new CancellationTokenSource();
            _renderTask = RunRender();
        }

        public void Hide(bool setCursorToControlRoot) => Hide(setCursorToControlRoot, false);
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

        public void StopRender()
        {
            _renderCancellationSource?.Cancel();
            _renderTask?.Wait();
            _renderCancellationSource = null;
            _renderTask = null;
        }

        public void Render()
        {
            if (!IsVisible)
                Show();

            using (ConsoleSynchronizer.Scope())
            using (new ConsoleScope(_console, false, true, false))
                OnRender();
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
                    catch (TaskCanceledException) { return; }

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
                catch (TaskCanceledException) { return; }
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
                        _console.WriteWithColor(indicatorChars[_lastIndicatorIndex = (_lastIndicatorIndex + 1) % indicatorChars.Length], IndicatorColor);
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

            if (double.IsNaN(Progress) && _lastProgress.HasValue && !double.IsNaN(_lastProgress.Value) || !double.IsNaN(Progress) && _lastProgress.HasValue && double.IsNaN(_lastProgress.Value))
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
