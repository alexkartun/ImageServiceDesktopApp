using ImageServiceDesktopApp.Commands.Models;
using ImageServiceDesktopApp.Loggers.Models;

namespace ImageServiceDesktopApp.Controllers
{
    public interface IImageController
    {
        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="commandID">Command to execute.</param>
        /// <param name="args">Arguments for the command.</param>
        void ExecuteCommand(CommandEnum commandID, string[] args);
    }
}
