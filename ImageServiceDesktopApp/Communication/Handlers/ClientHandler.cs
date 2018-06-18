using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceDesktopApp.Communication.Handlers
{
    public class ClientHandler : IClientHandler
    {
        private string directoryHandler;

        public ClientHandler()
        {
            directoryHandler = ConfigurationManager.AppSettings["Handler"].Split(';')[0];
        }


        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                try
                {
                    using (NetworkStream stream = client.GetStream())
                    using (BinaryReader binaryReader = new BinaryReader(stream))
                    {
                        while (true)
                        {
                            Int32 lengthNameBytes = ReadInt32(binaryReader);
                            byte[] nameBytes = binaryReader.ReadBytes(lengthNameBytes);
                            string imageName = Encoding.ASCII.GetString(nameBytes);

                            Int32 lengthImageBytes = ReadInt32(binaryReader);
                            byte[] imageBytes = binaryReader.ReadBytes(lengthImageBytes);

                            using (Image img = ByteArrayToImage(imageBytes))
                            {
                                img.Save(@directoryHandler + @"\" + @imageName);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }).Start();
        }

        /// <summary>
        /// Convert bytes array to image.
        /// </summary>
        /// <param name="byteArray"> Byte array to be converted.</param>
        /// <returns></returns>
        private static Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream mStream = new MemoryStream(byteArray))
            {
                return Image.FromStream(mStream);
            }
        }

        /// <summary>
        /// Read array of 4 bytes from binary reader. Reversing bytes array from big endian to little endian. And convert to integer.
        /// </summary>
        /// <param name="reader">Binary reader.</param>
        /// <returns>Return int.</returns>
        private static Int32 ReadInt32(BinaryReader reader)
        {
            byte[] a32 = reader.ReadBytes(4);
            Array.Reverse(a32);
            return BitConverter.ToInt32(a32, 0);
        }

        /// <summary>
        /// Read array of 8 bytes from binary reader. Reversing bytes array from big endian to little endian. And convert to long.
        /// </summary>
        /// <param name="reader">Binary reader.</param>
        /// <returns>Return long.</returns>
        private static Int64 ReadInt64(BinaryReader reader)
        {
            byte[] a64 = reader.ReadBytes(8);
            Array.Reverse(a64);
            return BitConverter.ToInt64(a64, 0);
        }
    }
}
