namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ServerSocket server = new ServerSocket("127.0.0.1", 5050);

            server.BindAndListen();
            await server.AcceptAndRecieve();
        }
    }
}