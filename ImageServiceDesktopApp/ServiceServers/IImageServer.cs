namespace ImageServiceDesktopApp.ServiceServers
{
    public interface IImageServer
    {
        /// <summary>
        /// Start the service. Start listening to directories.
        /// </summary>
        void StartService();
        /// <summary>
        /// Stop the service. Stop listening to directories.
        /// </summary>
        void StopService();
    }
}
