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
            System.Net.IPAddress serverAddress = System.Net.IPAddress.Parse("127.0.0.1");
            int port = 4444;

            // Create the listener.
            System.Net.Sockets.TcpListener listener = new System.Net.Sockets.TcpListener(serverAddress, port);
            Console.WriteLine("Listener created.");

            // Start the listener.
            Console.WriteLine("Starting listener. . .");
            listener.Start();
            Console.WriteLine("Listening on: " + serverAddress + ":" + port);

            TcpClient client = listener.AcceptTcpClient();


        }
    }
}
