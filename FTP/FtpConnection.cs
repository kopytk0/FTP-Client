using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FTP
{
    public interface IFtpConnection
    {
        FtpResponse ReceiveResponse();
        FtpResponse SendRequest(string request);
        FtpResponse SendRequest(params string[] request);
        NetworkStream ReceiveDataStream(IPEndPoint endPoint);
    }
    public class FtpConnection : IFtpConnection
    {
        private Socket socket { get; set; }
        private TcpClient Tcp { get; set; }

        public FtpConnection(string host, int port = 21)
        {
            Tcp = new TcpClient(host, port);
            socket = Tcp.Client;
            ReceiveResponse();
        }

        public NetworkStream ReceiveDataStream(IPEndPoint endPoint)
        {
            TcpClient tcpClient = new TcpClient(endPoint.Address.ToString(), endPoint.Port);
            return tcpClient.GetStream();
        }
        public FtpResponse ReceiveResponse()
        {
            byte[] array = new byte[1024];
            StringBuilder stringBuilder = new StringBuilder();
            int received;
            do
            {
                received = socket.Receive(array);
                stringBuilder.Append(Encoding.ASCII.GetString(array, 0, received));
            } while (received == 1024);

            Trace.WriteLine(stringBuilder.ToString());
            return FtpResponse.Parse(stringBuilder.ToString());
        }


        public FtpResponse SendRequest(string request)
        {
            socket.Send(Encoding.ASCII.GetBytes($"{request}\r\n"));
            Trace.WriteLineIf(!request.StartsWith("PASS"), request);

            return ReceiveResponse();
        }
        public FtpResponse SendRequest(params string[] request)
        {
            return this.SendRequest(string.Join(" ", request));
        }
    }
}