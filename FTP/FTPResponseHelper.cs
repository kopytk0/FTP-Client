using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP
{
    public static class FTPResponseHelper
    {
        public static bool IsSuccess(this FTPResponse response)
        {
            return response.Code >= 100 && response.Code < 400;
        }
    }
}
