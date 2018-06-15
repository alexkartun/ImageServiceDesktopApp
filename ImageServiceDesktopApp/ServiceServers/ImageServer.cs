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

        private ILoggingService loggingService;
        private IImageController imageController;
        private ITcpServerChannel channel;
        private List<IDirectoryHandler> directoryHandlers;

        public ImageServer(ILoggingService logger, IImageController controller)
        {
            loggingService = logger;
            imageController = controller;
            CreateHandlers();
            channel = new TcpServerChannel(loggingService);
            channel.CommandRecieved += OnCommandRecieved;
        }

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

        private void StartHandlers()
        {
            foreach (IDirectoryHandler handler in directoryHandlers)
            {
                handler.StartHandleDirectory();
            }
        }

        private void StopHandlers()
        {
            foreach (IDirectoryHandler handler in directoryHandlers)
            {
                handler.StopHandleDirectory();
            }
        }

        private void OnCommandRecieved(object sender, CommandRecievedEventArgs c_args)
        {
            string output = imageController.ExecuteCommand(c_args.Command, c_args.Args, out MessageTypeEnum status);
            loggingService.Log(output, status);
        }
    }
}
