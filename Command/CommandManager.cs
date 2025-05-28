using System;

namespace Iv.Command
{
    public class CommandManager
    {
        public List<Command> _commands;

        public CommandManager()
        {
            _commands = new List<Command>();
        }        
    }
}