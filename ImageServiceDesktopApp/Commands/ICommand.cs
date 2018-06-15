using ImageServiceDesktopApp.Loggers.Models;

namespace ImageServiceDesktopApp.Commands
{
    public interface ICommand
    {
        string Execute(string[] args, out MessageTypeEnum result);
    }
}
