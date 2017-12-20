using System;

namespace NetworkingServer
{
    public class NetServerConsole
    {
        static void Main()
        {
            var server = new Server();
            server.Start(1000);

            Console.Read();
        }
    }
}
