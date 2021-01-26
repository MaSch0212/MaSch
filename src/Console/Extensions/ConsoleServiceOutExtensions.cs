using MaSch.Core.Extensions;
using System;

namespace MaSch.Console
{
    public static class ConsoleServiceOutExtensions
    {
        public static void WriteTitle(this IConsoleService service, string? title)
            => WriteTitle(service, title, '-');
        public static void WriteTitle(this IConsoleService service, string? title, char lineChar)
            => service.Write(title?.PadBoth(service.BufferSize.Width, lineChar) ?? new string(lineChar, service.BufferSize.Width));

        public static void WriteWithColor(this IConsoleService service, string? value, ConsoleColor foregroundColor)
        {
            using (SetColors(service, foregroundColor))
                service.Write(value);
        }
        public static void WriteWithColor(this IConsoleService service, string? value, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            using (SetColors(service, foregroundColor, backgroundColor))
                service.Write(value);
        }
        public static void WriteLineWithColor(this IConsoleService service, string? value, ConsoleColor foregroundColor)
        {
            using (SetColors(service, foregroundColor))
                service.WriteLine(value);
        }
        public static void WriteLineWithColor(this IConsoleService service, string? value, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            using (SetColors(service, foregroundColor, backgroundColor))
                service.WriteLine(value);
        }

        public static ConsoleColorScope SetColors(this IConsoleService service, ConsoleColor foregroundColor)
            => new ConsoleColorScope(service, foregroundColor);
        public static ConsoleColorScope SetColors(this IConsoleService service, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
            => new ConsoleColorScope(service, foregroundColor, backgroundColor);
    }
}
