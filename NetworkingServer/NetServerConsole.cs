using NetworkingClient.Common;
using NetworkingClientCore;
using System;

namespace NetworkingServer
{
    public class NetServerConsole
    {
        static void Main()
        {
            var server = new Server();
            server.Start(1000);

            while(true)
            {
                string input = Console.ReadLine();

                if(input == "exit")
                    break;

                var message = new NetworkMessage(MessageTypes.Test, input, server.ServerId);
                server.Send(message);
            }
        }
    }
}
