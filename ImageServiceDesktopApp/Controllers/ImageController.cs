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
                { CommandEnum.NewFileCommand, new NewFileCommand(iModal) }
            };
        }

        public void ExecuteCommand(CommandEnum commandID, string[] args)
        {
            ICommand command = commands[commandID];
            command.Execute(args);
        }
    }
}
