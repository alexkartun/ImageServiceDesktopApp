using ImageServiceDesktopApp.Loggers.Models;
using System;

namespace ImageServiceDesktopApp.Loggers
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs msg = new MessageRecievedEventArgs(message, type);
            MessageRecieved(this, msg);
        }
    }
}

