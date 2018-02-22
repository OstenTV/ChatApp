﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

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

            Thread sendMessages = new Thread(() => SendMessages(client, inStream));
            sendMessages.Start();
            
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
        static void SendMessages(TcpClient client, NetworkStream inStream)
        {
            try
            {
                while (true)
                {
                    string message = Console.ReadLine();
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    inStream.Write(data, 0, data.Length);
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                
            }
            catch (System.IO.IOException)
            {
                
            }
        }
    }
}
