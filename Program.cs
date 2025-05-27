using System;
using Spectre.Console;

namespace Iv;

public static class Iv_Main
{
    public static void Main(string[] args)
    {
        Console.Clear();

        if (args.Length == 0)
        {
            Execute();
        }
        else
        {

        }
    }

    public static void Execute()
    {
        PrintLogo();
        Console.ReadKey();
        Global._textEditor = new TextEditor();
    }

    private static void PrintLogo()
    {
        QuickConsole.FastWrite("Iv - Simple text editor", new Components.Cursor()
        {
            X = (short)((Console.WindowWidth - "Iv - Simple text editor".Length) / 2),
            Y = 4
        });

        QuickConsole.FastWrite("Made by nevixity_", new Components.Cursor()
        {
            X = (short)((Console.WindowWidth - "Made by nevixity_".Length) / 2),
            Y = 5,
        });

        QuickConsole.FastWrite($"Version {Global.version}", new Components.Cursor()
        {
            X = (short)((Console.WindowWidth - $"Version {Global.version}".Length) / 2),
            Y = 7,
        });

        QuickConsole.FastWrite("Iv is a text editor inspired by Vi", new Components.Cursor()
        {
            X = (short)((Console.WindowWidth - "Iv is a text editor inspired by Vi".Length) / 2),
            Y = 8,
        });
        
        QuickConsole.FastWrite_Color("Press any key to continue", new Components.Cursor()
        {
            X = (short)((Console.WindowWidth - "Press any key to continue".Length) / 2), Y = 11,
        }, ConsoleColor.Black, ConsoleColor.Magenta);
    }
}