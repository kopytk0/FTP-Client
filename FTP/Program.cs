using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security;
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
            var pass = new SecureString();
            pass.AppendChar('1');
            pass.AppendChar('2');
            pass.AppendChar('3');
            pass.AppendChar('4');
            pass.AppendChar('5');
            client.Login("local", pass);
            client.GetFile(@"/plik.txt", @"C:\Users\jakub\Desktop\MemTest\plik.txt");
            /*while (true)
            {
                string command = Console.ReadLine();
                client.SendCommand(command);
            }*/
        }
    }
}
