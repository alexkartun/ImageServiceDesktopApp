using ImageServiceDesktopApp.Commands.Models;
using ImageServiceDesktopApp.Loggers;
using ImageServiceDesktopApp.Loggers.Models;
using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageServiceDesktopApp.Models
{
    public class ImageServiceModel : IImageServiceModel
    {
        private ILoggingService loggingService;
        private string m_OutputFolder;
        private string m_ThumbnailFolder;
        private int m_thumbnailSize;

        public ImageServiceModel(ILoggingService logger)
        {
            loggingService = logger;

            m_OutputFolder = ConfigurationManager.AppSettings["OutputDir"];
            m_ThumbnailFolder = Path.Combine(m_OutputFolder, "Thumbnails");

            m_thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
        }

        public void AddFile(string[] args)
        {
            string fullPath = args[0];
            string fileName = args[1];

            try
            {
                if (!Directory.Exists(m_OutputFolder))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(m_OutputFolder);
                    dir.Attributes = FileAttributes.Hidden;
                    Directory.CreateDirectory(m_ThumbnailFolder);
                }

                DateTime creation = GetDateTakenFromImage(fullPath);
                string picPathDir = CreateDateDirectory(m_OutputFolder, creation);
                string thumbnailPathDir = CreateDateDirectory(m_ThumbnailFolder, creation);
                string destFilePath = Path.Combine(picPathDir, fileName);

                // Exists a picture with same name. Renaming current pic.
                if (File.Exists(destFilePath))
                {
                    File.Delete(fullPath);
                    loggingService.Log("Image: " + fileName + " is already exist.", MessageTypeEnum.FAIL);
                }
                else
                {
                    File.Move(fullPath, destFilePath);
                    CreateThumbnail(destFilePath, thumbnailPathDir, fileName);
                }
            }
            catch (Exception e)
            {
                loggingService.Log("Error: " + e.Message, MessageTypeEnum.FAIL);
            }
        }

        /// <summary>
        /// Create date directory in OutputDir directory.
        /// </summary>
        /// <param name="srcPath">Source path of the image.</param>
        /// <param name="d">Date time of the image.</param>
        /// <returns>Path to created directory.</returns>
        private static string CreateDateDirectory(string srcPath, DateTime d)
        {
            string month = d.Month.ToString();
            string year = d.Year.ToString();
            string datePathDir = Path.Combine(srcPath, year, month);
            Directory.CreateDirectory(datePathDir);
            return datePathDir;
        }

        /// <summary>
        /// Create thumbnail.
        /// </summary>
        /// <param name="picPath">Path of image.</param>
        /// <param name="thumbDest">Path to destination for saving thumbnail.</param>
        /// <param name="name">Name of the image.</param>
        private void CreateThumbnail(string picPath, string thumbDest, string name)
        {
            using (Image image = Image.FromFile(picPath))
            using (Image thumb = image.GetThumbnailImage(
                m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero))
            {
                thumb.Save(Path.Combine(thumbDest, name));
            }
        }

        private static Regex r = new Regex(":");
        /// <summary>
        /// Get date time of image.
        /// </summary>
        /// <param name="path">Path of image.</param>
        /// <returns>Date time of image.</returns>
        private DateTime GetDateTakenFromImage(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return File.GetCreationTime(path);
        }
    }
}
