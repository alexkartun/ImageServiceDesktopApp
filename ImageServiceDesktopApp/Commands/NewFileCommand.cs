using ImageServiceDesktopApp.Loggers.Models;
using ImageServiceDesktopApp.Models;

namespace ImageServiceDesktopApp.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModel imageModal;

        public NewFileCommand(IImageServiceModel modal)
        {
            imageModal = modal;
        }

        public string Execute(string[] args, out MessageTypeEnum result)
        {
            return imageModal.AddFile(args, out result);
        }
    }
}

