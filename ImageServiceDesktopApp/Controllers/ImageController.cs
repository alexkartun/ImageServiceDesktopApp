using ImageServiceDesktopApp.Commands;
using ImageServiceDesktopApp.Commands.Models;
using ImageServiceDesktopApp.Loggers.Models;
using ImageServiceDesktopApp.Models;
using System.Collections.Generic;

namespace ImageServiceDesktopApp.Controllers
{
    public class ImageController : IImageController
    {
        private Dictionary<CommandEnum, ICommand> commands;

        public ImageController(IImageServiceModel iModal)
        {
            /// Creates an enum-ICommand dictionary. 
            commands = new Dictionary<CommandEnum, ICommand>()
            {
                { CommandEnum.NewFileCommand, new NewFileCommand(iModal) },
                { CommandEnum.TransferFileCommand, new TransferFileCommand(iModal) }
            };
        }

        public string ExecuteCommand(CommandEnum commandID, string[] args, out MessageTypeEnum status)
        {
            ICommand command = commands[commandID];
            return command.Execute(args, out status);
        }
    }
}
