using ImageServiceDesktopApp.Commands.Models;
using System;

namespace ImageServiceDesktopApp.Communication
{
    public interface ITcpServerChannel
    {
        /// <summary>
        /// Start server. Start listen to clients.
        /// </summary>
        void Start();
        /// <summary>
        /// Stop server. Stop listen to clients.
        /// </summary>
        void Stop();
    }
}
