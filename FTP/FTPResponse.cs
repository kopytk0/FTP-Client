using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP
{
    public class FTPResponse
    {
        public int Code { get; private set; }
        public string Message { get; private set; }

        public FTPResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public static FTPResponse Parse(string rawMessage)
        {
            int code;
            if(rawMessage == null || rawMessage.Length < 3 || !Int32.TryParse(rawMessage.Substring(0, 3), out code))
            {
                throw new ArgumentException("Wrong format of server message", nameof(rawMessage));
            }

            var message = rawMessage.Substring(3).Trim();
            return new FTPResponse(code, message);
        }
    }
}
