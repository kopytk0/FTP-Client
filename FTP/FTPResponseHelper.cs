using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace FTP
{
    public static class FtpResponseHelper
    {
        public static bool IsSuccess(this FtpResponse response)
        {
            return response.Code >= 100 && response.Code < 400;
        }

        public static Exception GenerateException(this FtpResponse response)
        {
            if (response.Code.ToString()[1] == '0')
            {
                return new SyntaxErrorException(response.Message);
            }
            if (response.Code.ToString()[1] == '3')
            {
                return new AuthenticationException(response.Message);
            }

            return new Exception(response.Message);
        }

        public static IPEndPoint ParsePasv(this FtpResponse response)
        {
            try
            {
                var startIndex = response.Message.LastIndexOf('(');
                var endIndex = response.Message.LastIndexOf(')');
                if (startIndex == -1 || endIndex == -1 || startIndex > endIndex)
                {
                    throw new SyntaxErrorException("not a pasv idiot");
                }

                var data = response.Message.Substring(startIndex + 1, endIndex - startIndex - 1).Split('\u002C');
                var ip = $"{data[0]}.{data[1]}.{data[2]}.{data[3]}";
                var port = int.Parse(data[4]) * 256 + int.Parse(data[5]);

                return new IPEndPoint(new IPAddress(data.Take(4).Select(byte.Parse).ToArray()), port);
            }
            catch(Exception e)
            {
                throw new FtpException("incorrect format response from server", response);
            }

        }
    }
}
