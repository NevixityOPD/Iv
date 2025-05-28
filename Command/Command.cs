using System;

namespace Iv.Command
{
    public abstract class Command
    {
        public Command(string _command_name, string _command_description)
        {
            this._command_name = _command_name;
            this._command_description = _command_description;
        }

        public abstract string _command_name { get; set; }
        public abstract string _command_description { get; set; }

        public abstract int _command_execute();
    }
}