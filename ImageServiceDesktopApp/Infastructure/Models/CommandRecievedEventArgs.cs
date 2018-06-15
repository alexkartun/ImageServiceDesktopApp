using System;

namespace ImageServiceDesktopApp.Commands.Models
{
    public class CommandRecievedEventArgs : EventArgs
    {
        public CommandEnum Command { get; set; }
        public string[] Args { get; set; }

        public CommandRecievedEventArgs(CommandEnum a_id, string[] a_args)
        {
            Command = a_id;
            Args = a_args;
        }
    }
}

