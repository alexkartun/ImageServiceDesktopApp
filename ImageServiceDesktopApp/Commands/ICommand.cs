using ImageServiceDesktopApp.Loggers.Models;

namespace ImageServiceDesktopApp.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="args">Arguments for the command.</param>
        void Execute(string[] args);
    }
}
