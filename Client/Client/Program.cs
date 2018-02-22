using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Starting ChatApp client. . .");

            // Get the IP address and port from the user.
            Console.Write("Please enter a ChatApp server IP address: ");
            string IP = Console.ReadLine();
            Console.Write("And now the port please: ");
            string port = Console.ReadLine();

            // Verify if the IP address is valid.
            if (VerifyIpAddr(IP))
            {
                Console.WriteLine("The IP address is valid.");

                // Verify the connection to the IP address.
                Console.WriteLine("Pinging the server. . .");
                if (PingServer(IP))
                {
                    Console.WriteLine("Recieved reply from server.");

                    // Verify the port is valid.
                    if (VerifyPortRange(ConvertPortToInt(port)))
                    {
                        // Start the chat session.
                        Console.WriteLine("Connectiong to server. . .");

                        ChatJoin(IP, ConvertPortToInt(port));
                        
                    } else
                    {
                        End();
                    }
                } else
                {
                    Console.WriteLine("Ping request timed out.");
                    End();
                }
            } else
            {
                Console.WriteLine("The IP address is invalid.");
                Console.WriteLine("Please note that: hostnames and domains are not supported yet.");

                End();
            }
        }
        static bool VerifyIpAddr(string IP)
        {
            string[] splitter = { "." };
            string[] IP_array = IP.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            int scanned_sections = 0;

            Console.WriteLine("Scanning IP address");

            if (IP_array.Length == 4)
            {
                foreach (var a in IP_array)
                {
                    int b;
                    try
                    {
                        b = Convert.ToInt32(a);
                    }
                    catch(FormatException)
                    {
                        Console.WriteLine("\""+ a + "\" at section " + scanned_sections + " is not an integer.");
                        return false;
                    }

                    Console.WriteLine("Scanning section: " + scanned_sections + ".");

                    // Check if the IP sections are inbetween 0 and 255.
                    if (b > 255 || b < 0)
                    {
                        Console.WriteLine(b + " at section " + scanned_sections + " is not inbetween 0 and 255.");
                        return false;
                    }
                    scanned_sections++;
                }
                return true;
            } else
            {
                Console.WriteLine("The IP address is not 4 sections long.");
                return false;
            }
        }
        static int ConvertPortToInt(string port)
        {
            int portInt;
            try
            {
                portInt = Convert.ToInt32(port);
            }
            catch (FormatException)
            {
                return -1;
            }
            return portInt;
        }
        static bool VerifyPortRange(int port)
        {
            Console.WriteLine("Verifying port range. . .");
            
            if (port < 0 || port > 65535)
            {
                Console.WriteLine("port is not inbetween 0 and 65535.");
                return false;
            }

            Console.WriteLine("Port number is valid.");
            return true;
        }
        static bool PingServer(string IP)
        {
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(IP);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
            catch (System.ArgumentException)
            {
                Console.WriteLine("You cannot connect to all interfaces at once.");
                return false;
            }
            return true;
        }
        static void ChatJoin(string IP, int port)
        {
            TcpClient client = new TcpClient();

            try
            {
                client.Connect(IP, port);
                Console.WriteLine("Connected to ChatApp server.");

                NetworkStream inStream = client.GetStream();

                Thread sendMessages = new Thread(() => SendMessages(client, inStream));
                Thread recieveMessahes = new Thread(() => RecieveMessages(client, inStream));
                sendMessages.Start();
                recieveMessahes.Start();
            }
            catch (System.Net.Sockets.SocketException)
            {
                Console.WriteLine("Unable to connect to ChatApp server at: " + IP + ":" + port);
                End();
            }
        }
        static void SendMessages(TcpClient client, NetworkStream inStream)
        {
            byte[] data = new byte[client.ReceiveBufferSize];

            try
            {
                while (true)
                {
                    string message = Console.ReadLine();
                    data = Encoding.ASCII.GetBytes("Client: " + message);
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
        static void RecieveMessages(TcpClient client, NetworkStream inStream)
        {
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
                Console.WriteLine("ChatApp server closed unexpectedly.");
                End();
            }
        }
        static void End()
        {
            // Write somthing when there is nothing else to do.
            Console.Write("There is nothing else to do now.");
            Console.ReadLine();
        }
    }
}
