namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServerSocket server = new ServerSocket("127.0.0.1", 5050);

            server.BindAndListen();
            server.AcceptAndRecieve();
        }
    }
}