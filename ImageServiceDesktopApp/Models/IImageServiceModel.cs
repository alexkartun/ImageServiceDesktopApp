using ImageServiceDesktopApp.Loggers.Models;

namespace ImageServiceDesktopApp.Models
{
    public interface IImageServiceModel
    {
        string AddFile(string[] args, out MessageTypeEnum result);
        string TransferFile(string[] args, out MessageTypeEnum result);
    }
}
