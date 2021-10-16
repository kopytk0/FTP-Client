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
        Pasv,
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
            if (response[0] == '4' || response[0] == '5')
            {
                Console.WriteLine("Client: Server error");
                return;
            }
            TcpClient tcp = new TcpClient(client.IP, client.DataPort);
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
            byte[] array = new byte[1024];
            client.socket.Receive(array);
            Console.Write(Encoding.ASCII.GetString(array).TrimEnd((char)0));
        }
    }
    class PasvStrategy : ICommandStrategy
    {
        public void ProcessCommand(Program.Client client, string response)
        {
            var startIndex = response.LastIndexOf('(');
            var endIndex = response.LastIndexOf(')');
            if (!(startIndex >= 0 && endIndex >= startIndex))
            {
                throw new ExternalException("Wrong server response");
            }

            var args = response.Substring(startIndex + 1, endIndex - startIndex - 1).Split(',');
            int port = Int32.Parse(args[4]) * 256 + Int32.Parse(args[5]);
            client.DataPort = port;
            Console.WriteLine($"Client: Setting port to {port}");
        }
    }

    class Command
    {
        private ICommandStrategy commandStrategy;

        private readonly string rawCommand;

        private Program.Client client;

        string GetResult()
        {
            byte[] array = new byte[1024];
            client.socket.Receive(array);
            var res = Encoding.ASCII.GetString(array).TrimEnd((char)0);
            
            Console.Write(res);
            return res;
        }

        public void ProcessCommand()
        {
            client.socket.Send(Encoding.ASCII.GetBytes($"{rawCommand}\r\n"));
            var response = GetResult();
            commandStrategy.ProcessCommand(client, response);
        }
        public Command(string command, Program.Client client)
        {
            rawCommand = command;
            this.client = client;

            switch (command.GetCommandType())
            {
                case CommandType.Pasv:
                    commandStrategy = new PasvStrategy();
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
    public static class CommandHelper
    {
        public static CommandType GetCommandType(this string command)
        {
            switch (command.ToLower())
            {
                case "pasv":
                    return CommandType.Pasv;
                case "list":
                    return CommandType.Data;
                default:
                    return CommandType.Other;
            }
        }
        
    }
}
