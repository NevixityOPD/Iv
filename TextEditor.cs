using System.Text;
using Iv.Components;

namespace Iv
{
    public class TextEditor
    {
        //Page
        private Thread _update_text;
        private ManualResetEvent _render_page;
        private bool _page_is_paused = false;

        //Bar
        private BottomBar _bottom_bar;
        private TopBar _top_bar;


        public Page _file_page;
        public KeyboardState _keyboardState = KeyboardState.Normal;

        //Padding
        private int left_padding = 2;
        private int top_padding = 1;
        private int bottom_padding = 1;

        //Limits
        public int page_heigth;
        public int line_length = Console.WindowWidth - 2;

        //Variables
        public int line_padding = 0;

        public TextEditor()
        {
            Console.Clear();

            _bottom_bar = new BottomBar(this);
            _top_bar = new TopBar(this);

            _update_text = new Thread(UpdateText);
            _render_page = new ManualResetEvent(true);

            _bottom_bar.FirstStart();
            _top_bar.FirstStart();

            _file_page = new Page();

            //Calculates variables
            page_heigth = Console.WindowHeight - 1 - (top_padding + bottom_padding);

            Console.Title = $"Iv - {_file_page._title}";
            
            Console.SetCursorPosition(0, 1);

            _update_text.Start();
            MainEditor();
        }

        public void SaveFile()
        {
            using (var i = File.Create(_file_page._directory + '\\' + _file_page._title)) { }

            using (StreamWriter _sr = new StreamWriter(_file_page._directory + '\\' + _file_page._title))
            {
                foreach (var i in _file_page._lines)
                {
                    _sr.WriteLine(i.ToString());
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void UpdateText()
        {
            while (true)
            {
                _render_page.WaitOne();
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
                    if (_keyboardState != KeyboardState.Readonly)
                    {
                        _file_page._file_status = FileStatus.Unsaved;
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
                }
                else if (_key.Key == ConsoleKey.Backspace && _file_page._cursor.X != 0)
                {
                    if (_keyboardState != KeyboardState.Readonly)
                    {
                        _file_page._file_status = FileStatus.Unsaved;
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
                else if (_key.Key == ConsoleKey.Escape)
                {
                    _bottom_bar.PauseBottomBar();
                    _render_page.Reset();

                    if (_file_page._file_status == FileStatus.Unsaved)
                    {
                        QuickConsole.FastWrite_Color("File is unsaved. Are you sure you want to quit?[Y/N]: ", new Cursor()
                        {
                            X = 0,
                            Y = (short)(Console.WindowHeight - 1),
                        }, ConsoleColor.White, ConsoleColor.Black);

                        while (true)
                        {
                            ConsoleKeyInfo _console_key = Console.ReadKey(true);

                            if (_console_key.Key == ConsoleKey.Y)
                            {
                                _bottom_bar.PauseBottomBar();
                                _top_bar.PauseTopBar();
                                _render_page.Reset();
                                Console.Clear();
                                Environment.Exit(0);
                            }
                            else if (_console_key.Key == ConsoleKey.N)
                            {
                                _bottom_bar.ResumeBottomBar();
                                _render_page.Set();
                                break;
                            }
                        }
                    }
                    else
                    {
                        QuickConsole.FastWrite_Color("Are you sure you want to quit?[Y/N]: ", new Cursor()
                        {
                            X = 0,
                            Y = (short)(Console.WindowHeight - 1),
                        }, ConsoleColor.White, ConsoleColor.Black);

                        while (true)
                        {
                            ConsoleKeyInfo _console_key = Console.ReadKey(true);

                            if (_console_key.Key == ConsoleKey.Y)
                            {
                                Console.Clear();
                                Environment.Exit(0);
                            }
                            else if (_console_key.Key == ConsoleKey.N)
                            {
                                _bottom_bar.ResumeBottomBar();
                                _render_page.Set();
                                break;
                            }
                        }
                    }
                }
                else if (_key.Modifiers.HasFlag(ConsoleModifiers.Control) && _key.Key == ConsoleKey.L)
                {
                    _render_page.Reset();

                    string _filename = _bottom_bar.PromptBottomBar("Enter filename (Don't forget file extension!): ", ConsoleColor.White, ConsoleColor.Black, _file_page._title, 128);
                    string _directory = "";

                    if (string.IsNullOrEmpty(_filename) || string.IsNullOrWhiteSpace(_filename))
                    {
                        _bottom_bar.WriteBottomBar("Filename cannot be blank!", ConsoleColor.White, ConsoleColor.DarkRed);
                        Thread.Sleep(2500);
                        _render_page.Set();
                        _bottom_bar.RedrawBottomBar();
                        _bottom_bar.ResumeBottomBar();
                        continue;
                    }

                    if (_filename != _file_page._title)
                    {
                        if (_bottom_bar.ConfirmationBottomBar("Change current existing filename?")) { _file_page._title = _filename; }
                    }

                    if (string.IsNullOrEmpty(_file_page._directory) || string.IsNullOrWhiteSpace(_file_page._directory))
                    {
                        _directory = _bottom_bar.PromptBottomBar("Enter new file directory: ", ConsoleColor.White, ConsoleColor.Black, Environment.CurrentDirectory, 128);
                        _file_page._directory = _directory;
                    }

                    if (!Directory.Exists(_file_page._directory))
                    {
                        _file_page._directory = "";
                        _bottom_bar.WriteBottomBar("Directory does not exist!", ConsoleColor.White, ConsoleColor.DarkRed);
                        Thread.Sleep(2500);
                        _render_page.Set();
                        _bottom_bar.RedrawBottomBar();
                        _bottom_bar.ResumeBottomBar();
                        continue;
                    }

                    //if (File.Exists(_file_page._directory + '\\' + _file_page._title))
                    //{
                    //    _bottom_bar.WriteBottomBar("File already existed!", ConsoleColor.White, ConsoleColor.DarkRed);
                    //    Thread.Sleep(2500);
                    //    _render_page.Set();
                    //    _bottom_bar.RedrawBottomBar();
                    //    _bottom_bar.ResumeBottomBar();
                    //    continue;
                    //}

                    SaveFile();
                    _file_page._file_status = FileStatus.Saved;
                    _render_page.Set();
                    _bottom_bar.RedrawBottomBar();
                    _bottom_bar.ResumeBottomBar();
                }
                else
                {
                    if (_keyboardState != KeyboardState.Readonly)
                    {
                        _file_page._file_status = FileStatus.Unsaved;
                        if (_file_page._lines[_file_page._cursor.Y].Length >= line_length)
                        {
                            if (!char.IsControl(_key.KeyChar))
                            {
                                _file_page._lines.Add(new StringBuilder());
                                _file_page._cursor.X = 0;
                                _file_page._cursor.Y++;
                                _file_page._lines[_file_page._cursor.Y].Insert(_file_page._cursor.X, _key.KeyChar);
                                _file_page._cursor.X++;
                            }
                        }
                        else
                        {
                            if (!char.IsControl(_key.KeyChar))
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
}