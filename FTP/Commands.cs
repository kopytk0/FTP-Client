using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FTP
{
    public enum CommandType
    {
        Passive,
        FileTransfer,
        Data,
        Custom,
        Other
    }

    interface ICommandStrategy
    {
        void ProcessCommand(Program.Client client, string response);
    }

    class OtherStrategy : ICommandStrategy
    {
        public void ProcessCommand(Program.Client client, string response)
        {
            
        }
    }

    class DataStrategy : ICommandStrategy
    {
        public void ProcessCommand(Program.Client client, string response)
        {
           
            TcpClient tcp = new TcpClient(client.DataIp, client.DataPort);
            using (var stream = tcp.GetStream())
            {
                var buffer = new byte[1024];
                int readed = 0;
                do
                {
                    buffer = new byte[1024];
                    readed = stream.Read(buffer, 0, buffer.Length);
                    Console.Write(Encoding.ASCII.GetString(buffer).TrimEnd((char)0));
                } while (readed == buffer.Length);
            }
            client.GetResult();
        }
    }
    class PassiveStrategy : ICommandStrategy
    {
        public void ProcessCommand(Program.Client client, string response)
        {
            var startIndex = response.LastIndexOf('(');
            var endIndex = response.LastIndexOf(')');
            if (!(startIndex >= 0 && endIndex >= startIndex))
            {
                throw new ExternalException("Wrong server response");
            }

            var args = response.Substring(startIndex + 1, endIndex - startIndex - 1).Split(',', '|');
            int port;
            string ip = client.Ip;
            if (client.LastCommand.RawCommand == "pasv")
            {
                port = Int32.Parse(args[4]) * 256 + Int32.Parse(args[5]);
                ip = string.Format("{0}.{1}.{2}.{3}", args[0], args[1], args[2], args[3]);
            }
            else
            {
                port = Int32.Parse(args[3]);
            }
            client.DataIp = ip;
            client.DataPort = port;

            Console.WriteLine($"Client: Setting data transfer ip to {ip}:{port}");
        }
    }

    class Command
    {
        private ICommandStrategy commandStrategy;

        internal readonly string RawCommand;

        private Program.Client client;

       

        public void ProcessCommand()
        {
            var response = client.GetResult(RawCommand);
            
            commandStrategy.ProcessCommand(client, response);
        }
        public Command(string command, Program.Client client)
        {
            RawCommand = command.ToLower();
            this.client = client;

            switch (command.GetCommandType())
            {
                case CommandType.Passive:
                    commandStrategy = new PassiveStrategy();
                    break;
                case CommandType.Data:
                    commandStrategy = new DataStrategy();
                    break;
                case CommandType.Other:
                    commandStrategy = new OtherStrategy();
                    break;
            }
        }
    }
    
}
