using NetworkingCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using TCGCards;
using TCGCards.Core;

namespace TestClient
{
    class Program
    {
        static NetworkPlayer networkPlayer;
        static UserService userService;
        static GameService gameService;

        static void Main(string[] args)
        {
            var thread = new Thread(RunPlayer);
            thread.Start();

            var x = NetworkId.Generate();

            var s = Serializer.Serialize(x);
            var b = Serializer.Serialize(new object[] {x});

            string input;

            do
            {
                input = Console.ReadLine().Trim();

                if (input == "login")
                {
                    string userName = Console.ReadLine().Trim();
                    string password = Console.ReadLine().Trim();

                    var value = userService.Login(userName, password);

                    Console.WriteLine(value);
                }
                if (input == "register")
                {
                    string userName = Console.ReadLine().Trim();
                    string password = Console.ReadLine().Trim();

                    Console.WriteLine(userService.Register(userName, password));
                }
                if (input == "game")
                {
                    PlayGame();
                }

            } while (input.ToLower() != "exit");
        }

        private static void PlayGame()
        {
            var gameField = gameService.JoinTheActiveGame(networkPlayer.Id);

            ClientPlayer me = gameField.Players.First(p => p.Id.Equals(networkPlayer.Id));
            PokemonCard starter = me.Hand.OfType<PokemonCard>().Where(p => p.Stage == 0).FirstOrDefault();
        }

        private static void RunPlayer(object obj)
        {
            var tcp = new TcpClient();
            tcp.Connect("127.0.0.1", 1565);
            Console.WriteLine("Connected to server");

            networkPlayer = new NetworkPlayer(tcp);
            userService = new UserService(networkPlayer);
            gameService = new GameService(networkPlayer);
            networkPlayer.DataReceived += NetworkPlayer_DataReceived;
        }

        private static void NetworkPlayer_DataReceived(object sender, NetworkDataRecievedEventArgs e)
        {
            if (e.Message.MessageType == MessageTypes.Connected)
            {
                networkPlayer.Id = (NetworkId)e.Message.Data;
                Console.WriteLine("Received connectionId: " + networkPlayer.Id);
            }
            else
            {
                Console.WriteLine("Received data: " + e.Message.Data);
            }
        }

        class Base
        {
            public int Id { get; set; }

            public string Serialize()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        class Master : Base
        {
            public Master()
            {
                Id = 32;
                MyProperty = "AKLSJhdkashdasd";
            }

            public string MyProperty { get; set; }

        }
    }
}
