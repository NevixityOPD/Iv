using System.Runtime.CompilerServices;

namespace Iv.TextEditor.Command.Commands;

public class textprop : Command
{
    public override string cmdTag { get; set; } = "textprop";
    public override string cmdDesc { get; set; } = "Set text properties";

    public override (int, string) Start(string[] args)
    {
        string outputLog = "";

        if(args.Length == 0)
        {
            outputLog = "This command require argument!";
        }
        else
        {
            if(args[0] == "set")
            {
                if(args[1] == "dir")
                {
                    if(args.Length > 1)
                    {
                        if(Directory.Exists(args[2]))
                        {
                            Program.ted.filePath = args[2];
                            outputLog = "Property changed.";
                        }
                        else
                        {
                            outputLog = "Directory does not exist.";
                        }
                    }
                    else
                    {
                        outputLog = "This command require 2 arguments!";
                    }
                }
                else if(args[1] == "filename")
                {
                    if(args.Length > 1)
                    {
                        if(!(args[2].Contains('<') ||
                        args[2].Contains('>')) ||
                        args[2].Contains(':') ||
                        args[2].Contains('"') ||
                        args[2].Contains('/') ||
                        args[2].Contains('\\')||
                        args[2].Contains('|') ||
                        args[2].Contains('?'))
                        {
                            Program.ted.textTitle = args[2];
                            outputLog = "Property changed.";
                        }
                        else
                        {
                            outputLog = "File name cannot contain >,:,\",/,\\,|,?";
                        }
                    }
                    else
                    {
                        outputLog = "This command require 2 arguments!";
                    }
                }
                else if(args[1] == "filext")
                {
                    if(args.Length > 1)
                    {
                        if(args[2].Contains('.'))
                        {
                            Program.ted.fileExt = args[2].Replace('.', ' ');
                        }
                        else
                        {
                            Program.ted.fileExt = args[2];
                        }
                    }
                    else
                    {
                        outputLog = "This command require 2 arguments!";
                    }
                }
                else
                {
                    outputLog = "Enter a valid property!";
                }
            }
            else if(args[0] == "get")
            {
                if(args[1] == "dir")
                {
                    outputLog = Program.ted.filePath;
                }
                else if(args[1] == "filename")
                {
                    outputLog = Program.ted.textTitle;
                }
                else if(args[1] == "filext")
                {
                    outputLog = Program.ted.fileExt;
                }
            }
        }

        return (0, outputLog);
    }
}