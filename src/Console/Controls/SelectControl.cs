using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MaSch.Console.Controls
{
    /// <summary>
    /// Control for a <see cref="IConsoleService"/> with which the user can select an element out of a set of items.
    /// </summary>
    public class SelectControl
    {
        private readonly IConsoleService _console;
        private int _selectedIndex = -1;
        private string? _selectedItem;
        private IList<string?>? _items;

        /// <summary>
        /// Gets or sets the selection mode to use.
        /// </summary>
        public OneSelectionMode SelectionMode { get; set; } = OneSelectionMode.UpDown;

        /// <summary>
        /// Gets or sets the label. This can be a question for example.
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets the last selected item.
        /// </summary>
        /// <exception cref="InvalidOperationException">Set the <see cref="Items"/> property before setting a selected item.</exception>
        /// <exception cref="ArgumentException">The item was not found in the <see cref="Items"/> list.</exception>
        public string? SelectedItem
        {
            get => _selectedItem;
            set
            {
                var index = (Items ?? throw new InvalidOperationException($"Set the {nameof(Items)} property before setting a selected item.")).IndexOf(value);
                if (index < 0)
                    throw new ArgumentException($"The item \"{value}\" was not found in the {nameof(Items)} list.", nameof(value));
                _selectedItem = value;
                _selectedIndex = index;
            }
        }

        /// <summary>
        /// Gets or sets the last selected index.
        /// </summary>
        /// <exception cref="InvalidOperationException">Set the <see cref="Items"/> property before setting a selected index.</exception>
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                var item = (Items ?? throw new InvalidOperationException($"Set the {nameof(Items)} property before setting a selected index."))[_selectedIndex];
                _selectedIndex = value;
                _selectedItem = item;
            }
        }

        /// <summary>
        /// Gets or sets the items that can be selected.
        /// </summary>
        public IList<string?>? Items
        {
            get => _items;
            set
            {
                _items = value;
                _selectedIndex = -1;
                _selectedItem = null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectControl"/> class.
        /// </summary>
        /// <param name="console">The console to use.</param>
        public SelectControl(IConsoleService console)
        {
            _console = Guard.NotNull(console, nameof(console));
        }

        /// <summary>
        /// Requests the user to select an item.
        /// </summary>
        public void Show()
        {
            var result = Show(_console, SelectionMode, Label, SelectedIndex, Items);
            _selectedIndex = result.Index;
            _selectedItem = result.Value;
        }

        /// <summary>
        /// Requests the user to select an item.
        /// </summary>
        /// <param name="console">The console to use.</param>
        /// <param name="mode">The selection mode to use.</param>
        /// <param name="label">The label.</param>
        /// <param name="startIndex">The index of the item to preselect.</param>
        /// <param name="items">The items from which the user can choose one.</param>
        /// <returns>The from the user selected item.</returns>
        public static Selection Show(IConsoleService console, OneSelectionMode mode, string? label, int startIndex, params string?[] items)
            => Show(console, mode, label, startIndex, (IList<string?>?)items);

        /// <summary>
        /// Requests the user to select an item.
        /// </summary>
        /// <param name="console">The console to use.</param>
        /// <param name="mode">The selection mode to use.</param>
        /// <param name="label">The label.</param>
        /// <param name="startIndex">The index of the item to preselect.</param>
        /// <param name="items">The items from which the user can choose one.</param>
        /// <returns>The from the user selected item.</returns>
        /// <exception cref="ArgumentException">
        /// At least one item needs to be present. - <paramref name="items"/>.
        /// or
        /// The index is out of the items bounds. - <paramref name="startIndex"/>.
        /// </exception>
        public static Selection Show(IConsoleService console, OneSelectionMode mode, string? label, int startIndex, IList<string?>? items)
        {
            if (items.IsNullOrEmpty())
                throw new ArgumentException("At least one item needs to be present.", nameof(items));
            if (startIndex >= items.Count)
                throw new ArgumentException("The index is out of the items bounds.", nameof(startIndex));

            using var scope = ConsoleSynchronizer.Scope();

            var result = default(Selection);
            console.Write(label);

            if (mode == OneSelectionMode.LeftRight)
                result = ShowLeftRight(console, startIndex, items);
            else if (mode == OneSelectionMode.UpDown)
                result = ShowUpDown(console, startIndex, items);
            else if (mode == OneSelectionMode.UpDown3)
                result = ShowUpDown3(console, startIndex, items);

            return result;
        }

        private static Selection ShowUpDown(IConsoleService console, int startIndex, IList<string?> data)
        {
            console.IsCursorVisible = false;
            var pos = console.CursorPosition.Point;
            string up = " ↑ ", down = " ↓ ", updown = " ↕ ";
            int index = startIndex;
            var bs = console.BufferSize;

            ConsoleKeyInfo key;
            int space;
            string? item;
            do
            {
                console.CursorPosition.Point = pos;
                console.Write(new string(' ', bs.Width - pos.X - 1));
                console.CursorPosition.Point = pos;
                if ((index == 0 && data.Count > 1) || index < 0)
                    console.Write(down);
                else if (index == data.Count - 1)
                    console.Write(up);
                else
                    console.Write(updown);
                space = bs.Width - pos.X - 1;
                item = index < 0 ? null : data[index];
                if (item != null && item.Length > space)
                    item = item.Substring(0, space - 3) + "...";
                console.Write(item);
                key = console.ReadKey(true);
                if ((key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.LeftArrow) && index > 0)
                    index--;
                else if ((key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.RightArrow) && index < data.Count - 1)
                    index++;
            }
            while (key.Key != ConsoleKey.Enter || index < 0);

            console.CursorPosition.Point = new Point(0, pos.Y);
            console.WriteLine();

            console.IsCursorVisible = true;
            return new Selection(index, data[index]);
        }

        private static Selection ShowUpDown3(IConsoleService console, int startIndex, IList<string?> data)
        {
            console.IsCursorVisible = false;
            var pos = console.CursorPosition.Point;
            int index = startIndex;
            ConsoleKeyInfo key;
            string item;
            var bs = console.BufferSize;
            int space = bs.Width - pos.X - 1;

            do
            {
                int start = index <= 0 ? 0 : (index == data.Count - 1 ? data.Count - 3 : index - 1);
                if (start < 0)
                    start = 0;
                for (int i = 0; i < 3 && start + i < data.Count; i++)
                {
                    console.CursorPosition.Point = new Point(pos.X, pos.Y + i);
                    console.Write(new string(' ', space));
                    console.CursorPosition.Point = new Point(pos.X, pos.Y + i);

                    item = " " + data[start + i] + " ";
                    if (item.Length > space - 1)
                        item = item.Substring(0, space - 4) + "... ";

                    if (i == 0 && start > 0)
                        console.Write("↑");
                    else if (i == 2 && start < data.Count - 3)
                        console.Write("↓");
                    else
                        console.Write("│");

                    if (start + i == index)
                        console.WriteWithColor(item, console.BackgroundColor, console.ForegroundColor);
                    else
                        console.Write(item);
                }

                key = console.ReadKey(true);
                if ((key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.LeftArrow) && index > 0)
                    index--;
                else if ((key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.RightArrow) && index < data.Count - 1)
                    index++;
            }
            while (key.Key != ConsoleKey.Enter || index < 0);

            for (int i = 0; i < 3; i++)
            {
                console.CursorPosition.Point = new Point(pos.X, pos.Y + i);
                console.Write(new string(' ', space));
            }

            console.CursorPosition.Point = pos;
            item = " " + data[index] + " ";
            if (item.Length > space)
                item = item.Substring(0, space - 4) + "... ";
            console.Write(item);
            console.CursorPosition.Point = new Point(0, pos.Y);
            console.WriteLine();

            console.IsCursorVisible = true;
            return new Selection(index, data[index]);
        }

        private static Selection ShowLeftRight(IConsoleService console, int startIndex, IList<string?> data)
        {
            console.IsCursorVisible = false;
            var pos = console.CursorPosition.Point;
            int index = startIndex;
            ConsoleKeyInfo key;
            string item;
            var bs = console.BufferSize;
            int space;

            do
            {
                console.CursorPosition.Point = pos;
                for (int i = 0; i < data.Count; i++)
                {
                    item = " " + data[i] + " ";
                    space = bs.Width - pos.X - 1;
                    if (item.Length > space)
                        item = Environment.NewLine + item;

                    if (i == index)
                        console.WriteWithColor(item, console.BackgroundColor, console.ForegroundColor);
                    else
                        console.Write(item);

                    if (i < data.Count - 1)
                        console.Write(" ");
                }

                key = console.ReadKey(true);
                if ((key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.LeftArrow) && index > 0)
                    index--;
                else if ((key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.RightArrow) && index < data.Count - 1)
                    index++;
            }
            while (key.Key != ConsoleKey.Enter || index < 0);

            console.CursorPosition.Point = new Point(0, pos.Y);
            console.WriteLine();

            console.IsCursorVisible = true;
            return new Selection(index, data[index]);
        }

        /// <summary>
        /// The selection mode for the <see cref="SelectControl"/>.
        /// </summary>
        public enum OneSelectionMode
        {
            /// <summary>
            /// Displays one item at a time. The user can switch between the items using the up and down arrow keys.
            /// </summary>
            UpDown,

            /// <summary>
            /// Displays the items next to each other. The user can switch between the items using the left and right arrow keys.
            /// </summary>
            LeftRight,

            /// <summary>
            /// Displays a maximum of three items at a time. The user can switch between the items using the up and down arrow keys.
            /// </summary>
            UpDown3,
        }

        /// <summary>
        /// Represents a user selection that has been made using the <see cref="SelectControl"/>.
        /// </summary>
        public struct Selection
        {
            /// <summary>
            /// Gets or sets the index that has been selected.
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// Gets or sets the value that has been selected.
            /// </summary>
            public string? Value { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Selection"/> struct.
            /// </summary>
            /// <param name="index">The index that has been selected..</param>
            /// <param name="value">The value that has been selected..</param>
            public Selection(int index, string? value)
            {
                Index = index;
                Value = value;
            }
        }
    }
}
