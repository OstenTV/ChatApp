using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the IP address from the user.
            Console.Write("Please enter a ChatApp server IP address: ");
            string IP = Console.ReadLine();

            // Verify if the IP address is valid.
            if (VerifyIpAddr(IP))
            {
                Console.WriteLine("The IP address is valid.");

                // Verify the connection to the IP address.
                Console.WriteLine("Pinging the server. . .");
                if (CheckConnection(IP))
                {
                    Console.WriteLine("Recieved reply from server.");

                    // Start the chat session.
                    Console.WriteLine("Connectiong to server. . .");
                    ChatJoin(IP);
                } else
                {
                    Console.WriteLine("Ping request timed out.");
                }
            } else
            {
                Console.WriteLine("The IP address invalid.");
            }

            // Write somthing when there is nothing more to do.
            Console.Write("There is nothing else to do now.");
            Console.ReadLine();
        }
        public static bool VerifyIpAddr(string IP)
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
        static bool CheckConnection(string IP)
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
                // Discard PingExceptions and return false;
            }
            return pingable;
        }
        static void ChatJoin(string IP)
        {
            // We will create the connection here.
        }
    }
}
