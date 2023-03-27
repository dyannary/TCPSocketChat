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
            while(true)
            {
                try
                {
                    var client = _serverSocket.Accept();
                    Console.WriteLine("Client accepted");

                    ReceiveMessage(client);
                }
                catch(Exception e) 
                {
                    Console.WriteLine("Error accepting");
                    Console.WriteLine(e.Message);   
                }
            }
        }

        public void ReceiveMessage(Socket client)
        {
            while(true)
            {
                try
                {
                    var bytesReceived = new byte[1024];
                    client.Receive(bytesReceived);
                    var receivedMessage = Encoding.ASCII.GetString(bytesReceived);
                    Console.WriteLine("Message recieved: {0}", receivedMessage);

                    byte[] bytesMessage = Encoding.ASCII.GetBytes(receivedMessage);
                    SendMessage(client, bytesMessage);

                }
                catch(Exception e)
                {
                    Console.WriteLine("Error recieving");
                    Console.WriteLine(e.Message);
                }
            }
        }
        public void SendMessage(Socket client, byte[] bytesMessages) 
        {
            client.Send(bytesMessages);
        }
    }
}
