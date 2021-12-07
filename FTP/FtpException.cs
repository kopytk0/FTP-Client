using System;

namespace FTP
{
    public class FtpException : Exception
    {
        internal FtpException(FtpResponse response) : this(response.Message, response)
        {
        }

        internal FtpException(string message, FtpResponse response) : base(message)
        {
            Response = response;
        }

        public FtpResponse Response { get; }
    }
}