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

        public void Execute(string[] args)
        {
            imageModal.AddFile(args);
        }
    }
}

