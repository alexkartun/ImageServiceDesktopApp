using ImageServiceDesktopApp.Loggers.Models;
using System;

namespace ImageServiceDesktopApp.Loggers
{
    public interface ILoggingService
    {
        // Event handler for updating the event logger of the service.
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// Log the message to event logger.
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <param name="type">Status of the message.</param>
        void Log(string message, MessageTypeEnum type);
    }
}

