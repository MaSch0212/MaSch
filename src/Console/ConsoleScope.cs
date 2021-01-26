using System;
using System.Drawing;

namespace MaSch.Console
{
    public class ConsoleScope : IDisposable
    {
        private readonly IConsoleService _console;
        private readonly int _posY;
        private readonly int _posX;
        private readonly ConsoleColor _backColor;
        private readonly ConsoleColor _foreColor;
        private readonly bool _pos;
        private readonly bool _color;
        private readonly bool _clear;
        private readonly bool _cursorVisible;

        public ConsoleScope(IConsoleService console, bool scopeColor, bool scopePosition, bool clearContent)
        {
            _console = console;
            _pos = scopePosition;
            _color = scopeColor;
            _clear = clearContent;

            _backColor = scopeColor ? console.BackgroundColor : ConsoleColor.Black;
            _foreColor = scopeColor ? console.ForegroundColor : ConsoleColor.Gray;
#if NETFX
            _cursorVisible = console.IsCursorVisible;
#else
            _cursorVisible = !OperatingSystem.IsWindows() || console.IsCursorVisible;
#endif

            var pos = scopePosition || clearContent ? console.CursorPosition.Point : new Point(0, 0);
            _posX = pos.X;
            _posY = pos.Y;
        }

        public void Dispose()
        {
            if (_color)
            {
                _console.ForegroundColor = _foreColor;
                _console.BackgroundColor = _backColor;
            }
            if (_clear)
            {
                var pos = _console.CursorPosition;
                var bufferSize = _console.BufferSize;
                _console.CursorPosition.Point = new Point(_posX, _posY);
                var count = (pos.Y - _posY) * bufferSize.Width + pos.X + (bufferSize.Width - _posX);
                _console.Write(new string(' ', count));
            }
            if (_pos)
            {
                _console.CursorPosition.Point = new Point(_posX, _posY);
            }
            _console.IsCursorVisible = _cursorVisible;
        }
    }

    public sealed class ConsoleColorScope : IDisposable
    {
        private readonly IConsoleService _console;
        private readonly ConsoleColor _backColor;
        private readonly ConsoleColor _foreColor;

        public ConsoleColorScope(IConsoleService console)
        {
            _console = console;
            _backColor = console.BackgroundColor;
            _foreColor = console.ForegroundColor;
        }
        public ConsoleColorScope(IConsoleService console, ConsoleColor newForegroundColor)
            : this(console)
        {
            console.ForegroundColor = newForegroundColor;
        }
        public ConsoleColorScope(IConsoleService console, ConsoleColor newForegroundColor, ConsoleColor newBackgroundColor)
            : this(console, newForegroundColor)
        {
            console.BackgroundColor = newBackgroundColor;
        }

        public void Dispose()
        {
            _console.ForegroundColor = _foreColor;
            _console.BackgroundColor = _backColor;
        }
    }
}
