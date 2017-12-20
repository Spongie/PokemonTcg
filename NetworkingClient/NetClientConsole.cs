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

                var data = Encoding.ASCII.GetBytes(input);

                var prefixBytes = BitConverter.GetBytes(data.Length);
                client.GetStream().Write(prefixBytes, 0, prefixBytes.Length);
                client.GetStream().Write(data, 0, data.Length);
            }
        }
    }
}
