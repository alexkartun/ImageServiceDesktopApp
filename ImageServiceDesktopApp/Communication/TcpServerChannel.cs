using ImageServiceDesktopApp.Commands.Models;
using ImageServiceDesktopApp.Loggers;
using ImageServiceDesktopApp.Loggers.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceDesktopApp.Communication
{
    public class TcpServerChannel : ITcpServerChannel
    {
        private TcpListener server;
        private ILoggingService log;
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        public TcpServerChannel(ILoggingService logger)
        {
            log = logger;
            string ip = ConfigurationManager.AppSettings["Ip"];
            string port = ConfigurationManager.AppSettings["Port"];

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port));
            server = new TcpListener(ep);
        }

        public void Start()
        {
            server.Start();
            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        log.Log("strt listening", MessageTypeEnum.FAIL);
                        TcpClient client = server.AcceptTcpClient();
                        log.Log("accepted cient", MessageTypeEnum.FAIL);
                        HandleClient(client);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
            }).Start();
        }

        public void Stop()
        {
            server.Stop();
        }

        private void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                try
                {
                    NetworkStream stream = stream = client.GetStream();
                    BinaryReader reader = reader = new BinaryReader(stream);
                    while (true)
                    {
                        int bytesSize = reader.ReadInt32();
                        log.Log(bytesSize.ToString(), MessageTypeEnum.FAIL);
                        byte[] bytesArray = reader.ReadBytes(bytesSize);
                        string stringFromBytes = Encoding.ASCII.GetString(bytesArray);
                        log.Log(stringFromBytes, MessageTypeEnum.FAIL);
                        CommandRecieved(this, new CommandRecievedEventArgs(CommandEnum.TransferFileCommand, new string[] { stringFromBytes }));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    client.Close();
                }

            }).Start();
        }
    }
}
