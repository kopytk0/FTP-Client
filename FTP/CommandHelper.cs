using System.Linq;

namespace FTP
{
    public static class CommandHelper
    {
        public static CommandType GetCommandType(this string command)
        {

            switch (command.ToLower().Split(' ').First())
            {
                case "pasv":
                case "epsv":
                    return CommandType.Passive;
                case "list":
                case "retr":
                    return CommandType.Data;
                default:
                    return CommandType.Other;
            }
        }
    }
}