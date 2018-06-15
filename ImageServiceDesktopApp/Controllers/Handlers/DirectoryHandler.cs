using ImageServiceDesktopApp.Commands.Models;
using System;
using System.IO;

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

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string strFileExt = GetFileExt(e.FullPath);

            if (strFileExt.CompareTo(".jpg") == 0 || strFileExt.CompareTo(".png") == 0
                || strFileExt.CompareTo(".gif") == 0 || strFileExt.CompareTo(".bmp") == 0)
            {
                string[] args = { e.FullPath, e.Name };
                CommandRecieved(this, new CommandRecievedEventArgs(CommandEnum.NewFileCommand, args));
            }
        }

        private static string GetFileExt(string filePath)
        {
            if (filePath == null) return "";

            if (filePath.Length == 0) return "";

            if (filePath.LastIndexOf(".") == -1) return "";

            return filePath.Substring(filePath.LastIndexOf("."));
        }
    }
}
