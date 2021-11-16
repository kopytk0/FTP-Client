using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FTP
{
    public class FtpResponse
    {
        public int Code { get; private set; }
        public string Message { get; private set; }

        internal FtpResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }

        internal static FtpResponse Parse(string rawMessage)
        {
            int code;
            if(rawMessage == null || rawMessage.Length < 3 || !Int32.TryParse(rawMessage.Substring(0, 3), out code))
            {
                throw new ArgumentException("Wrong format of server message", nameof(rawMessage));
            }

            var message = rawMessage.Substring(3).Trim();
            return new FtpResponse(code, message);
        }

        public override string ToString()
        {
            return $"{Code} {Message}";
        }
    }
}
