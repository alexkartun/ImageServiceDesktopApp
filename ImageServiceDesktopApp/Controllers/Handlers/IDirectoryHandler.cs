using ImageServiceDesktopApp.Commands.Models;
using System;

namespace ImageServiceDesktopApp.Controllers.Handlers
{
    public interface IDirectoryHandler
    {
        event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        /// <summary>
        /// Start handle directory with system watcher.
        /// </summary>
        void StartHandleDirectory();
        /// <summary>
        /// Stop handler directory. Dispose system watcher.
        /// </summary>
        void StopHandleDirectory();
    }
}
