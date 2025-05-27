using System;
using System.Drawing;
using Iv.Components;

namespace Iv
{
    public class TextEditor
    {
        private Thread _top_bar;
        private Thread _bottom_bar;
        private Page _file_page;
        private KeyboardState _keyboardState = KeyboardState.Normal;

        //Padding
        private int line_padding = 2;
        private int top_padding = 1;
        private int bottom_padding = 1;

        public TextEditor()
        {
            Console.Clear();
            _top_bar = new Thread(UpdateTopBar);
            _bottom_bar = new Thread(UpdateBottomBar);

            _top_bar.Start();
            _bottom_bar.Start();

            _file_page = new Page();

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

            Console.SetCursorPosition(0, top_padding);
        }

        private void UpdateTopBar()
        {
            string text = $"{DateTime.Now.ToString("HH:mm:ss")} | Editing file {_file_page._title} | ";
            while (true)
            {
                text = $"{DateTime.Now.ToString("HH:mm:ss")} | Editing file {_file_page._title} | ";
                QuickConsole.FastWrite_Color(text, new Cursor() { X = 0, Y = 0 }, ConsoleColor.White, ConsoleColor.Black);
                if (_file_page._file_status == FileStatus.Unsaved)
                {
                    QuickConsole.FastWrite_Color("*Unsaved", new Cursor() { X = (short)(text.Length), Y = 0 }, ConsoleColor.White, ConsoleColor.Red);
                }
            }
        }

        private void UpdateBottomBar()
        {
            while (true)
            {
                QuickConsole.FastWrite_Color($"[{_keyboardState}]", new Cursor() { X = 0, Y = (short)(Console.WindowHeight - 1), }, ConsoleColor.White, ConsoleColor.Green);
                QuickConsole.FastWrite_Color($"X: {_file_page._cursor.X} Y: {_file_page._cursor.Y}", new Cursor() { X = (short)(Console.WindowWidth - $"X: {_file_page._cursor.X} Y: {_file_page._cursor.Y}".Length), Y = (short)(Console.WindowHeight - 1), }, ConsoleColor.White, ConsoleColor.Black);
            }
        }
    }
}