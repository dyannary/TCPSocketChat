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
                Console.WriteLine("Client connected");
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Error connecting");
                Console.WriteLine(ex);
            } 
        }

        public void StartSending()
        {
            Console.Write("Your message: ");
            string message = Console.ReadLine();

            byte[] bytesMessage = Encoding.ASCII.GetBytes(message);

            byte[] messageReceived = new byte[1024];

            try
            {
                _clientSocket.Send(bytesMessage);

                _clientSocket.Receive(messageReceived);

                var receivedMessage = Encoding.ASCII.GetString(messageReceived);
                Console.WriteLine("Message recieved: {0}", receivedMessage);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error sending data");
                Console.WriteLine(e);
            }
        }
    }
}
