using ImageServiceDesktopApp.Commands.Models;
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
        private string m_OutputFolder;
        private string m_ThumbnailFolder;
        private int m_thumbnailSize;

        public ImageServiceModel()
        {
            m_OutputFolder = ConfigurationManager.AppSettings["OutputDir"];
            m_ThumbnailFolder = Path.Combine(m_OutputFolder, "Thumbnails");

            m_thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
        }

        public string TransferFile(string[] args, out MessageTypeEnum result)
        {
            result = MessageTypeEnum.INFO;
            try
            {
                string handler = ConfigurationManager.AppSettings["Handlers"].Split(';')[0];
                byte[] byteArray = Encoding.ASCII.GetBytes(args[0]);
                Image img = ByteArrayToImage(byteArray);
                img.Save(handler);
            }
            catch (Exception)
            {
                result = MessageTypeEnum.FAIL;
            }

            return "Got command: " + (CommandEnum.TransferFileCommand).ToString() + " Args: " + args[0];
        }

        private static Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream mStream = new MemoryStream(byteArray))
            {
                return Image.FromStream(mStream);
            }
        }

        public string AddFile(string[] args, out MessageTypeEnum result)
        {
            result = MessageTypeEnum.INFO;
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
                DateTime creation = GetDateTakenFromImage(fullPath, ref result);
                string picPathDir = CreateDateDirectory(m_OutputFolder, creation);
                string thumbnailPathDir = CreateDateDirectory(m_ThumbnailFolder, creation);
                string destFilePath = Path.Combine(picPathDir, fileName);

                // Exists a picture with same name. Renaming current pic.
                if (File.Exists(destFilePath))
                {
                    result = MessageTypeEnum.FAIL;
                }
                else
                {
                    File.Move(fullPath, destFilePath);
                    CreateThumbnail(destFilePath, thumbnailPathDir, fileName);
                }

            }
            catch (Exception)
            {
                result = MessageTypeEnum.FAIL;
            }

            return "Got command: " + (CommandEnum.NewFileCommand).ToString() + " Args: " + args[0] + ", " + args[1];
        }

        private static string CreateDateDirectory(string srcPath, DateTime d)
        {
            string month = d.Month.ToString();
            string year = d.Year.ToString();
            string datePathDir = Path.Combine(srcPath, year, month);
            Directory.CreateDirectory(datePathDir);
            return datePathDir;
        }


        private void CreateThumbnail(string picPath, string thumbDest, string name)
        {
            Image image = Image.FromFile(picPath);
            Image thumb = image.GetThumbnailImage(
                m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
            thumb.Save(Path.Combine(thumbDest, name));
            image.Dispose();
            thumb.Dispose();
        }

        private static Regex r = new Regex(":");
        public static DateTime GetDateTakenFromImage(string path, ref MessageTypeEnum status)
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
            catch (Exception)
            {
                status = MessageTypeEnum.WARNING;
            }
            return File.GetCreationTime(path);
        }
    }
}
