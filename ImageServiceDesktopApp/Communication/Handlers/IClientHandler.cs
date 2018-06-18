using System.Net.Sockets;

namespace ImageServiceDesktopApp.Communication.Handlers
{
    public interface IClientHandler
    {
        /// <summary>
        /// Handle client. Recieve commands from client.
        /// </summary>
        /// <param name="client">Client to be handled.</param>
        void HandleClient(TcpClient client);
    }
}
