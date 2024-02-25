using System.Text;
using Iv.TextEditor.Command;

namespace Iv.TextEditor;

public class TextEditor
{
    public string textTitle;
    public string fileExt;
    public string filePath;

    public Canvas textCanvas;
    public CurrentState textEditorState = CurrentState.INSERT;

    private int CursorTop = 0;
    private int CursorLeft = 2;

    public TextEditor(string textTitle = "NewText", string fileExt = ".txt")
    {
        Console.Title = $"Iv - {textTitle}{fileExt}";

        this.textTitle = textTitle;
        this.fileExt = fileExt;
        filePath = @$"/home/";
        textCanvas = new Canvas();
    }

    public void Start()
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        for(int e = 0; e < Console.WindowWidth - 1; e++) { Console.Write(' '); }
        Console.SetCursorPosition((Console.WindowWidth - 1) - $"ln {CursorTop + 1}, col {CursorLeft - 1}".Length, Console.WindowHeight - 1);
        Console.Write($"ln {CursorTop + 1}, col {CursorLeft - 1}");
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write(" " + textEditorState);
        Console.ResetColor();

        textCanvas.Render(CursorTop, CursorLeft);
        
        ConsoleKeyInfo Keyboard = Console.ReadKey(true);

        if(Keyboard.Key == ConsoleKey.Enter)
        {
            if(textEditorState != CurrentState.READONLY)
            {
                textCanvas.page[textCanvas.CurrentPage].text.Insert(CursorTop + 1, new StringBuilder());
                CursorTop++;
                CursorLeft = 2;
            }
        }
        else if(Keyboard.Key == ConsoleKey.Backspace)
        {
            if(textEditorState != CurrentState.READONLY)
            {
                if(textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length != 0 || CursorTop != 0)
                {
                    if(textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length != 0)
                    {
                        if(CursorLeft != 2) { CursorLeft--; }

                        if(CursorLeft == 2)
                        {
                            textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Remove(0, 1); 
                            Console.Write("\b ");
                        }
                        else if (CursorLeft >= 2)
                        {
                            textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Remove(CursorLeft - 2, 1);
                            Console.Write("\b ");
                        }
                        else if(CursorLeft < textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length - 3)
                        {
                            textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Remove(CursorLeft - 2, 1);
                        }

                        for(int i = 0; i < Console.WindowWidth - 2; i++) { Console.Write(' '); }

                        if(textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length == 0)
                        {
                            if(CursorLeft == 2)
                            {   
                                if(CursorTop != 0)
                                {
                                    CursorTop--;
                                    CursorLeft = textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length + 2;
                                }
                            }
                        }
                    }
                }
            }
        }
        else if(Keyboard.Key == ConsoleKey.UpArrow)
        {
            if(CursorTop != 0) 
            { 
                CursorTop--;
                if(Console.CursorLeft > textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length) { CursorLeft = textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length + 2; } 
            }
        }
        else if(Keyboard.Key == ConsoleKey.DownArrow)
        {
            if(CursorTop != textCanvas.page[textCanvas.CurrentPage].text.Count - 1) 
            { 
                CursorTop++;
                if(Console.CursorLeft > textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length) { CursorLeft = textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length + 2; } 
            }
        }
        else if(Keyboard.Key == ConsoleKey.LeftArrow)
        {
            if(CursorLeft != 2) { CursorLeft--; }
        }
        else if(Keyboard.Key == ConsoleKey.RightArrow)
        {
            if(CursorLeft != textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length + 2) { CursorLeft++; }
            if(CursorLeft == textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length + 3)
            {
                if(CursorTop != textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length) { CursorTop++; }
            }
        }
        else if(Keyboard.Key == ConsoleKey.Escape)
        {
            if(textEditorState == CurrentState.COMMAND)
            {
                textEditorState = CurrentState.INSERT;
            }
            else 
            {
                textEditorState = CurrentState.COMMAND;
            }
        }
        else if(Keyboard.Key == ConsoleKey.Insert)
        {
            if(textEditorState != CurrentState.INSERT)
            {
                textEditorState = CurrentState.INSERT;
            }
            else
            {
                textEditorState = CurrentState.READONLY;
            }
        }
        else
        {
            if(textEditorState != CurrentState.READONLY)
            {
                if(textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length != Console.WindowWidth - 3) 
                {
                    if(CursorLeft - 2 < textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Length)
                    { textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Insert(CursorLeft - 2, Keyboard.KeyChar);  }
                    else { textCanvas.page[textCanvas.CurrentPage].text[CursorTop].Append(Keyboard.KeyChar); }
                }
                else { textCanvas.page[textCanvas.CurrentPage].text.Add(new StringBuilder()); CursorTop++; CursorLeft = 1; }
                CursorLeft++;
            }
        }

        if(textEditorState == CurrentState.COMMAND)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            for(int e = 0; e < Console.WindowWidth - 1; e++) { Console.Write(' '); }
            Console.SetCursorPosition((Console.WindowWidth - 1) - $"ln {CursorTop + 1}, col {CursorLeft - 1}".Length, Console.WindowHeight - 1);
            Console.Write($"ln {CursorTop + 1}, col {CursorLeft - 1}");

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(": ");
            (int returnCode, string outputLog) = CommandMode.ReadCommand(Program.CReadLine());

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            for(int e = 0; e < Console.WindowWidth - 1; e++) { Console.Write(' '); }
            Console.SetCursorPosition((Console.WindowWidth - 1) - $"ln {CursorTop + 1}, col {CursorLeft - 1}".Length, Console.WindowHeight - 1);
            Console.Write($"ln {CursorTop + 1}, col {CursorLeft - 1}");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(outputLog);
            Thread.Sleep(2500);

            textEditorState = CurrentState.INSERT;
            Console.ResetColor();
            Console.Clear();
        }
    } 
}

public enum CurrentState
{
    INSERT,
    COMMAND,
    READONLY
}