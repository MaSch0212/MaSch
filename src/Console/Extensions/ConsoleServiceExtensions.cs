using MaSch.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Console
{
    public static class ConsoleServiceExtensions
    {
        public static void Write(this IConsoleService service, string format, params object?[] args)
            => service.Write(string.Format(format, args));
        public static void Write<T>(this IConsoleService service, [AllowNull] T value)
            => service.Write(value?.ToString());
        public static void Write<T>(this IConsoleService service, [AllowNull] T value, string format)
            where T : IFormattable
            => service.Write(value?.ToString(format, null));
        public static void Write<T>(this IConsoleService service, [AllowNull] T value, IFormatProvider formatProvider)
            where T : IFormattable
            => service.Write(value?.ToString(null, formatProvider));
        public static void Write<T>(this IConsoleService service, [AllowNull] T value, string format, IFormatProvider formatProvider)
            where T : IFormattable
            => service.Write(value?.ToString(format, formatProvider));
        public static void Write(this IConsoleService service, char[] buffer)
            => service.Write(new string(buffer));
        public static void Write(this IConsoleService service, char[] buffer, int index, int count)
            => service.Write(new string(buffer, index, count));

        public static void WriteLine(this IConsoleService service, string format, params object?[] args)
            => service.WriteLine(string.Format(format, args));
        public static void WriteLine<T>(this IConsoleService service, [AllowNull] T value)
            => service.WriteLine(value?.ToString());
        public static void WriteLine<T>(this IConsoleService service, [AllowNull] T value, string format)
            where T : IFormattable
            => service.WriteLine(value?.ToString(format, null));
        public static void WriteLine<T>(this IConsoleService service, [AllowNull] T value, IFormatProvider formatProvider)
            where T : IFormattable
            => service.WriteLine(value?.ToString(null, formatProvider));
        public static void WriteLine<T>(this IConsoleService service, [AllowNull] T value, string format, IFormatProvider formatProvider)
            where T : IFormattable
            => service.WriteLine(value?.ToString(format, formatProvider));
        public static void WriteLine(this IConsoleService service, char[] buffer)
            => service.WriteLine(new string(buffer));
        public static void WriteLine(this IConsoleService service, char[] buffer, int index, int count)
            => service.WriteLine(new string(buffer, index, count));
        public static void WriteLine(this IConsoleService service)
            => service.WriteLine(string.Empty);

        public static void ReserveBufferLines(this IConsoleService service, int lineCount)
        {
            Guard.NotOutOfRange(lineCount, nameof(lineCount), 0, service.BufferSize.Height - 1);

            if (!service.IsOutputRedirected && lineCount > 0)
            {
                service.Write(new string('\n', lineCount));
                service.CursorPosition.Y -= lineCount;
            }
        }
    }
}
