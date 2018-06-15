using System;

namespace ImageServiceDesktopApp.Loggers.Models
{
    public class MessageRecievedEventArgs : EventArgs
    {

        public string Message { get; set; }
        public MessageTypeEnum Status { get; set; }

        public MessageRecievedEventArgs(string msg, MessageTypeEnum st)
        {
            Message = msg;
            Status = st;
        }

    }
}
