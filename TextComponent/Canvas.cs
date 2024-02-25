using System;
using System.Text;
using Iv.TextComponent;

namespace Iv.TextEditor;

public class Canvas
{
    public List<TextPage> page;
    public int CurrentPage = 0;

    public Canvas()
    {
        page = new List<TextPage>()
        {
            new TextPage()
        };
    }
    
    public void Render(int CursorTop, int CursorLeft)
    {
        Console.SetCursorPosition(2, 0);
        if(page[CurrentPage].text.Count != 0)
        {
            for(int i = 0; i < page[CurrentPage].text.Count; i++)
            {
                Console.SetCursorPosition(2, i);
                for(int e = 0; e < page[CurrentPage].text[i].Length; e++)
                {
                    Console.Write(page[CurrentPage].text[i][e]);
                }
                for(int s = 0; s < (Console.WindowWidth - 2) - page[CurrentPage].text[i].Length; s++) { Console.Write(' '); }
            }
            Console.SetCursorPosition(CursorLeft, CursorTop);
        }
    } 
}