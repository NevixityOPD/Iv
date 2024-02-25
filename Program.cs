using System;
using System.Text;
using Iv.TextEditor.Command;
using Iv.TextEditor.Command.Commands;

namespace Iv;

public static class Program
{
    public static bool Run = true;
    public static TextEditor.TextEditor ted = new TextEditor.TextEditor();
    public static List<Command> commands = new List<Command>();

    public static void Main()
    {
        commands.Add(new sv());
        commands.Add(new textprop());

        Console.ForegroundColor = ConsoleColor.White;
        Console.Title = "Iv - siv Remake";
        Console.Clear();

        for(int i = 0; i < Console.WindowHeight - 1; i++)
        {
            Console.WriteLine("~");
            if(i == Console.WindowHeight - 2)
                Console.Write("~");
        }
        Console.SetCursorPosition((Console.WindowWidth - $"Iv - siv Remake".Length) / 2, 10);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Iv");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($" - siv Remake");
        Console.SetCursorPosition((Console.WindowWidth - "v2.0".Length) / 2, 11);
        Console.Write("v2.0");
        Console.SetCursorPosition((Console.WindowWidth - "Made By nevixity_".Length) / 2, 11);
        Console.Write("Made By nevixity_");
        Console.SetCursorPosition((Console.WindowWidth - "Press ESC to enter command mode".Length) / 2, 13);
        Console.Write("Press ESC to enter command mode");

        Console.SetCursorPosition((Console.WindowWidth - "<Press any key to start...>".Length) / 2, 17);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("<Press any key to start...>");

        Console.SetCursorPosition(1, 0);

        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey();

        Console.Clear();

        while(Run)
        {
            for(int i = 0; i < Console.WindowHeight - 2; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("~");
                if(i == Console.WindowHeight - 3)
                    Console.Write("~");
            }
            ted.Start();
        }

        Console.Clear();
        foreach(var i in ted.textCanvas.page)
        {
            foreach(var e in i.text)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    public static string CReadLine()
    {
        StringBuilder str = new StringBuilder();

        while(true)
        {
            ConsoleKeyInfo Keyboard = Console.ReadKey();

            if(Keyboard.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if(Keyboard.Key == ConsoleKey.Backspace)
            {
                str.Length--;
                Console.Write("\b \b");
            }
            else
            {
                str.Append(Keyboard.KeyChar);
            }
        }

        return str.ToString();
    }
}