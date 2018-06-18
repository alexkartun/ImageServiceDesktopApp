using ImageServiceDesktopApp.Commands.Models;
using ImageServiceDesktopApp.Communication;
using ImageServiceDesktopApp.Controllers;
using ImageServiceDesktopApp.Controllers.Handlers;
using ImageServiceDesktopApp.Loggers;
using ImageServiceDesktopApp.Loggers.Models;
using System.Collections.Generic;
using System.Configuration;

namespace ImageServiceDesktopApp.ServiceServers
{
    public class ImageServer : IImageServer
    {
        private IImageController imageController;
        private ITcpServerChannel channel;
        private List<IDirectoryHandler> directoryHandlers;

        public ImageServer(IImageController controller)
        {
            imageController = controller;
            CreateHandlers();
            channel = new TcpServerChannel();
        }

        /// <summary>
        /// Create all handlers.
        /// </summary>
        private void CreateHandlers()
        {
            directoryHandlers = new List<IDirectoryHandler>();

            string directories = ConfigurationManager.AppSettings["Handler"];
            string[] handlers = directories.Split(';');

            foreach (string handler in handlers)
            {
                IDirectoryHandler dirHandler = new DirectoryHandler(handler);
                dirHandler.CommandRecieved += OnCommandRecieved;
                directoryHandlers.Add(dirHandler);
            }
        }

        public void StartService()
        {
            StartHandlers();
            channel.Start();
        }


        public void StopService()
        {
            StopHandlers();
            channel.Stop();
        }

        /// <summary>
        /// Start listening to handlers.
        /// </summary>
        private void StartHandlers()
        {
            foreach (IDirectoryHandler handler in directoryHandlers)
            {
                handler.StartHandleDirectory();
            }
        }

        /// <summary>
        /// Stop listening to handlers.
        /// </summary>
        private void StopHandlers()
        {
            foreach (IDirectoryHandler handler in directoryHandlers)
            {
                handler.StopHandleDirectory();
            }
        }

        /// <summary>
        /// On command recieved event execute command via controller.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="c_args">Command arguments.</param>
        private void OnCommandRecieved(object sender, CommandRecievedEventArgs c_args)
        {
            imageController.ExecuteCommand(c_args.Command, c_args.Args);
        }
    }
}
