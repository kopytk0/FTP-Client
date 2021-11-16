using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Microsoft.CSharp.RuntimeBinder;

namespace FTP
{

    class Program
    {

        static void Main()
        {
            Console.Write("Ip: ");
            string ip = "127.0.0.1";
            //ip = Console.ReadLine();
            Client client = new Client(ip, 21);
            client.Login("local", "12345");
            var tmp = client.ListFiles("\\");
            /*while (true)
            {
                string command = Console.ReadLine();
                client.SendCommand(command);
            }*/
        }
    }
}
