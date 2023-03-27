namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ClientSocket client = new ClientSocket();

            client.Connect("127.0.0.1", 5050);

            client.StartSending();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}