using NetworkingClient.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingClient
{
    class NetClientConsole
    {
        static void Main()
        {
            var client = new TcpClient();
            client.Connect("127.0.0.1", 1000);

            while(true)
            {
                string input = Console.ReadLine();

                if(input == "exit")
                    break;

                var message = new NetworkMessage(MessageTypes.Connected, input);
                message.Send(client.GetStream());
            }
        }
    }
}
