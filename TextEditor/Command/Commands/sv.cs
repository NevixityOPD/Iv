using System.Text;

namespace Iv.TextEditor.Command.Commands;

public class sv : Command
{
    public override string cmdTag { get; set; } = "sv";
    public override string cmdDesc { get; set; } = "Save file";

    public override (int, string) Start(string[] args)
    {
        string outputLog = "";

        if(args.Length == 0)
        {
            try
            {
                if(Directory.Exists(Program.ted.filePath))
                {
                    List<string> txtln = new List<string>();

                    for(int i = 0; i < Program.ted.textCanvas.page.Count; i++)
                    {
                        for(int e = 0; e < Program.ted.textCanvas.page[i].text.Count; e++)
                        {
                            txtln.Add(Program.ted.textCanvas.page[i].text[e].ToString());
                        }
                    }

                    File.WriteAllLines(Program.ted.filePath + Program.ted.textTitle + Program.ted.fileExt, txtln);
                    outputLog = "File saved.";
                }
                else
                {
                    outputLog = "Cannot save file, Directory unavailable";
                }
            }
            catch (UnauthorizedAccessException)
            {
                outputLog = "Cannot save file, Permission Denied";
            }
            catch(Exception e)
            {
                outputLog = $"Cannot save file, Exception {e.Message}";
            }
        }
        else
        {
            if(args[0] == "open")
            {
                try
                {

                }
                catch
                {

                }
            }
            else
            {
                try
                {
                    if(Directory.Exists(args[0]))
                    {
                        List<string> txtln = new List<string>();

                        for(int i = 0; i < Program.ted.textCanvas.page.Count; i++)
                        {
                            for(int e = 0; e < Program.ted.textCanvas.page[i].text.Count; e++)
                            {
                                txtln.Add(Program.ted.textCanvas.page[i].text[e].ToString());
                            }
                        }

                        File.WriteAllLines(args[0] + Program.ted.textTitle + Program.ted.fileExt, txtln);
                        outputLog = "File saved.";
                    }
                    else
                    {
                        outputLog = "Cannot save file, Directory unavailable";
                    }
                }
                catch
                {
                    outputLog = $"Cannot save file, IO Exception";
                }
            }
        }

        return (0, outputLog);
    }
}