using NetworkingCore;
using System;
using System.Threading;

namespace NetworkingClient
{
    class NetClientConsole
    {
        static GameClient client;

        static void Main()
        {
            var t = new Thread(Run);
            t.Start();

            while(true)
            {
                string input = Console.ReadLine();

                if(input == "exit")
                    break;

                var message = new NetworkMessage(MessageTypes.Test, input, client.Player.Id, NetworkId.Generate());
                client.Send(message);
            }
        }

        static void Run()
        {
            client = new GameClient();
            client.Start("127.0.0.1", 1000);
        }
    }
}
