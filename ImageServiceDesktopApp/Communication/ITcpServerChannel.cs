using ImageServiceDesktopApp.Commands.Models;
using System;

namespace ImageServiceDesktopApp.Communication
{
    public interface ITcpServerChannel
    {
        event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        void Start();
        void Stop();
    }
}
