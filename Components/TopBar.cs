using System;
using Spectre.Console;

namespace Iv.Components
{
    public class TopBar
    {
        private Thread _top_bar;
        private ManualResetEvent _render_top_bar;
        private bool _top_bar_is_paused = false;
        private readonly TextEditor _ted;

        public TopBar(TextEditor _ted)
        {
            _top_bar = new Thread(UpdateTopBar);
            _render_top_bar = new ManualResetEvent(false);
            this._ted = _ted;
        }

        private void UpdateTopBar()
        {
            string text = $"Iv | {DateTime.Now:HH:mm:ss} | Editing file \"{_ted._file_page._title}\" | ";
            while (true)
            {
                _render_top_bar.WaitOne();
                text = $" Iv | {DateTime.Now:HH:mm:ss} | Editing file \"{_ted._file_page._title}\" | ";
                QuickConsole.FastWrite_Color(text, new Cursor() { X = 0, Y = 0 }, ConsoleColor.White, ConsoleColor.Black);
                if (_ted._file_page._file_status == FileStatus.Unsaved)
                {
                    QuickConsole.FastWrite_Color("*Unsaved", new Cursor() { X = (short)text.Length, Y = 0 }, ConsoleColor.White, ConsoleColor.DarkRed);
                }
                else if (_ted._file_page._file_status == FileStatus.Saved)
                {
                    QuickConsole.FastWrite_Color(new string(' ', "*Unsaved".Length), new Cursor() { X = (short)text.Length, Y = 0 }, ConsoleColor.White, ConsoleColor.DarkRed);
                }

#pragma warning disable CA1416 // Validate platform compatibility
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
#pragma warning restore CA1416 // Validate platform compatibility
                Thread.Sleep(150);
            }
        }

        public void FirstStart()
        {
            RedrawTopBar();
            _top_bar.Start();
            _render_top_bar.Set();
        }

        public void RedrawTopBar(ConsoleColor background, ConsoleColor foreground)
        {
            QuickConsole.FastWrite_Color(new string(' ', Console.WindowWidth), new Cursor()
            {
                X = 0,
                Y = 0,
            }, background, foreground);
        }

        public void RedrawTopBar()
        {
            QuickConsole.FastWrite_Color(new string(' ', Console.WindowWidth), new Cursor()
            {
                X = 0,
                Y = 0,
            }, ConsoleColor.White, ConsoleColor.Black);
        }

        public void PauseTopBar()
        {
            if (!_top_bar_is_paused)
            {
                _top_bar_is_paused = true;
                _render_top_bar.Reset();
            }
        }

        public void ResumeTopBar()
        {
            if (_top_bar_is_paused)
            {
                _top_bar_is_paused = false;
                _render_top_bar.Set();
            }
        }

        public void WriteTopBar(string _message, ConsoleColor background, ConsoleColor foreground)
        {
            PauseTopBar();
            RedrawTopBar();

            QuickConsole.FastWrite_Color(_message, new Cursor()
            {
                X = 0,
                Y = 0,
            }, background, foreground);
        }

        public void WriteTopBar(string _message)
        {
            PauseTopBar();
            RedrawTopBar();

            QuickConsole.FastWrite_Color(_message, new Cursor()
            {
                X = 0,
                Y = 0,
            }, ConsoleColor.White, ConsoleColor.Black);
        }
    }
}