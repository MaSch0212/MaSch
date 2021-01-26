using MaSch.Core.Lazy;
using System;
using System.IO;
using System.Runtime.Versioning;
using System.Text;

namespace MaSch.Console
{
    public interface IConsoleService
    {
        bool IsFancyConsole { get; }

        ConsoleSize BufferSize { get; }
        ConsoleSize WindowSize { get; }
        ConsolePoint WindowPosition { get; }
        LazySize LargestWindowSize { get; }
        string WindowTitle { get; [SupportedOSPlatform("windows")] set; }

        ModifiableLazyPoint CursorPosition { get; }
        int CursorSize { get; [SupportedOSPlatform("windows")] set; }
        bool IsCursorVisible { [SupportedOSPlatform("windows")] get; set; }

        TextWriter ErrorStream { get; set; }
        bool IsErrorRedirected { get; }

        TextReader InputStream { get; set; }
        Encoding InputEncoding { get; set; }
        bool IsInputRedirected { get; }
        bool IsKeyAvailable { get; }
        [SupportedOSPlatform("windows")]
        bool IsCapsLockEnabled { get; }
        [SupportedOSPlatform("windows")]
        bool IsNumberLockEnabled { get; }
        bool IsControlCTreatedAsInput { get; set; }

        TextWriter OutputStream { get; set; }
        Encoding OutputEncoding { get; set; }
        bool IsOutputRedirected { get; }
        ConsoleColor BackgroundColor { get; set; }
        ConsoleColor ForegroundColor { get; set; }

        event ConsoleCancelEventHandler CancelKeyPress;

        void Beep();
        [SupportedOSPlatform("windows")]
        void Beep(int frequency, int duration);
        void Clear();
        Stream OpenStandardError();
        Stream OpenStandardError(int bufferSize);
        Stream OpenStandardInput();
        Stream OpenStandardInput(int bufferSize);
        Stream OpenStandardOutput();
        Stream OpenStandardOutput(int bufferSize);
        int Read();
        ConsoleKeyInfo ReadKey();
        ConsoleKeyInfo ReadKey(bool intercept);
        string? ReadLine();
        void ResetColor();
        void Write(string? value);
        void WriteLine(string? value);
    }
}
