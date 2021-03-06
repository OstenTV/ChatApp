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
        public static void Main()
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

            Thread getNewStream = new Thread(() => GetNewStream(listener));
            getNewStream.Start();
        }
        public static void ToggleThreads(bool toggle, TcpClient client, NetworkStream inStream, TcpListener listener)
        {
            Thread sendMessages = new Thread(() => SendMessages(client, inStream, listener));
            Thread recieveMessages = new Thread(() => RecieveMessages(client, inStream, listener));

            //Toggle the threads.
            if (toggle)
            {
                sendMessages.Start();
                recieveMessages.Start();
            } else if (!toggle)
            {
                sendMessages.Abort();
                recieveMessages.Abort();
            }
        }
        static void SendMessages(TcpClient client, NetworkStream inStream, TcpListener listener)
        {
            byte[] data = new byte[client.ReceiveBufferSize];

            //start sending messages.
            try
            {
                while (true)
                {
                    string message = Console.ReadLine();
                    data = Encoding.ASCII.GetBytes("Server: " + message);
                    inStream.Write(data, 0, data.Length);
                }
            }
            //I don't remember what these exceptions are...
            catch (System.Net.Sockets.SocketException)
            {
                
            }
            catch (System.IO.IOException)
            {
                
            }
        }
        static void RecieveMessages(TcpClient client, NetworkStream inStream, TcpListener listener)
        {
            byte[] data = new byte[client.ReceiveBufferSize];

            //Start recieving messages.
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
                //Handles user disconnect
                Console.WriteLine("A user has disconnected from the server.");
            }
        }
        static void GetNewStream(TcpListener listener)
        {
            while (true)
            {
                //Accept a new connection.
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream inStream = client.GetStream();

                Console.WriteLine("A user has connected to the server.");
                //start the threads.
                ToggleThreads(true, client, inStream, listener);
            }
        }
    }
}
