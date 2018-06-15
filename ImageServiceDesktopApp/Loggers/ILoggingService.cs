using ImageServiceDesktopApp.Loggers.Models;
using System;

namespace ImageServiceDesktopApp.Loggers
{
    public interface ILoggingService
    {
        // Event handler for updating the event logger of the service.
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        void Log(string message, MessageTypeEnum type);
    }
}

