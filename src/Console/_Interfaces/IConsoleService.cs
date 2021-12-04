using MaSch.Core.Lazy;
using System.Runtime.Versioning;

namespace MaSch.Console;

/// <summary>
/// Provides properties and methods to interact with a console window.
/// </summary>
public interface IConsoleService
{
    /// <summary>
    /// Occurs when the System.ConsoleModifiers.Control modifier key (Ctrl) and either the System.ConsoleKey.C
    /// console key (C) or the Break key are pressed simultaneously (Ctrl+C or Ctrl+Break).
    /// </summary>
    event ConsoleCancelEventHandler CancelKeyPress;

    /// <summary>
    /// Gets a value indicating whether this instance supports special characters like emojis.
    /// </summary>
    bool IsFancyConsole { get; }

    /// <summary>
    /// Gets a value indicating whether ANSI escape sequences are removed when the console output is redirected.
    /// </summary>
    bool IsAnsiEscapeTrimmingEnabled { get; }

    /// <summary>
    /// Gets a modifiable object representing the size of the buffer area.
    /// </summary>
    ConsoleSize BufferSize { get; }

    /// <summary>
    /// Gets a modifiable object representing the size of the console window area.
    /// </summary>
    ConsoleSize WindowSize { get; }

    /// <summary>
    /// Gets a modifiable object representing the position of the console window area relative to the screen buffer.
    /// </summary>
    ConsolePoint WindowPosition { get; }

    /// <summary>
    /// Gets the largest possible number of console window rows and columns, based on the current font and screen resolution.
    /// </summary>
    LazySize LargestWindowSize { get; }

    /// <summary>
    /// Gets or sets the title to display in the console title bar.
    /// </summary>
    string WindowTitle
    {
        get;
        [SupportedOSPlatform("windows")]
        set;
    }

    /// <summary>
    /// Gets a modifiable object representing the position of the cursor within the buffer area.
    /// </summary>
    ModifiableLazyPoint CursorPosition { get; }

    /// <summary>
    /// Gets or sets the height of the cursor within a character cell.
    /// </summary>
    int CursorSize
    {
        get;
        [SupportedOSPlatform("windows")]
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the cursor is visible.
    /// </summary>
    bool IsCursorVisible
    {
        [SupportedOSPlatform("windows")]
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the standard error output stream.
    /// </summary>
    TextWriter ErrorStream { get; set; }

    /// <summary>
    /// Gets a value indicating whether the error output stream has been redirected from the standard error stream.
    /// </summary>
    bool IsErrorRedirected { get; }

    /// <summary>
    /// Gets or sets the standard input stream.
    /// </summary>
    TextReader InputStream { get; set; }

    /// <summary>
    /// Gets or sets the encoding the console uses to read input.
    /// </summary>
    Encoding InputEncoding { get; set; }

    /// <summary>
    /// Gets a value indicating whether input has been redirected from the standard input stream.
    /// </summary>
    bool IsInputRedirected { get; }

    /// <summary>
    /// Gets a value indicating whether a key press is available in the input stream.
    /// </summary>
    bool IsKeyAvailable { get; }

    /// <summary>
    /// Gets a value indicating whether the CAPS LOCK keyboard toggle is turned on or turned off.
    /// </summary>
    [SupportedOSPlatform("windows")]
    bool IsCapsLockEnabled { get; }

    /// <summary>
    /// Gets a value indicating whether the NUM LOCK keyboard toggle is turned on or turned off.
    /// </summary>
    [SupportedOSPlatform("windows")]
    bool IsNumberLockEnabled { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the combination of the System.ConsoleModifiers.Control modifier key and
    /// System.ConsoleKey.C console key (Ctrl+C) is treated as ordinary input or as an interruption that is handled by the operating system.
    /// </summary>
    bool IsControlCTreatedAsInput { get; set; }

    /// <summary>
    /// Gets or sets the standard output stream.
    /// </summary>
    TextWriter OutputStream { get; set; }

    /// <summary>
    /// Gets or sets the encoding the console uses to write output.
    /// </summary>
    Encoding OutputEncoding { get; set; }

    /// <summary>
    /// Gets a value indicating whether output has been redirected from the standard output stream.
    /// </summary>
    bool IsOutputRedirected { get; }

    /// <summary>
    /// Gets or sets the background color of the console.
    /// </summary>
    ConsoleColor BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the foreground color of the console.
    /// </summary>
    ConsoleColor ForegroundColor { get; set; }

    /// <summary>
    /// Plays the sound of a beep through the console speaker.
    /// </summary>
    void Beep();

    /// <summary>
    /// Plays the sound of a beep of a specified frequency and duration through the console speaker.
    /// </summary>
    /// <param name="frequency">The frequency of the beep, ranging from 37 to 32767 hertz.</param>
    /// <param name="duration">The duration of the beep measured in milliseconds.</param>
    [SupportedOSPlatform("windows")]
    void Beep(int frequency, int duration);

    /// <summary>
    /// Clears the console buffer and corresponding console window of display information.
    /// </summary>
    void Clear();

    /// <summary>
    /// Acquires the standard error stream.
    /// </summary>
    /// <returns>The standard error stream.</returns>
    Stream OpenStandardError();

    /// <summary>
    /// Acquires the standard error stream, which is set to a specified buffer size.
    /// </summary>
    /// <param name="bufferSize">The internal stream buffer size.</param>
    /// <returns>The standard error stream.</returns>
    Stream OpenStandardError(int bufferSize);

    /// <summary>
    /// Acquires the standard input stream.
    /// </summary>
    /// <returns>The standard input stream.</returns>
    Stream OpenStandardInput();

    /// <summary>
    /// Acquires the standard input stream, which is set to a specified buffer size.
    /// </summary>
    /// <param name="bufferSize">The internal stream buffer size.</param>
    /// <returns>The standard input stream.</returns>
    Stream OpenStandardInput(int bufferSize);

    /// <summary>
    /// Acquires the standard output stream.
    /// </summary>
    /// <returns>The standard output stream.</returns>
    Stream OpenStandardOutput();

    /// <summary>
    /// Acquires the standard output stream, which is set to a specified buffer size.
    /// </summary>
    /// <param name="bufferSize">The internal stream buffer size.</param>
    /// <returns>The standard output stream.</returns>
    Stream OpenStandardOutput(int bufferSize);

    /// <summary>
    /// Reads the next character from the standard input stream.
    /// </summary>
    /// <returns>The next character from the input stream, or negative one (-1) if there are currently no more characters to be read.</returns>
    int Read();

    /// <summary>
    /// Obtains the next character or function key pressed by the user.
    /// The pressed key is displayed in the console window.
    /// </summary>
    /// <returns>
    /// An object that describes the System.ConsoleKey constant and Unicode character,
    /// if any, that correspond to the pressed console key. The System.ConsoleKeyInfo
    /// object also describes, in a bitwise combination of System.ConsoleModifiers values,
    /// whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously
    /// with the console key.
    /// </returns>
    ConsoleKeyInfo ReadKey();

    /// <summary>
    /// Obtains the next character or function key pressed by the user.
    /// The pressed key is optionally displayed in the console window.
    /// </summary>
    /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
    /// <returns>
    /// An object that describes the System.ConsoleKey constant and Unicode character,
    /// if any, that correspond to the pressed console key. The System.ConsoleKeyInfo
    /// object also describes, in a bitwise combination of System.ConsoleModifiers values,
    /// whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously
    /// with the console key.
    /// </returns>
    ConsoleKeyInfo ReadKey(bool intercept);

    /// <summary>
    /// Reads the next line of characters from the standard input stream.
    /// </summary>
    /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
    string? ReadLine();

    /// <summary>
    /// Sets the foreground and background console colors to their defaults.
    /// </summary>
    void ResetColor();

    /// <summary>
    /// Writes the specified string value to the standard output stream.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void Write(string? value);

    /// <summary>
    /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void WriteLine(string? value);
}
