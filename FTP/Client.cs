using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Text;

namespace FTP
{
    public class Client
    {
        private readonly IFtpConnection ftpConnection;
        public string Ip { get; set; }
        internal IPEndPoint DataEndPoint { get; set; }


        public void Login(string username, string password = "")
        {

            var response = ftpConnection.SendRequest($"USER {username}");
            if (!response.IsSuccess())
            {
                throw new FtpException("login failed", response);
            }
            
            response = ftpConnection.SendRequest($"PASS {password}");
            if (!response.IsSuccess())
            {
                throw new FtpException("pass failed", response);
            }
        }

        private IPEndPoint GetDataTransferIP()
        {
            ftpConnection.SendRequest("PASV");
            return ftpConnection.ReceiveResponse().ParsePasv();
        }
        public List<string> ListFiles(string path)
        {
            var endPoint = GetDataTransferIP();
            ftpConnection.SendRequest("LIST");
            ftpConnection.ReceiveResponse();
            var stream = ftpConnection.ReceiveDataStream(endPoint);

            StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd().Split('\n').ToList();

           
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