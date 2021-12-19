using MaSch.Console.Ansi;
using MaSch.Core.Lazy;
using System.Runtime.Versioning;
using C = System.Console;

namespace MaSch.Console;

/// <summary>
/// Implementation of the <see cref="IConsoleService"/> interface wrapping the <see cref="System.Console"/> class.
/// </summary>
/// <seealso cref="IConsoleService" />
public class ConsoleService : IConsoleService
{
    private int _fallbackBufferWidth = 1000;
    private int _fallbackBufferHeight = 1000;
    private int _fallbackWindowWidth = 1000;
    private int _fallbackWindowHeight = 1000;
    private int _fallbackWindowLeft = 0;
    private int _fallbackWindowTop = 0;

    private ConsoleSize? _bufferSize;
    private ConsoleSize? _windowSize;
    private ConsolePoint? _windowPosition;

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
    public event ConsoleCancelEventHandler CancelKeyPress
    {
        add => C.CancelKeyPress += value;
        remove => C.CancelKeyPress -= value;
    }

    /// <inheritdoc />
    public bool IsFancyConsole { get; }

    /// <inheritdoc />
    public bool IsAnsiEscapeTrimmingEnabled { get; }

    /// <inheritdoc />
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "This is already verified by the ConsoleSize class.")]
    public ConsoleSize BufferSize
    {
        get
        {
            return _bufferSize ??=
                new ConsoleSize(
                    widthFactory: () => IsOutputRedirected ? _fallbackBufferWidth : C.BufferWidth,
                    widthCallback: x =>
                    {
                        if (IsOutputRedirected)
                            _fallbackBufferWidth = x;
                        else
                            C.BufferWidth = x;
                    },
                    heightFactory: () => IsOutputRedirected ? _fallbackBufferHeight : C.BufferHeight,
                    heightCallback: x =>
                    {
                        if (IsOutputRedirected)
                            _fallbackBufferHeight = x;
                        else
                            C.BufferHeight = x;
                    },
                    useCaching: false);
       }
    }

    /// <inheritdoc />
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "This is already verified by the ConsoleSize class.")]
    public ConsoleSize WindowSize
    {
        get
        {
            return _windowSize ??=
                new ConsoleSize(
                    widthFactory: () => IsOutputRedirected ? _fallbackWindowWidth : C.WindowWidth,
                    widthCallback: x =>
                    {
                        if (IsOutputRedirected)
                            _fallbackWindowWidth = x;
                        else
                            C.WindowWidth = x;
                    },
                    heightFactory: () => IsOutputRedirected ? _fallbackWindowHeight : C.WindowHeight,
                    heightCallback: x =>
                    {
                        if (IsOutputRedirected)
                            _fallbackWindowHeight = x;
                        else
                            C.WindowHeight = x;
                    },
                    useCaching: false);
        }
    }

    /// <inheritdoc />
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "This is already verified by the ConsolePoint class.")]
    public ConsolePoint WindowPosition
    {
        get
        {
            return _windowPosition ??=
                new ConsolePoint(
                    xFactory: () => IsOutputRedirected ? _fallbackWindowLeft : C.WindowLeft,
                    xCallback: x =>
                    {
                        if (IsOutputRedirected)
                            _fallbackWindowLeft = x;
                        else
                            C.WindowLeft = x;
                    },
                    yFactory: () => IsOutputRedirected ? _fallbackWindowTop : C.WindowTop,
                    yCallback: x =>
                    {
                        if (IsOutputRedirected)
                            _fallbackWindowTop = x;
                        else
                            C.WindowTop = x;
                    },
                    useCaching: false);
        }
    }

    /// <inheritdoc />
    public LazySize LargestWindowSize { get; } = new LazySize(() => C.IsOutputRedirected ? int.MaxValue : C.LargestWindowWidth, () => C.IsOutputRedirected ? int.MaxValue : C.LargestWindowHeight, false);

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
    public void Beep()
    {
        C.Beep();
    }

    /// <inheritdoc />
    [SupportedOSPlatform("windows")]
    public void Beep(int frequency, int duration)
    {
        C.Beep(frequency, duration);
    }

    /// <inheritdoc />
    public void Clear()
    {
        C.Clear();
    }

    /// <inheritdoc />
    public Stream OpenStandardError()
    {
        return C.OpenStandardError();
    }

    /// <inheritdoc />
    public Stream OpenStandardError(int bufferSize)
    {
        return C.OpenStandardError(bufferSize);
    }

    /// <inheritdoc />
    public Stream OpenStandardInput()
    {
        return C.OpenStandardInput();
    }

    /// <inheritdoc />
    public Stream OpenStandardInput(int bufferSize)
    {
        return C.OpenStandardInput(bufferSize);
    }

    /// <inheritdoc />
    public Stream OpenStandardOutput()
    {
        return C.OpenStandardOutput();
    }

    /// <inheritdoc />
    public Stream OpenStandardOutput(int bufferSize)
    {
        return C.OpenStandardOutput(bufferSize);
    }

    /// <inheritdoc />
    public int Read()
    {
        return C.Read();
    }

    /// <inheritdoc />
    public ConsoleKeyInfo ReadKey()
    {
        return C.ReadKey();
    }

    /// <inheritdoc />
    public ConsoleKeyInfo ReadKey(bool intercept)
    {
        return C.ReadKey(intercept);
    }

    /// <inheritdoc />
    public string? ReadLine()
    {
        return C.ReadLine();
    }

    /// <inheritdoc />
    public void ResetColor()
    {
        C.ResetColor();
    }

    /// <inheritdoc />
    public void Write(string? value)
    {
        C.Write(EscapeIfNeeded(value));
    }

    /// <inheritdoc />
    public void WriteLine(string? value)
    {
        C.WriteLine(EscapeIfNeeded(value));
    }

    private string? EscapeIfNeeded(string? value)
    {
        return IsAnsiEscapeTrimmingEnabled && value != null ? AnsiEscapeUtility.EscapeSequenceRegex.Replace(value, string.Empty) : value;
    }
}
