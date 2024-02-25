namespace Iv.TextEditor.Command;

public static class CommandMode
{
    public static (int, string) ReadCommand(string cmdline)
    {
        string[] cmdlineSplit = cmdline.Split(' ');
        List<string> args = new List<string>();

        foreach (var i in cmdlineSplit)
        {
            if(i != cmdlineSplit[0])
            {
                args.Add(i);
            }
        }

        foreach (var i in Program.commands)
        {
            if(cmdlineSplit[0] == i.cmdTag)
            {
                (int returnCode, string outputLog) = i.Start(args.ToArray());

                return (returnCode, outputLog);
            }
        }

        return(0, "");
    }
}