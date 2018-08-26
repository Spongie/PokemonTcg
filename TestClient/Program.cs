using NetworkingCore;
using NetworkingCore.Messages;
using System;
using System.Net.Sockets;
using System.Threading;

namespace TestClient
{
    class Program
    {
        static NetworkPlayer networkPlayer;
        static UserService userService;
        static void Main(string[] args)
        {
            var thread = new Thread(RunPlayer);
            thread.Start();

            string input;

            do
            {
                input = Console.ReadLine().Trim();

                if (input == "login")
                {
                    string userName = Console.ReadLine().Trim();
                    string password = Console.ReadLine().Trim();

                    Console.WriteLine(userService.Login(userName, password));
                }
                if (input == "register")
                {
                    string userName = Console.ReadLine().Trim();
                    string password = Console.ReadLine().Trim();

                    Console.WriteLine(userService.Register(userName, password));
                }

            } while (input.ToLower() != "exit");
        }

        private static void RunPlayer(object obj)
        {
            var tcp = new TcpClient();
            tcp.Connect("127.0.0.1", 1565);
            Console.WriteLine("Connected to server");

            networkPlayer = new NetworkPlayer(tcp);
            userService = new UserService(networkPlayer);
            networkPlayer.DataReceived += NetworkPlayer_DataReceived;
        }

        private static void NetworkPlayer_DataReceived(object sender, NetworkDataRecievedEventArgs e)
        {
            if (e.Message.MessageType == MessageTypes.Connected)
            {
                networkPlayer.Id = new Guid(e.Message.Data);
                Console.WriteLine("Received connectionId: " + networkPlayer.Id);
            }
            else
            {
                Console.WriteLine("Received data: " + e.Message.Data);
            }
        }
    }
}
