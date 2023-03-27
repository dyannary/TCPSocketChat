using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class ClientSocket
    {
        private readonly Socket _clientSocket;

        public ClientSocket()
        {
            _clientSocket = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream, 
                ProtocolType.Tcp);

        }
        
        public void Connect(string ip, int port)
        {
            var ipAddress = IPAddress.Parse(ip);
            var endPoint = new IPEndPoint(ipAddress, port);

            try
            {
                _clientSocket.Connect(endPoint);
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Error connecting");
                Console.WriteLine(ex);
            } 
        }

        public void StartSending()
        {
            while(true)
            {
                try
                {
                    byte[] messageReceived = new byte[1024];
                    Task.Run(() => ReceiveMessage(messageReceived));

                    Console.Write("Your message: ");

                    string message = Console.ReadLine() ?? "";
                    byte[] bytesMessage = Encoding.UTF8.GetBytes(message);

                    _clientSocket.Send(bytesMessage);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Error sending data");
                    Console.WriteLine(e);
                }
            }
        }

        public void ReceiveMessage(byte[] messageReceived)
        {
            _clientSocket.Receive(messageReceived);

            var receivedMessage = Encoding.UTF8.GetString(messageReceived);
            Console.WriteLine("\nMessage from {0} - {1}", _clientSocket.RemoteEndPoint, receivedMessage);

        }
    }
}
