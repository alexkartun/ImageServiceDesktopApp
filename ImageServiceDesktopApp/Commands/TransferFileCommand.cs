using ImageServiceDesktopApp.Loggers.Models;
using ImageServiceDesktopApp.Models;

namespace ImageServiceDesktopApp.Commands
{
    public class TransferFileCommand : ICommand
    {
        private IImageServiceModel imageModal;

        public TransferFileCommand(IImageServiceModel modal)
        {
            imageModal = modal;
        }

        public string Execute(string[] args, out MessageTypeEnum result)
        {
            return imageModal.TransferFile(args, out result);
        }
    }
}