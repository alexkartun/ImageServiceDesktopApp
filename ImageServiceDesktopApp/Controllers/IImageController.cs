using ImageServiceDesktopApp.Commands.Models;
using ImageServiceDesktopApp.Loggers.Models;

namespace ImageServiceDesktopApp.Controllers
{
    public interface IImageController
    {
        string ExecuteCommand(CommandEnum commandID, string[] args, out MessageTypeEnum result);
    }
}
