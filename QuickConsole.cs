using System;
using System.Runtime.InteropServices;
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
        public struct CHAR_INFO {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(2)] public ushort Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMALL_RECT {
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
                buffer[i].Attributes  = attr;
            }

            IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
            var bufSize   = new COORD((short)width, 1);
            var bufCoord  = new COORD(0, 0);
            var writeRect = new SMALL_RECT
            {
                Left   = 0,
                Top    = (short)y,
                Right  = (short)(width - 1),
                Bottom = (short)y
            };

            WriteConsoleOutput(hConsole, buffer, bufSize, bufCoord, ref writeRect);
        }

        public static void FastClear(int y) { FastWrite(new string(' ', Console.WindowWidth), new Cursor { X = 0, Y = (short)y }); }
    }
}