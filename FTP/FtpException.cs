using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP
{
    public class FtpException : Exception
    {
        public FtpResponse Response { get; private set; }

        internal FtpException(FtpResponse response) : this(response.Message, response)
        {

        }
        internal FtpException(string message, FtpResponse response) : base(message)
        {
            Response = response;
        }
    }
}
