using System;

namespace FTP
{
    public class FtpResponse
    {
        internal FtpResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; }
        public string Message { get; }

        internal static FtpResponse Parse(string rawMessage)
        {
            int code;
            if (rawMessage == null || rawMessage.Length < 3 || !int.TryParse(rawMessage.Substring(0, 3), out code))
                throw new ArgumentException("Wrong format of server message", nameof(rawMessage));

            var message = rawMessage.Substring(3).Trim();
            return new FtpResponse(code, message);
        }

        public override string ToString()
        {
            return $"{Code} {Message}";
        }
    }
}