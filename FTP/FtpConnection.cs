using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace FTP
{
    public interface IFtpConnection
    {
        FtpResponse ReceiveResponse();
        FtpResponse SendRequest(string request);
        FtpResponse SendRequest(params string[] request);
        void SendFile(IPEndPoint endPoint, string filePath);
        NetworkStream ReceiveDataStream(IPEndPoint endPoint);
        Task SendFileAsync(IPEndPoint endPoint, string filePath, Action<byte> progress = null);
    }

    public class FtpConnection : IFtpConnection
    {
        public FtpConnection(string host, int port = 21)
        {
            Tcp = new TcpClient(host, port);
            socket = Tcp.Client;
            ReceiveResponse();
        }

        private Socket socket { get; }
        private TcpClient Tcp { get; }

        public NetworkStream ReceiveDataStream(IPEndPoint endPoint)
        {
            var tcpClient = new TcpClient(endPoint.Address.ToString(), endPoint.Port);
            return tcpClient.GetStream();
        }

        public FtpResponse ReceiveResponse()
        {
            var array = new byte[1024];
            var stringBuilder = new StringBuilder();
            int received;
            do
            {
                received = socket.Receive(array);
                stringBuilder.Append(Encoding.ASCII.GetString(array, 0, received));
            } while (received == 1024);

            Trace.WriteLine(stringBuilder.ToString());
            return FtpResponse.Parse(stringBuilder.ToString());
        }

        public void SendFile(IPEndPoint endPoint, string filePath)
        {
            var tcpClient = new TcpClient(endPoint.Address.ToString(), endPoint.Port);
            tcpClient.Client.SendFile(filePath);
        }
        public async Task SendFileAsync(IPEndPoint endPoint, string filePath, Action<byte> progress = null)
        {
            var tcpClient = new TcpClient(endPoint.Address.ToString(), endPoint.Port);
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                byte[] buffer = new byte[8 * 1024];
                int read = 0;
                long allRead = 0;
                byte progressPercent = 0;
                
                while((read = await fileStream.ReadAsync(buffer, 0, 8 * 1024)) >= 0)
                {
                    if (read == 0)
                    {
                        if (fileStream.Position == fileStream.Length)
                        {
                            break;
                        }
                    }
                    var bufferSegment = new ArraySegment<byte>(buffer, 0, read);
                    await tcpClient.Client.SendAsync(bufferSegment, SocketFlags.None).ConfigureAwait(false);
                    allRead += read;

                    int lastProgress = progressPercent;
                    progressPercent = (byte)((allRead * 100) / fileStream.Length);
                    if (progress != null && lastProgress != progressPercent)
                    {
                        progress.Invoke(progressPercent);
                    }
                }
            }
        }

        public FtpResponse SendRequest(string request)
        {
            socket.Send(Encoding.ASCII.GetBytes($"{request}\r\n"));
            Trace.WriteLineIf(!request.StartsWith("PASS"), request);

            return ReceiveResponse();
        }

        public FtpResponse SendRequest(params string[] request)
        {
            return SendRequest(string.Join(" ", request));
        }
    }
}