using System.Text;

namespace Iv.TextComponent;

public class TextPage
{
    public readonly int MaxLineHeight = Console.WindowHeight - 3;

    public List<StringBuilder> text;

    public TextPage()
    {
        text = new List<StringBuilder>()
        {
            new StringBuilder()
        };
    }
}