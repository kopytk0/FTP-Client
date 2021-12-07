using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Security;
using System.Text;

namespace FTP
{
    public class Client
    {
        private readonly IFtpConnection ftpConnection;
        public string Ip { get; set; }
        internal IPEndPoint DataEndPoint { get; set; }

        public void GetFile(string ftpPath, string localPath)
        {
            var endPoint = GetDataTransferIP();
            var response = ftpConnection.SendRequest(Consts.Commands.GetFile, ftpPath);

            if (response.IsSuccess())
            {
                using (var stream = ftpConnection.ReceiveDataStream(endPoint))
                {
                    using (var fileStream = File.Create(localPath))
                    {
                        stream.CopyTo(fileStream);
                    }
                }

                response = ftpConnection.ReceiveResponse();
            }

            if (!response.IsSuccess())
            {
                throw new FtpException(response);
            }
            
        }

        public FtpResponse Login(string username, string password = "")
        {

            var response = ftpConnection.SendRequest(Consts.Commands.Login, username);
            if (!response.IsSuccess())
            {
                throw new FtpException("login failed", response);
            }
            
            response = ftpConnection.SendRequest(Consts.Commands.Password, password);
            if (!response.IsSuccess())
            {
                throw new FtpException("pass failed", response);
            }
            return response;
        }

        public FtpResponse Login(string username, SecureString password)
        {
            return this.Login(username, password.ConvertToString());
        }

        private IPEndPoint GetDataTransferIP()
        {
            return ftpConnection.SendRequest(Consts.Commands.Passive).ParsePasv();
        }
        public List<FtpEntry> ListFiles(string path)
        {
            
            var endPoint = GetDataTransferIP();
            ftpConnection.SendRequest(Consts.Commands.List);
            using (var stream = ftpConnection.ReceiveDataStream(endPoint))
            {
                using (StreamReader reader = new StreamReader(stream))
                {

                    var rawList = reader.ReadToEnd().Split('\n').Select(x => x.TrimEnd('\r'))
                        .SkipWhile(string.IsNullOrWhiteSpace).ToList();
                    return rawList.Select(x => FtpEntry.Parse(x)).ToList();
                }
            }
            
            return null;
        }
        public Client(string ip, int port = 21)
        {
            Ip = ip;
            DataEndPoint = new IPEndPoint(0, 0);
            ftpConnection = new FtpConnection(ip, port);
        }

        internal Client(IFtpConnection connection)
        {
            ftpConnection = connection;
        }
    }
}