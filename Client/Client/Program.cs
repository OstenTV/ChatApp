using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine("Valid IP address");

                // Verify the connection to the IP address.
                if (CheckConnection(IP))
                {
                    Console.WriteLine("successfully established a connection to " + IP);

                    // Make the ChatApp connection here.
                } else
                {
                    Console.WriteLine("Cannot establish a connection to " + IP);
                }
            } else
            {
                Console.WriteLine("Invalid IP address.");
            }

            // Write somthing when there is nothing more to do.
            Console.Write("Program has ended.");
            Console.ReadLine();
        }
        public static bool VerifyIpAddr(string IP)
        {
            string[] splitter = { "." };
            string[] IP_array = IP.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            int scanned_sections = 0;

            if (IP_array.Length == 4)
            {
                foreach (var a in IP_array)
                {
                    // TODO: Verify if a can be converted to an int32.
                    int b = Convert.ToInt32(a);

                    Console.WriteLine("Scanning section: " + scanned_sections + ".");

                    // Check if the IP sections are inbetween 0 and 255.
                    if (b > 255 || b < 0)
                    {
                        Console.WriteLine(b + " at section " + scanned_sections + " was invalid.");
                        return false;
                    }
                    scanned_sections++;
                }
                return true;
            } else
            {
                Console.WriteLine("The IP address was not 4 sections in total.");
                return false;
            }
        }
        static bool CheckConnection(string IP)
        {
            // This is temporary.
            return true;
        }
    }
}
