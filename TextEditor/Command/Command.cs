namespace Iv.TextEditor.Command;

public abstract class Command
{
    public abstract string cmdTag { get; set; }
    public abstract string cmdDesc { get; set; }
    public abstract (int, string) Start(string[] args);
}