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
            
            public string Ip { get; set; }
            internal Socket socket { get; set; }
            private TcpClient tcp { get; set; }
            internal int DataPort { get; set; }

            internal string DataIp { get; set; }
            internal Command LastCommand { get; set; }

            public string GetResult()
            {
                byte[] array = new byte[1024];
                socket.Receive(array);
                var res = Encoding.ASCII.GetString(array).TrimEnd((char)0);
                Console.Write(res);
                return res;
            }

            public string GetResult(string command)
            {
                socket.Send(Encoding.ASCII.GetBytes($"{command}\r\n"));
                return GetResult();
            }
            public void SendCommand(string command)
            {
                LastCommand = new Command(command, this);
                LastCommand.ProcessCommand();
            }


            public Client(string ip, int port)
            {
                Ip = ip;
                tcp = new TcpClient(ip, port);
                socket = tcp.Client;
                byte[] array = new byte[1024];
                socket.Receive(array);
                Console.Write(Encoding.ASCII.GetString(array).TrimEnd((char)0));
            }
        }
        static void Main()
        {
            Console.Write("Ip: ");
            string ip;
            ip = Console.ReadLine();
            Client client = new Client(ip, 21);
            while (true)
            {
                string command = Console.ReadLine();
                client.SendCommand(command);
            }
        }
    }
}
