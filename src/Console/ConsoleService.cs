using MaSch.Core.Lazy;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using C = System.Console;

namespace MaSch.Console
{
    public class ConsoleSize : ModifiableLazySize
    {
        public override int Height
        {
            get => base.Height;
            [SupportedOSPlatform("windows")]
            set => base.Height = value;
        }

        public override int Width
        {
            get => base.Width;
            [SupportedOSPlatform("windows")]
            set => base.Width = value;
        }

        public override Size Size
        {
            get => base.Size;
            [SupportedOSPlatform("windows")]
            set => base.Size = value;
        }

        public ConsoleSize(Func<int> widthFactory, Action<int> widthCallback, Func<int> heightFactory, Action<int> heightCallback) : base(widthFactory, widthCallback, heightFactory, heightCallback)
        {
        }

        public ConsoleSize(Func<int> widthFactory, Action<int> widthCallback, Func<int> heightFactory, Action<int> heightCallback, bool useCaching) : base(widthFactory, widthCallback, heightFactory, heightCallback, useCaching)
        {
        }
    }

    public class ConsolePoint : ModifiableLazyPoint
    {
        public override int X
        {
            get => base.X;
            [SupportedOSPlatform("windows")]
            set => base.X = value;
        }

        public override int Y
        {
            get => base.Y;
            [SupportedOSPlatform("windows")]
            set => base.Y = value;
        }

        public override Point Point
        {
            get => base.Point;
            [SupportedOSPlatform("windows")]
            set => base.Point = value;
        }

        public ConsolePoint(Func<int> xFactory, Action<int> xCallback, Func<int> yFactory, Action<int> yCallback) : base(xFactory, xCallback, yFactory, yCallback)
        {
        }

        public ConsolePoint(Func<int> xFactory, Action<int> xCallback, Func<int> yFactory, Action<int> yCallback, bool useCaching) : base(xFactory, xCallback, yFactory, yCallback, useCaching)
        {
        }
    }

    public class ConsoleService : IConsoleService
    {
        public bool IsFancyConsole { get; }

#pragma warning disable CA1416 // Validate platform compatibility
        public ConsoleSize BufferSize { get; } = new ConsoleSize(() => C.BufferWidth, x => C.BufferWidth = x, () => C.BufferHeight, x => C.BufferHeight = x, false);
        public ConsoleSize WindowSize { get; } = new ConsoleSize(() => C.WindowWidth, x => C.WindowWidth = x, () => C.WindowHeight, x => C.WindowHeight = x, false);
        public ConsolePoint WindowPosition { get; } = new ConsolePoint(() => C.WindowLeft, x => C.WindowLeft = x, () => C.WindowTop, x => C.WindowTop = x, false);
#pragma warning restore CA1416 // Validate platform compatibility
        public LazySize LargestWindowSize { get; } = new LazySize(() => C.LargestWindowWidth, () => C.LargestWindowHeight, false);
        public string WindowTitle
        {
            [SupportedOSPlatform("windows")]
            get => C.Title;
            set => C.Title = value;
        }

        public ModifiableLazyPoint CursorPosition { get; } = new ModifiableLazyPoint(() => C.CursorLeft, x => C.CursorLeft = x, () => C.CursorTop, x => C.CursorTop = x, false);
        public int CursorSize
        {
            get => C.CursorSize;
            [SupportedOSPlatform("windows")]
            set => C.CursorSize = value;
        }
        public bool IsCursorVisible
        {
            [SupportedOSPlatform("windows")]
            get => C.CursorVisible;
            set => C.CursorVisible = value;
        }

        public TextWriter ErrorStream
        {
            get => C.Error;
            set => C.SetError(value);
        }
        public bool IsErrorRedirected => C.IsErrorRedirected;

        public TextReader InputStream
        {
            get => C.In;
            set => C.SetIn(value);
        }
        public Encoding InputEncoding
        {
            get => C.InputEncoding;
            set => C.InputEncoding = value;
        }
        public bool IsInputRedirected => C.IsInputRedirected;
        public bool IsKeyAvailable => C.KeyAvailable;
        [SupportedOSPlatform("windows")]
        public bool IsCapsLockEnabled => C.CapsLock;
        [SupportedOSPlatform("windows")]
        public bool IsNumberLockEnabled => C.NumberLock;
        public bool IsControlCTreatedAsInput
        {
            get => C.TreatControlCAsInput;
            set => C.TreatControlCAsInput = value;
        }

        public TextWriter OutputStream
        {
            get => C.Out;
            set => C.SetOut(value);
        }
        public Encoding OutputEncoding
        {
            get => C.OutputEncoding;
            set => C.OutputEncoding = value;
        }
        public bool IsOutputRedirected => C.IsOutputRedirected;
        public ConsoleColor BackgroundColor
        {
            get => C.BackgroundColor;
            set => C.BackgroundColor = value;
        }
        public ConsoleColor ForegroundColor
        {
            get => C.ForegroundColor;
            set => C.ForegroundColor = value;
        }

        public event ConsoleCancelEventHandler CancelKeyPress
        {
            add => C.CancelKeyPress += value;
            remove => C.CancelKeyPress -= value;
        }

        public ConsoleService()
        {
            IsFancyConsole =
                !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WT_SESSION")); // Windows Terminal

            if (IsFancyConsole)
                OutputEncoding = Encoding.Unicode;
        }

        public void Beep() => C.Beep();
        [SupportedOSPlatform("windows")]
        public void Beep(int frequency, int duration) => C.Beep(frequency, duration);
        public void Clear() => C.Clear();
        public Stream OpenStandardError() => C.OpenStandardError();
        public Stream OpenStandardError(int bufferSize) => C.OpenStandardError(bufferSize);
        public Stream OpenStandardInput() => C.OpenStandardInput();
        public Stream OpenStandardInput(int bufferSize) => C.OpenStandardInput(bufferSize);
        public Stream OpenStandardOutput() => C.OpenStandardOutput();
        public Stream OpenStandardOutput(int bufferSize) => C.OpenStandardOutput(bufferSize);
        public int Read() => C.Read();
        public ConsoleKeyInfo ReadKey() => C.ReadKey();
        public ConsoleKeyInfo ReadKey(bool intercept) => C.ReadKey(intercept);
        public string? ReadLine() => C.ReadLine();
        public void ResetColor() => C.ResetColor();
        public void Write(string? value) => C.Write(value);
        public void WriteLine(string? value) => C.WriteLine(value);
    }
}
