using System;
using Spectre.Console;

namespace Iv.Components
{
    public class BottomBar
    {
        public BottomBar(TextEditor _ted)
        {
            _bottom_bar = new Thread(UpdateBottomBar);
            _render_bottom_bar = new ManualResetEvent(false);
            this._ted = _ted;
        }

        private Thread _bottom_bar;
        private ManualResetEvent _render_bottom_bar;
        private bool _top_bar_is_paused = false;
        private readonly TextEditor _ted;

        private void UpdateBottomBar()
        {
            while (true)
            {
                //Will stall if is in reset state
                _render_bottom_bar.WaitOne();

                QuickConsole.FastDrawLine(Console.WindowHeight - 1, $" [{_ted._keyboardState}]", ConsoleColor.White, (_ted._keyboardState == KeyboardState.Normal) ? ConsoleColor.DarkGreen : ConsoleColor.DarkYellow);
                QuickConsole.FastWrite_Color($"Ln {_ted._file_page._cursor.X} Col {_ted._file_page._cursor.Y} Ph {_ted.page_heigth} Lp {_ted.line_padding} ", new Cursor() { X = (short)(Console.WindowWidth - $"Ln {_ted._file_page._cursor.X} Col {_ted._file_page._cursor.Y} Ph {_ted.page_heigth} Lp {_ted.line_padding} ".Length), Y = (short)(Console.WindowHeight - 1), }, ConsoleColor.White, ConsoleColor.Black);
                Thread.Sleep(20);
            }
        }

        public void FirstStart()
        {
            RedrawBottomBar();
            _bottom_bar.Start();
            _render_bottom_bar.Set();
        }

        public void RedrawBottomBar(ConsoleColor background, ConsoleColor foreground)
        {
            QuickConsole.FastWrite_Color(new string(' ', Console.WindowWidth), new Cursor()
            {
                X = 0,
                Y = (short)(Console.WindowHeight - 1),
            }, background, foreground);
        }

        public void RedrawBottomBar()
        {
            QuickConsole.FastWrite_Color(new string(' ', Console.WindowWidth), new Cursor()
            {
                X = 0,
                Y = (short)(Console.WindowHeight - 1),
            }, ConsoleColor.White, ConsoleColor.Black);
        }

        public void PauseBottomBar()
        {
            if (!_top_bar_is_paused)
            {
                _top_bar_is_paused = true;
                _render_bottom_bar.Reset();
            }
        }

        public void ResumeBottomBar()
        {
            if (_top_bar_is_paused)
            {
                _top_bar_is_paused = false;
                _render_bottom_bar.Set();
            }
        }

        public void WriteBottomBar(string _message)
        {
            PauseBottomBar();
            RedrawBottomBar();

            QuickConsole.FastWrite_Color(_message, new Cursor()
            {
                X = 0,
                Y = (short)(Console.WindowHeight - 1),
            }, ConsoleColor.White, ConsoleColor.Black);
        }

        public void WriteBottomBar(string _message, ConsoleColor background, ConsoleColor foreground)
        {
            PauseBottomBar();
            RedrawBottomBar();

            QuickConsole.FastWrite_Color(_message, new Cursor()
            {
                X = 0,
                Y = (short)(Console.WindowHeight - 1),
            }, background, foreground);
        }

        public string PromptBottomBar(string _prompt, ConsoleColor background, ConsoleColor foreground, string _text_preset, int _char_limit)
        {
            PauseBottomBar();
            RedrawBottomBar();

            QuickConsole.FastWrite_Color(_prompt, new Cursor()
            {
                X = 0,
                Y = (short)(Console.WindowHeight - 1),
            }, background, foreground);

            return QuickConsole.FastReadLine(new Cursor()
            {
                X = (short)_prompt.Length,
                Y = (short)(Console.WindowHeight - 1),
            }, background, foreground, _text_preset, _char_limit);
        }

        public bool ConfirmationBottomBar(string _prompt, ConsoleColor background, ConsoleColor foreground)
        {
            PauseBottomBar();
            RedrawBottomBar();

            WriteBottomBar(_prompt + ": ", background, foreground);

            while (true)
            {
                ConsoleKeyInfo _key = Console.ReadKey(true);

                if (_key.Key == ConsoleKey.Y) { return true; }
                else if (_key.Key == ConsoleKey.N) { break; }
            }

            return false;
        }

        public bool ConfirmationBottomBar(string _prompt)
        {
            PauseBottomBar();
            RedrawBottomBar();

            WriteBottomBar(_prompt + " [Y/N]: ");

            while (true)
            {
                ConsoleKeyInfo _key = Console.ReadKey(true);

                if (_key.Key == ConsoleKey.Y) { return true; }
                else if (_key.Key == ConsoleKey.N) { break; }
            }

            return false;
        }

        public void ErrorBottomBar(string _message)
        {
            WriteBottomBar("Directory does not exist!", ConsoleColor.White, ConsoleColor.DarkRed);
            Thread.Sleep(2500);
            RedrawBottomBar();
            ResumeBottomBar();
        }
    }
}