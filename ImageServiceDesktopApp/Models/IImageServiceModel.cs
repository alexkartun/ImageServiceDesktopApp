using ImageServiceDesktopApp.Loggers.Models;

namespace ImageServiceDesktopApp.Models
{
    public interface IImageServiceModel
    {
        /// <summary>
        /// Add image to OutputDir directory and Thumbnail directory.
        /// </summary>
        /// <param name="args">Arguments of the image</param>
        void AddFile(string[] args);
    }
}
