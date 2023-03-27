using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class ServerSocket
    {
        private readonly Socket _serverSocket;
        private readonly IPEndPoint _serverEndPoint;

        public ServerSocket(string ip, int port)
        {
            var ipAddress = IPAddress.Parse(ip);

            _serverSocket = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream,
                ProtocolType.Tcp);

            _serverEndPoint = new IPEndPoint(ipAddress, port);  
        }

        public void BindAndListen()
        {
            try
            {
                _serverSocket.Bind(_serverEndPoint);
                _serverSocket.Listen();
                Console.WriteLine("Server listening on {0}", _serverEndPoint);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error binding and Listening");
                Console.WriteLine(e.Message);
            }
        }

        public void AcceptAndRecieve()
        {
            Socket client = _serverSocket.Accept();

            if(client != null) 
            {
                ReceiveMessage(client);
            }
        }

        public void ReceiveMessage(Socket client)
        {
            while(true)
            {
                try
                {
                    byte[] buffer = new byte[1024];

                    client.Receive(buffer);

                    string receivedMessage = Encoding.UTF8.GetString(buffer);
                    Console.WriteLine("Message from {0} - {1}", client.RemoteEndPoint, receivedMessage);

                    SendMessage(client, receivedMessage);

                }
                catch(Exception e)
                {
                    Console.WriteLine("Error recieving");
                    Console.WriteLine(e.Message);
                }
            }
        }
        public void SendMessage(Socket client, string message) 
        {
            byte[] bytesMessage = Encoding.ASCII.GetBytes(message);

            client.Send(bytesMessage);
        }
    }
}
