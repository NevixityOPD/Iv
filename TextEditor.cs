using System.Text;
using Iv.Components;

namespace Iv
{
    public class TextEditor
    {
        //Threads
        private Thread _top_bar;
        private Thread _bottom_bar;
        private Thread _update_text;

        private Page _file_page;
        private KeyboardState _keyboardState = KeyboardState.Normal;

        //Padding
        private int left_padding = 2;
        private int top_padding = 1;
        private int bottom_padding = 1;

        //Limits
        private int page_starts = 1;
        private int page_ends = Console.WindowHeight - 2;
        private int page_heigth;
        private int line_length = Console.WindowWidth - 2;

        //Variables
        private int line_padding = 0;

        public TextEditor()
        {
            Console.Clear();

            _top_bar = new Thread(UpdateTopBar);
            _bottom_bar = new Thread(UpdateBottomBar);
            _update_text = new Thread(UpdateText);

            _top_bar.Start();
            _bottom_bar.Start();

            _file_page = new Page();

            //Calculates variables
            page_heigth = (Console.WindowHeight - 1) - (top_padding + bottom_padding);

            Console.Title = $"Iv - {_file_page._title}";

            //Top bar
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.ResetColor();

            //Bottom bar
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.ResetColor();
            Console.SetCursorPosition(0, 1);

            _update_text.Start();
            MainEditor();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private void UpdateTopBar()
        {
            string text = $"Iv | {DateTime.Now:HH:mm:ss} | Editing file \"{_file_page._title}\" | ";
            while (true)
            {
                text = $" Iv | {DateTime.Now:HH:mm:ss} | Editing file \"{_file_page._title}\" | ";
                QuickConsole.FastWrite_Color(text, new Cursor() { X = 0, Y = 0 }, ConsoleColor.White, ConsoleColor.Black);
                if (_file_page._file_status == FileStatus.Unsaved)
                {
                    QuickConsole.FastWrite_Color("*Unsaved", new Cursor() { X = (short)text.Length, Y = 0 }, ConsoleColor.White, ConsoleColor.DarkRed);
                }

                if (Console.CapsLock)
                {
                    QuickConsole.FastWrite_Color("Capslock On", new Cursor()
                    {
                        X = (short)(Console.WindowWidth - "Capslock On".Length),
                        Y = 0,
                    }, ConsoleColor.White, ConsoleColor.Black);
                }
                else
                {
                    QuickConsole.FastWrite_Color(new string(' ', "Capslock On".Length), new Cursor()
                    {
                        X = (short)(Console.WindowWidth - "Capslock On".Length),
                        Y = 0,
                    }, ConsoleColor.White, ConsoleColor.Black);
                }
                Thread.Sleep(150);
            }
        }

        private void UpdateBottomBar()
        {
            while (true)
            {
                QuickConsole.FastDrawLine(Console.WindowHeight - 1, $" [{_keyboardState}]", ConsoleColor.White, (_keyboardState == KeyboardState.Normal) ? ConsoleColor.DarkGreen : ConsoleColor.DarkYellow);
                QuickConsole.FastWrite_Color($"Ln {_file_page._cursor.X} Col {_file_page._cursor.Y} Ph {page_heigth} Lp {line_padding} ", new Cursor() { X = (short)(Console.WindowWidth - $"Ln {_file_page._cursor.X} Col {_file_page._cursor.Y} Ph {page_heigth} Lp {line_padding} ".Length), Y = (short)(Console.WindowHeight - 1), }, ConsoleColor.White, ConsoleColor.Black);
                Thread.Sleep(20);
            }
        }

        private void UpdateText()
        {
            while (true)
            {
                List<StringBuilder> _lines;
                //Checks if total lines is more than screen height
                if (_file_page._cursor.Y > page_heigth)
                {
                    line_padding = _file_page._cursor.Y - page_heigth;
                    _lines = _file_page._lines.GetRange(line_padding, page_heigth + 1);
                }
                else
                {
                    _lines = _file_page._lines;
                }

                for (int i = 0; i < _lines.Count; i++)
                {
                    if ((short)(i + top_padding) < Console.WindowHeight - 1)
                    {
                        QuickConsole.FastDrawLine((short)(i + top_padding), $"~ {_lines[i]}", ConsoleColor.Black, ConsoleColor.White);
                    }
                }

                if (_file_page._cursor.X >= Console.WindowWidth - 3)
                {
                    if (_file_page._cursor.Y >= page_heigth)
                    {
                        Console.SetCursorPosition(_file_page._cursor.X + 1, page_heigth + top_padding);
                    }
                    else
                    {
                        Console.SetCursorPosition(_file_page._cursor.X + 1, _file_page._cursor.Y + top_padding);
                    }
                }
                else
                {
                
                    if (_file_page._cursor.Y >= page_heigth)
                    {
                        Console.SetCursorPosition(_file_page._cursor.X + left_padding, page_heigth + top_padding);
                    }
                    else
                    {
                        Console.SetCursorPosition(_file_page._cursor.X + left_padding, _file_page._cursor.Y + top_padding);
                    }
                }
            }
        }


        public void MainEditor()
        {
            while (true)
            {
                ConsoleKeyInfo _key = Console.ReadKey(true);

                if (_key.Key == ConsoleKey.Enter)
                {
                    _file_page._lines.Add(new StringBuilder());
                    _file_page._cursor.Y++;
                    if (_file_page._lines[_file_page._cursor.Y].Length != 0)
                    {
                        _file_page._cursor.X = (short)_file_page._lines[_file_page._cursor.Y].Length;
                    }
                    else
                    {
                        _file_page._cursor.X = 0;
                    }
                }
                else if (_key.Key == ConsoleKey.Backspace)
                {
                    if (_file_page._cursor.X != 0)
                    {
                        _file_page._lines[_file_page._cursor.Y].Remove(_file_page._cursor.X - 1, 1);
                        _file_page._cursor.X--;
                    }
                }
                else if (_key.Key == ConsoleKey.LeftArrow)
                {
                    if (_file_page._cursor.X != 0)
                    {
                        _file_page._cursor.X--;
                    }
                }
                else if (_key.Key == ConsoleKey.RightArrow)
                {
                    if (_file_page._cursor.X != _file_page._lines[_file_page._cursor.Y].Length)
                    {
                        _file_page._cursor.X++;
                    }
                }
                else if (_key.Key == ConsoleKey.UpArrow)
                {
                    if (_file_page._cursor.Y != 0)
                    {
                        _file_page._cursor.Y--;
                        _file_page._cursor.X = (short)_file_page._lines[_file_page._cursor.Y].Length;
                    }
                }
                else if (_key.Key == ConsoleKey.DownArrow)
                {
                    if (_file_page._cursor.Y != _file_page._lines.Count - 1)
                    {
                        _file_page._cursor.Y++;
                        _file_page._cursor.X = (short)_file_page._lines[_file_page._cursor.Y].Length;
                    }
                }
                else if ((_key.Modifiers == ConsoleModifiers.Shift) && _key.Key == ConsoleKey.Escape)
                {
                    if (_keyboardState == KeyboardState.Normal) { _keyboardState = KeyboardState.Readonly; }
                    else if (_keyboardState == KeyboardState.Readonly) { _keyboardState = KeyboardState.Normal; }
                }
                else
                {
                    if (_keyboardState == KeyboardState.Normal)
                    {
                        if (_file_page._lines[_file_page._cursor.Y].Length >= line_length)
                        {
                            _file_page._lines.Insert(_file_page._cursor.Y, new StringBuilder());
                            _file_page._cursor.X = 0;
                            _file_page._cursor.Y++;
                            _file_page._lines[_file_page._cursor.Y].Insert(_file_page._cursor.X, _key.KeyChar);
                            _file_page._cursor.X++;
                        }
                        else
                        {
                            _file_page._lines[_file_page._cursor.Y].Insert(_file_page._cursor.X, _key.KeyChar);
                            _file_page._cursor.X++;
                        }
                    }
                }
            }
        }
    }
}