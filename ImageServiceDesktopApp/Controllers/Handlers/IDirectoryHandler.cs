using ImageServiceDesktopApp.Commands.Models;
using System;

namespace ImageServiceDesktopApp.Controllers.Handlers
{
    public interface IDirectoryHandler
    {
        event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        void StartHandleDirectory();
        void StopHandleDirectory();
    }
}
