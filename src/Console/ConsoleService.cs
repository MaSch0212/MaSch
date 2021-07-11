using MaSch.Console.Ansi;
using MaSch.Core.Lazy;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using C = System.Console;

namespace MaSch.Console
{
    /// <summary>
    /// Implementation of the <see cref="IConsoleService"/> interface wrapping the <see cref="System.Console"/> class.
    /// </summary>
    /// <seealso cref="IConsoleService" />
    public class ConsoleService : IConsoleService
    {
        /// <inheritdoc />
        public bool IsFancyConsole { get; }

        /// <inheritdoc />
        public bool IsAnsiEscapeTrimmingEnabled { get; }

        /// <inheritdoc />
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "This is already verified by the ConsoleSize class.")]
        public ConsoleSize BufferSize { get; } = new ConsoleSize(() => C.BufferWidth, x => C.BufferWidth = x, () => C.BufferHeight, x => C.BufferHeight = x, false);

        /// <inheritdoc />
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "This is already verified by the ConsoleSize class.")]
        public ConsoleSize WindowSize { get; } = new ConsoleSize(() => C.WindowWidth, x => C.WindowWidth = x, () => C.WindowHeight, x => C.WindowHeight = x, false);

        /// <inheritdoc />
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "This is already verified by the ConsolePoint class.")]
        public ConsolePoint WindowPosition { get; } = new ConsolePoint(() => C.WindowLeft, x => C.WindowLeft = x, () => C.WindowTop, x => C.WindowTop = x, false);

        /// <inheritdoc />
        public LazySize LargestWindowSize { get; } = new LazySize(() => C.LargestWindowWidth, () => C.LargestWindowHeight, false);

        /// <inheritdoc />
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "False positive for some reason...")]
        public string WindowTitle
        {
            [SupportedOSPlatform("windows")]
            get => C.Title;
            set => C.Title = value;
        }

        /// <inheritdoc />
        public ModifiableLazyPoint CursorPosition { get; } = new ModifiableLazyPoint(() => C.CursorLeft, x => C.CursorLeft = x, () => C.CursorTop, x => C.CursorTop = x, false);

        /// <inheritdoc />
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "False positive for some reason...")]
        public int CursorSize
        {
            get => C.CursorSize;
            [SupportedOSPlatform("windows")]
            set => C.CursorSize = value;
        }

        /// <inheritdoc />
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "False positive for some reason...")]
        public bool IsCursorVisible
        {
            [SupportedOSPlatform("windows")]
            get => C.CursorVisible;
            set => C.CursorVisible = value;
        }

        /// <inheritdoc />
        public TextWriter ErrorStream
        {
            get => C.Error;
            set => C.SetError(value);
        }

        /// <inheritdoc />
        public bool IsErrorRedirected => C.IsErrorRedirected;

        /// <inheritdoc />
        public TextReader InputStream
        {
            get => C.In;
            set => C.SetIn(value);
        }

        /// <inheritdoc />
        public Encoding InputEncoding
        {
            get => C.InputEncoding;
            set => C.InputEncoding = value;
        }

        /// <inheritdoc />
        public bool IsInputRedirected => C.IsInputRedirected;

        /// <inheritdoc />
        public bool IsKeyAvailable => C.KeyAvailable;

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public bool IsCapsLockEnabled => C.CapsLock;

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public bool IsNumberLockEnabled => C.NumberLock;

        /// <inheritdoc />
        public bool IsControlCTreatedAsInput
        {
            get => C.TreatControlCAsInput;
            set => C.TreatControlCAsInput = value;
        }

        /// <inheritdoc />
        public TextWriter OutputStream
        {
            get => C.Out;
            set => C.SetOut(value);
        }

        /// <inheritdoc />
        public Encoding OutputEncoding
        {
            get => C.OutputEncoding;
            set => C.OutputEncoding = value;
        }

        /// <inheritdoc />
        public bool IsOutputRedirected => C.IsOutputRedirected;

        /// <inheritdoc />
        public ConsoleColor BackgroundColor
        {
            get => C.BackgroundColor;
            set => C.BackgroundColor = value;
        }

        /// <inheritdoc />
        public ConsoleColor ForegroundColor
        {
            get => C.ForegroundColor;
            set => C.ForegroundColor = value;
        }

        /// <inheritdoc />
        public event ConsoleCancelEventHandler CancelKeyPress
        {
            add => C.CancelKeyPress += value;
            remove => C.CancelKeyPress -= value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleService"/> class.
        /// </summary>
        public ConsoleService()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleService"/> class.
        /// </summary>
        /// <param name="enableAnsiEscapeTrimming">A value indicating whether ANSI escape sequences should be removed when the console output is redirected.</param>
        public ConsoleService(bool enableAnsiEscapeTrimming)
        {
            IsFancyConsole =
                !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WT_SESSION")); // Windows Terminal
            IsAnsiEscapeTrimmingEnabled = enableAnsiEscapeTrimming;

            if (IsFancyConsole)
                OutputEncoding = Encoding.UTF8;
        }

        /// <inheritdoc />
        public void Beep() => C.Beep();

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public void Beep(int frequency, int duration) => C.Beep(frequency, duration);

        /// <inheritdoc />
        public void Clear() => C.Clear();

        /// <inheritdoc />
        public Stream OpenStandardError() => C.OpenStandardError();

        /// <inheritdoc />
        public Stream OpenStandardError(int bufferSize) => C.OpenStandardError(bufferSize);

        /// <inheritdoc />
        public Stream OpenStandardInput() => C.OpenStandardInput();

        /// <inheritdoc />
        public Stream OpenStandardInput(int bufferSize) => C.OpenStandardInput(bufferSize);

        /// <inheritdoc />
        public Stream OpenStandardOutput() => C.OpenStandardOutput();

        /// <inheritdoc />
        public Stream OpenStandardOutput(int bufferSize) => C.OpenStandardOutput(bufferSize);

        /// <inheritdoc />
        public int Read() => C.Read();

        /// <inheritdoc />
        public ConsoleKeyInfo ReadKey() => C.ReadKey();

        /// <inheritdoc />
        public ConsoleKeyInfo ReadKey(bool intercept) => C.ReadKey(intercept);

        /// <inheritdoc />
        public string? ReadLine() => C.ReadLine();

        /// <inheritdoc />
        public void ResetColor() => C.ResetColor();

        /// <inheritdoc />
        public void Write(string? value)
            => C.Write(EscapeIfNeeded(value));

        /// <inheritdoc />
        public void WriteLine(string? value)
            => C.WriteLine(EscapeIfNeeded(value));

        private string? EscapeIfNeeded(string? value)
            => IsAnsiEscapeTrimmingEnabled && value != null ? AnsiEscapeUtility.EscapeSequenceRegex.Replace(value, string.Empty) : value;
    }
}
