using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Port && host");
                SendMessageFromSocket(Convert.ToInt32(Console.ReadLine()),Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static void SendMessageFromSocket(int port,string host)
        {
            byte[] replyBytes = new byte[1024];

            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
            Socket senderHandler = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            senderHandler.Connect(ipEndPoint); //not async
            Console.Write("Введите сообщение: ");
            string message = Console.ReadLine();
            Console.WriteLine("Connected to {0} ", senderHandler.RemoteEndPoint.ToString());
            byte[] msg = Encoding.UTF8.GetBytes(message);
            int bytesSent = senderHandler.Send(msg);

            int bytesReceiveCount = senderHandler.Receive(replyBytes);

            Console.WriteLine("ServerReply: {0}", Encoding.UTF8.GetString(replyBytes, 0, bytesReceiveCount));

            if (!message.Contains("End"))
                SendMessageFromSocket(port,host);

            senderHandler.Shutdown(SocketShutdown.Both);
            senderHandler.Close();
        }
    }
}
