using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP
{
    internal static class Consts
    {
        internal static class Commands
        {
            public const string Passive = "PASV";
            public const string Password = "PASS";
            public const string List = "MLSD";
            public const string GetFile = "RETR";
            public const string Login = "USER";
            public const string Close = "QUIT";
            public const string SendFile = "STOR";
        }
    }
}
