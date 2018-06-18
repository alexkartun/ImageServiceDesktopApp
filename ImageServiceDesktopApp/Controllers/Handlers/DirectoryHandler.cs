using ImageServiceDesktopApp.Commands.Models;
using System;
using System.IO;
using System.Threading;

namespace ImageServiceDesktopApp.Controllers.Handlers
{
    public class DirectoryHandler : IDirectoryHandler
    {
        private FileSystemWatcher m_dirWatcher;

        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        public DirectoryHandler(string path)
        {
            m_dirWatcher = new FileSystemWatcher(path);
        }

        public void StartHandleDirectory()
        {
            m_dirWatcher.Filter = "*.*";
            m_dirWatcher.Created += new FileSystemEventHandler(OnChanged);
            m_dirWatcher.EnableRaisingEvents = true;
        }

        public void StopHandleDirectory()
        {
            m_dirWatcher.Dispose();
        }

        /// <summary>
        /// On image saved on directory.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e">Arguments of the image.</param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string strFileExt = GetFileExt(e.FullPath);

            if (strFileExt.CompareTo(".jpg") == 0 || strFileExt.CompareTo(".png") == 0
                || strFileExt.CompareTo(".gif") == 0 || strFileExt.CompareTo(".bmp") == 0)
            {
                if (WaitForFile(e.FullPath))
                {
                    string[] args = { e.FullPath, e.Name };
                    CommandRecieved(this, new CommandRecievedEventArgs(CommandEnum.NewFileCommand, args));
                }              
            }
        }

        /// <summary>
        /// Wait till the image is free to be accessed.
        /// </summary>
        /// <param name="fullPath">Path to image.</param>
        /// <returns></returns>
        private static bool WaitForFile(string fullPath)
        {
            int numTries = 0;
            while (true)
            {
                ++numTries;
                try
                {
                    // Attempt to open the file exclusively.
                    using (FileStream fs = new FileStream(fullPath,
                        FileMode.Open, FileAccess.ReadWrite,
                        FileShare.None, 100))
                    {
                        fs.ReadByte();

                        // If we got this far the file is ready
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    
                    if (numTries > 10)
                    {
                        return false;
                    }

                    // Wait for the lock to be released
                    Thread.Sleep(500);
                }
            }

            return true;
        }

        /// <summary>
        /// Get file extenssion.
        /// </summary>
        /// <param name="filePath">Path to image.</param>
        /// <returns></returns>
        private static string GetFileExt(string filePath)
        {
            if (filePath == null) return "";

            if (filePath.Length == 0) return "";

            if (filePath.LastIndexOf(".") == -1) return "";

            return filePath.Substring(filePath.LastIndexOf("."));
        }
    }
}
