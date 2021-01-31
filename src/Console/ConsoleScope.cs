using System;
using System.Drawing;

namespace MaSch.Console
{
    /// <summary>
    /// Scopes an instance of the <see cref="IConsoleService"/> interface.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public sealed class ConsoleScope : IDisposable
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleScope"/> class.
        /// </summary>
        /// <param name="console">The console to scope.</param>
        /// <param name="scopeColor">if set to <c>true</c> the foreground and background color of the console are getting scoped.</param>
        /// <param name="scopePosition">if set to <c>true</c> the current cursor position of the console is getting scoped.</param>
        /// <param name="clearContent">if set to <c>true</c> the content that has been written during this scope are getting cleared afterwards.</param>
        public ConsoleScope(IConsoleService console, bool scopeColor, bool scopePosition, bool clearContent)
        {
            _console = console;
            _pos = scopePosition;
            _color = scopeColor;
            _clear = clearContent;

            _backColor = scopeColor ? console.BackgroundColor : ConsoleColor.Black;
            _foreColor = scopeColor ? console.ForegroundColor : ConsoleColor.Gray;
#if NETFRAMEWORK
            _cursorVisible = console.IsCursorVisible;
#else
            _cursorVisible = !OperatingSystem.IsWindows() || console.IsCursorVisible;
#endif

            var pos = scopePosition || clearContent ? console.CursorPosition.Point : new Point(0, 0);
            _posX = pos.X;
            _posY = pos.Y;
        }

        /// <inheritdoc />
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
                var count = ((pos.Y - _posY) * bufferSize.Width) + pos.X + (bufferSize.Width - _posX);
                _console.Write(new string(' ', count));
            }

            if (_pos)
            {
                _console.CursorPosition.Point = new Point(_posX, _posY);
            }

            _console.IsCursorVisible = _cursorVisible;
        }
    }
}
