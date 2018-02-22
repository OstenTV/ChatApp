using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting ChatApp server. . .");

            // Define which interface to use and what port to listen on.
            System.Net.IPAddress serverAddress = System.Net.IPAddress.Parse("0.0.0.0");
            int port = 4444;

            // Create the listener.
            System.Net.Sockets.TcpListener listener = new System.Net.Sockets.TcpListener(serverAddress, port);
            Console.WriteLine("Listener created.");

            // Start the listener.
            Console.WriteLine("Starting listener. . .");
            listener.Start();
            Console.WriteLine("Listening on: " + serverAddress + ":" + port);
            
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream inStream = client.GetStream();
            byte[] data = new byte[client.ReceiveBufferSize];

            try
            {
                while (true)
                {
                    int bytesRead = inStream.Read(data, 0, System.Convert.ToInt32(client.ReceiveBufferSize));
                    string s_data = Encoding.ASCII.GetString(data, 0, bytesRead);

                    Console.WriteLine(s_data);
                }
            }
            catch (System.IO.IOException)
            {
                listener.Stop();
            }
        }
    }
}
