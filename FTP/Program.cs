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
        class FileDonwloader 
        {

        }
        public class Client
        {
            
            public string IP { get; set; }
            internal Socket socket { get; set; }
            private TcpClient tcp { get; set; }
            internal int DataPort { get; set; }
            private Command lastCommand { get; set; }

            public void SendCommand(string command)
            {
                lastCommand = new Command(command, this);
                lastCommand.ProcessCommand();
            }


            public Client(string ip, int port)
            {
                IP = ip;
                tcp = new TcpClient(ip, port);
                socket = tcp.Client;
                byte[] array = new byte[1024];
                socket.Receive(array);
                Console.Write(Encoding.ASCII.GetString(array).TrimEnd((char)0));
            }
        }
        static void Main()
        {
            Console.Write("IP: ");
            string ip;
            ip = Console.ReadLine();
            Client client = new Client("127.0.0.1", 21);
            while (true)
            {
                string command = Console.ReadLine();
                client.SendCommand(command);
            }
        }
    }
}
