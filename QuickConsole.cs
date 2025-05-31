using System.Runtime.InteropServices;
using System.Text;
using Iv.Components;

namespace Iv
{
    public class QuickConsole
    {
        const int STD_OUTPUT_HANDLE = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutputCharacter(
        IntPtr hConsoleOutput,
        string lpCharacter,
        uint nLength,
        COORD dwWriteCoord,
        out uint lpNumberOfCharsWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutputAttribute(
        IntPtr hConsoleOutput,
        [In] ushort[] lpAttribute,
        uint nLength,
        COORD dwWriteCoord,
        out uint lpNumberOfAttrsWritten);

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;

            public COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CHAR_INFO
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(2)] public ushort Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMALL_RECT
        {
            public short Left, Top, Right, Bottom;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
            IntPtr hConsoleOutput,
            CHAR_INFO[] lpBuffer,
            COORD dwBufferSize,
            COORD dwBufferCoord,
            ref SMALL_RECT lpWriteRegion);


        public static void FastWrite(string _text, Cursor _cursor)
        {
            IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
            COORD coord = new COORD(_cursor.X, _cursor.Y);

            WriteConsoleOutputCharacter(hConsole, _text, (uint)_text.Length, coord, out _);
        }

        public static void FastWrite_Color(string _text, Cursor _cursor, ConsoleColor background_color, ConsoleColor foreground_color)
        {
            IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
            COORD coord = new COORD(_cursor.X, _cursor.Y);

            WriteConsoleOutputCharacter(hConsole, _text, (uint)_text.Length, coord, out _);

            ushort[] attrs = new ushort[_text.Length];
            for (int i = 0; i < attrs.Length; i++)
            {
                attrs[i] = (ushort)((int)foreground_color | ((int)background_color << 4));
            }

            WriteConsoleOutputAttribute(hConsole, attrs, (uint)attrs.Length, coord, out _);
        }

        public static void FastDrawLine(int y, string text, ConsoleColor background_color, ConsoleColor foreground_color)
        {
            int width = Console.WindowWidth;
            string padded = text.PadRight(width, ' ');

            var buffer = new CHAR_INFO[width];
            ushort attr = (ushort)((int)foreground_color | ((int)background_color << 4));
            for (int i = 0; i < width; i++)
            {
                buffer[i].UnicodeChar = padded[i];
                buffer[i].Attributes = attr;
            }

            IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
            var bufSize = new COORD((short)width, 1);
            var bufCoord = new COORD(0, 0);
            var writeRect = new SMALL_RECT
            {
                Left = 0,
                Top = (short)y,
                Right = (short)(width - 1),
                Bottom = (short)y
            };

            WriteConsoleOutput(hConsole, buffer, bufSize, bufCoord, ref writeRect);
        }

        public static string FastReadLine(Cursor _cursor, ConsoleColor background, ConsoleColor foreground, string _text_preset = "", int _char_limit = 12)
        {
            StringBuilder _string = new StringBuilder(_text_preset);
            ConsoleKeyInfo _key;

            if (_text_preset.Length > _char_limit)
            {
                throw new Exception("Preset text length is longer than it supposed to be!");
            }

            if (_char_limit == 0) { _char_limit = 12; }

            if (!string.IsNullOrEmpty(_text_preset) || !string.IsNullOrWhiteSpace(_text_preset))
            {
                FastWrite_Color(_text_preset, new Cursor()
                {
                    X = _cursor.X,
                    Y = _cursor.Y,
                }, background, foreground);
            }

            Console.SetCursorPosition(_cursor.X + _string.Length, _cursor.Y);

            while (true)
            {
                _key = Console.ReadKey(true);

                if (_key.Key == ConsoleKey.Enter)
                {
                    Console.ResetColor();
                    break;
                }
                else if (_key.Key == ConsoleKey.Backspace && _string.Length != 0)
                {
                    FastWrite_Color(" ", new Cursor()
                    {
                        X = (short)(_cursor.X + _string.Length - 1),
                        Y = _cursor.Y,
                    }, background, foreground);
                    _string.Length--;
                }
                else if (!char.IsControl(_key.KeyChar) && _string.Length != _char_limit)
                {
                    _string.Append(_key.KeyChar);
                    FastWrite_Color(_key.KeyChar.ToString(), new Cursor()
                    {
                        X = (short)(_cursor.X + _string.Length - 1),
                        Y = _cursor.Y,
                    }, background, foreground);
                }

                Console.SetCursorPosition(_cursor.X + _string.Length, _cursor.Y);
            }

            return _string.ToString();
        }

        public static void FastClear(int y) { FastWrite(new string(' ', Console.WindowWidth), new Cursor { X = 0, Y = (short)y }); }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private const int VK_LEFT = 0x25;
        private const int VK_RIGHT = 0x27;
        private const int VK_CONTROL = 0x11;

        public static bool IsCtrlLeftArrowPressed()
        {
            return (GetAsyncKeyState(VK_LEFT) & 0x8000) != 0 &&
                   (GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0;
        }
        
        public static bool IsCtrlRightArrowPressed()
        {
            return (GetAsyncKeyState(VK_RIGHT) & 0x8000) != 0 &&
                   (GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0;
        }
    }
}