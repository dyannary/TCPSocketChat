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

        private readonly List<Socket> _clientSockets = new List<Socket>();

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

        public async Task AcceptAndRecieve()
        {
            //Socket client = _serverSocket.Accept();

            //if (client != null)
            //{
            //    ReceiveMessage(client);
            //}

            while(true)
            {
                try
                {
                    Socket client = await _serverSocket.AcceptAsync();
                    _clientSockets.Add(client);

                    _ = Task.Factory.StartNew(() => ReceiveMessage(client));
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error accepting");
                    Console.WriteLine(e.Message);
                    break;
                }
            }
        }

        public async Task ReceiveMessage(Socket client)
        {
            while(true)
            {
                try 
                {
                    byte[] buffer = new byte[1024];

                    
                    client.Receive(buffer);

                    string receivedMessage = Encoding.UTF8.GetString(buffer);
                    Console.WriteLine("Message from {0} - {1}", client.RemoteEndPoint, receivedMessage);

                    foreach (var clientC in _clientSockets)
                    {
                        if(clientC != client)
                        {
                           // _ = Task.Factory.StartNew(() => SendMessage(clientC, receivedMessage));
                            SendMessage(clientC, receivedMessage);
                        }
                    }
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

            //Task.Factory.StartNew(() => ReceiveMessage(client));
            client.Send(bytesMessage);

        }
    }
}
