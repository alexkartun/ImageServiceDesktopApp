using ImageServiceDesktopApp.Communication.Handlers;
using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageServiceDesktopApp.Communication
{
    public class TcpServerChannel : ITcpServerChannel
    {
        private TcpListener server;
        private IClientHandler clientHandler;

        public TcpServerChannel()
        {
            string ip = ConfigurationManager.AppSettings["Ip"];
            string port = ConfigurationManager.AppSettings["Port"];

            server = new TcpListener(IPAddress.Any, Int32.Parse(port));
            clientHandler = new ClientHandler();
        }

        public void Start()
        {
            try
            {
                server.Start();
                new Task(() =>
                {
                    while (true)
                    {
                        try
                        {
                            TcpClient client = server.AcceptTcpClient();
                            clientHandler.HandleClient(client);
                        }
                        catch (SocketException e)
                        {
                            Console.WriteLine(e.Message);
                            break;
                        }
                    }
                }).Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            
        }

        public void Stop()
        {
            server.Stop();
        }
    }
}
