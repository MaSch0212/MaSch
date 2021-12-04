using MaSch.Console.Ansi;
using MaSch.Core;
using MaSch.Core.Extensions;

namespace MaSch.Console;

/// <summary>
/// Contains extensions for the <see cref="IConsoleService"/> interface.
/// </summary>
public static class ConsoleServiceExtensions
{
    /// <summary>
    /// Writes the text representation of the specified object, to the standard output stream using the specified format information.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An object to write using format.</param>
    public static void Write(this IConsoleService service, string format, params object?[] args)
    {
        service.Write(string.Format(format, args));
    }

    /// <summary>
    /// Writes the text representation of the specified object, to the standard output stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to write.</typeparam>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    public static void Write<T>(this IConsoleService service, [AllowNull] T value)
    {
        service.Write(value?.ToString());
    }

    /// <summary>
    /// Writes the text representation of the specified object, to the standard output stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to write.</typeparam>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="format">The format to use.</param>
    public static void Write<T>(this IConsoleService service, [AllowNull] T value, string format)
        where T : IFormattable
    {
        service.Write(value?.ToString(format, null));
    }

    /// <summary>
    /// Writes the text representation of the specified object, to the standard output stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to write.</typeparam>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    public static void Write<T>(this IConsoleService service, [AllowNull] T value, IFormatProvider formatProvider)
        where T : IFormattable
    {
        service.Write(value?.ToString(null, formatProvider));
    }

    /// <summary>
    /// Writes the text representation of the specified object, to the standard output stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to write.</typeparam>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="format">The format to use.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    public static void Write<T>(this IConsoleService service, [AllowNull] T value, string format, IFormatProvider formatProvider)
        where T : IFormattable
    {
        service.Write(value?.ToString(format, formatProvider));
    }

    /// <summary>
    /// Writes the specified array of Unicode characters to the standard output stream.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="buffer">A Unicode character array.</param>
    public static void Write(this IConsoleService service, char[] buffer)
    {
        service.Write(new string(buffer));
    }

    /// <summary>
    /// Writes the specified subarray of Unicode characters to the standard output stream.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="buffer">A Unicode character array.</param>
    /// <param name="index">The starting position in buffer.</param>
    /// <param name="count">The number of characters to write.</param>
    public static void Write(this IConsoleService service, char[] buffer, int index, int count)
    {
        service.Write(new string(buffer, index, count));
    }

    /// <summary>
    /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream using the specified format information.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An object to write using format.</param>
    public static void WriteLine(this IConsoleService service, string format, params object?[] args)
    {
        service.WriteLine(string.Format(format, args));
    }

    /// <summary>
    /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to write.</typeparam>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    public static void WriteLine<T>(this IConsoleService service, [AllowNull] T value)
    {
        service.WriteLine(value?.ToString());
    }

    /// <summary>
    /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to write.</typeparam>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="format">The format to use.</param>
    public static void WriteLine<T>(this IConsoleService service, [AllowNull] T value, string format)
        where T : IFormattable
    {
        service.WriteLine(value?.ToString(format, null));
    }

    /// <summary>
    /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to write.</typeparam>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    public static void WriteLine<T>(this IConsoleService service, [AllowNull] T value, IFormatProvider formatProvider)
        where T : IFormattable
    {
        service.WriteLine(value?.ToString(null, formatProvider));
    }

    /// <summary>
    /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to write.</typeparam>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="format">The format to use.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    public static void WriteLine<T>(this IConsoleService service, [AllowNull] T value, string format, IFormatProvider formatProvider)
        where T : IFormattable
    {
        service.WriteLine(value?.ToString(format, formatProvider));
    }

    /// <summary>
    /// Writes the specified array of Unicode characters, followed by the current line terminator, to the standard output stream.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="buffer">A Unicode character array.</param>
    public static void WriteLine(this IConsoleService service, char[] buffer)
    {
        service.WriteLine(new string(buffer));
    }

    /// <summary>
    /// Writes the specified subarray of Unicode characters, followed by the current line terminator, to the standard output stream.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="buffer">A Unicode character array.</param>
    /// <param name="index">The starting position in buffer.</param>
    /// <param name="count">The number of characters to write.</param>
    public static void WriteLine(this IConsoleService service, char[] buffer, int index, int count)
    {
        service.WriteLine(new string(buffer, index, count));
    }

    /// <summary>
    /// Writes the current line terminator to the standard output stream.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    public static void WriteLine(this IConsoleService service)
    {
        service.WriteLine(string.Empty);
    }

    /// <summary>
    /// Reserves lines in the console buffer.
    /// </summary>
    /// <param name="service">The console to use.</param>
    /// <param name="lineCount">The number of lines to reserve.</param>
    public static void ReserveBufferLines(this IConsoleService service, int lineCount)
    {
        _ = Guard.NotOutOfRange(lineCount, nameof(lineCount), 0, service.BufferSize.Height - 1);

        if (!service.IsOutputRedirected && lineCount > 0)
        {
            service.Write(new string('\n', lineCount));
            service.CursorPosition.Y -= lineCount;
        }
    }

    /// <summary>
    /// Writes a title to the console.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="title">The title to write.</param>
    public static void WriteTitle(this IConsoleService service, string? title)
    {
        WriteTitle(service, title, '-');
    }

    /// <summary>
    /// Writes a title to the console.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="title">The title to write.</param>
    /// <param name="lineChar">The line character for the left and right padding.</param>
    public static void WriteTitle(this IConsoleService service, string? title, char lineChar)
    {
        service.Write(title?.PadBoth(service.BufferSize.Width, lineChar) ?? new string(lineChar, service.BufferSize.Width));
    }

    /// <summary>
    /// Writes the specified string value to the standard output stream using specific colors.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="foregroundColor">The foreground color of the text.</param>
    public static void WriteWithColor(this IConsoleService service, string? value, ConsoleColor foregroundColor)
    {
        using (SetColors(service, foregroundColor))
            service.Write(value);
    }

    /// <summary>
    /// Writes the specified string value to the standard output stream using specific colors.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="foregroundColor">The foreground color of the text.</param>
    /// <param name="backgroundColor">The background color of the text.</param>
    public static void WriteWithColor(this IConsoleService service, string? value, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        using (SetColors(service, foregroundColor, backgroundColor))
            service.Write(value);
    }

    /// <summary>
    /// Wries the specified string value to the standard output stream using the specified style.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="styleAction">The style for the text.</param>
    public static void WriteWithStyle(this IConsoleService service, string? value, Action<AnsiStyle> styleAction)
    {
        var style = new AnsiStyle();
        styleAction?.Invoke(style);
        service.Write(string.Concat(style.BuildAnsiSequence(), value, AnsiEscapeUtility.GetResetStyle()));
    }

    /// <summary>
    /// Writes the specified string value, followed by the current line terminator, to the standard output stream using specific colors.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="foregroundColor">The foreground color of the text.</param>
    public static void WriteLineWithColor(this IConsoleService service, string? value, ConsoleColor foregroundColor)
    {
        using (SetColors(service, foregroundColor))
            service.WriteLine(value);
    }

    /// <summary>
    /// Writes the specified string value, followed by the current line terminator, to the standard output stream using specific colors.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="foregroundColor">The foreground color of the text.</param>
    /// <param name="backgroundColor">The background color of the text.</param>
    public static void WriteLineWithColor(this IConsoleService service, string? value, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        using (SetColors(service, foregroundColor, backgroundColor))
            service.WriteLine(value);
    }

    /// <summary>
    /// Wries the specified string value, followed by the current line terminator, to the standard output stream using the specified style.
    /// </summary>
    /// <param name="service">The console to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="styleAction">The style for the text.</param>
    public static void WriteLineWithStyle(this IConsoleService service, string? value, Action<AnsiStyle> styleAction)
    {
        var style = new AnsiStyle();
        styleAction?.Invoke(style);
        service.WriteLine(string.Concat(style.BuildAnsiSequence(), value, AnsiEscapeUtility.GetResetStyle()));
    }

    /// <summary>
    /// Creates a <see cref="ConsoleColorScope"/> for this <see cref="IConsoleService"/>.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="foregroundColor">The foreground color that should be used inside this scope.</param>
    /// <returns>An instance of the <see cref="ConsoleColorScope"/> class.</returns>
    public static ConsoleColorScope SetColors(this IConsoleService service, ConsoleColor foregroundColor)
    {
        return new(service, foregroundColor);
    }

    /// <summary>
    /// Creates a <see cref="ConsoleColorScope"/> for this <see cref="IConsoleService"/>.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="foregroundColor">The foreground color that should be used inside this scope.</param>
    /// <param name="backgroundColor">The background color that should be used inside this scope.</param>
    /// <returns>An instance of the <see cref="ConsoleColorScope"/> class.</returns>
    public static ConsoleColorScope SetColors(this IConsoleService service, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        return new(service, foregroundColor, backgroundColor);
    }
}
