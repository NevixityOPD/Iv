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
        private int page_heigth = (Console.WindowHeight - 1) - 2;
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
            }
        }

        private void UpdateBottomBar()
        {
            while (true)
            {
                QuickConsole.FastWrite_Color($" [{_keyboardState}]", new Cursor() { X = 0, Y = (short)(Console.WindowHeight - 1), }, ConsoleColor.White, ConsoleColor.DarkGreen);
                QuickConsole.FastWrite_Color($"Ln {_file_page._cursor.X} Col {_file_page._cursor.Y} ", new Cursor() { X = (short)(Console.WindowWidth - $"Ln {_file_page._cursor.X} Col {_file_page._cursor.Y} ".Length), Y = (short)(Console.WindowHeight - 1), }, ConsoleColor.White, ConsoleColor.Black);
            }
        }

        private void UpdateText()
        {
            while (true)
            {
                for (int i = 0; i < _file_page._lines.Count; i++)
                {
                    QuickConsole.FastWrite_Color($"~ {_file_page._lines[i]}", new Cursor()
                    {
                        X = 0,
                        Y = (short)(i + top_padding)
                    }, ConsoleColor.Black, ConsoleColor.White);
                }

                if (_file_page._cursor.X >= Console.WindowWidth - 3)
                {
                    Console.SetCursorPosition(_file_page._cursor.X + 1, _file_page._cursor.Y + top_padding);
                }
                else
                {
                    Console.SetCursorPosition(_file_page._cursor.X + left_padding, _file_page._cursor.Y + top_padding);
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
                    _file_page._lines.Add(new System.Text.StringBuilder());
                    _file_page._cursor.X = 0;
                    _file_page._cursor.Y++;
                }
                else
                {
                    if (_file_page._lines[_file_page._cursor.Y].Length >= line_length)
                    {
                        _file_page._lines.Add(new System.Text.StringBuilder());
                        _file_page._cursor.X = 0;
                        _file_page._cursor.Y++;
                        _file_page._lines[_file_page._cursor.Y].Append(_key.KeyChar);
                        _file_page._cursor.X++;
                    }
                    else
                    {
                        _file_page._lines[_file_page._cursor.Y].Append(_key.KeyChar);
                        _file_page._cursor.X++;
                    }
                }
            }
        }
    }
}