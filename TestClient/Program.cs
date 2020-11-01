using NetworkingCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TestClient
{
    class Program
    {
        static NetworkPlayer networkPlayer;
        static GameService gameService;

        static void Main(string[] args)
        {
            Console.WriteLine("Perform stresstest?");
            var stress = Console.ReadLine();

            if (stress.Trim() == "yes")
            {
                Console.WriteLine("How many clients?");
                var count = Console.ReadLine();

                for (int i = 0; i < int.Parse(count); i++)
                {
                    var tcp = new TcpClient();
                    //tcp.Connect("85.90.244.171", 80);
                    tcp.Connect("127.0.0.1", 80);
                    var player = new NetworkPlayer(tcp);
                    Thread.Sleep(100);
                }

                Console.WriteLine(count + " players connected");

                Thread.Sleep(100000);

                Console.Read();
                return;
            }

            var thread = new Thread(RunPlayer);
            thread.Start();

            string input;

            do
            {
                input = Console.ReadLine().Trim();

                if (input == "game")
                {
                    PlayGame();
                }
                if (input == "host")
                {
                    Console.Clear();

                    for (int i = 1; i <= 1000; i++)
                    {
                        var xxxx  = new GameService(networkPlayer).HostGame(networkPlayer.Id, new System.Collections.Generic.List<System.Reflection.TypeInfo>());
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine($"{i} / 1000");
                        Thread.Sleep(25);
                    }

                    Console.WriteLine("pass");
                }

            } while (input.ToLower() != "exit");
        }

        static GameField gameField;

        private static void PlayGame()
        {
            var gameId = gameService.GetAvailableGames().First().Id;
            Console.WriteLine("Joining the game");
            var deck = Serializer.Deserialize<List<TypeInfo>>(File.ReadAllText("Karpen.dck"));
            gameField = gameService.JoinTheActiveGame(networkPlayer.Id, gameId, deck);

            Player me = gameField.Players.First(p => p.Id.Equals(networkPlayer.Id));
            PokemonCard starter = me.Hand.OfType<PokemonCard>().Where(p => p.Stage == 0).FirstOrDefault();

            Console.Read();

            Console.WriteLine("Setting active pokemon");
            gameField = gameService.SetActivePokemon(gameId, networkPlayer.Id, starter.Id);

            Console.Read();

            Console.WriteLine("Setting benched pokemon");
            gameField = gameService.AddToBench(gameId, networkPlayer.Id, me.Hand.OfType<PokemonCard>().Where(p => p.Stage == 0).Select(x => x.Id).ToList());

            while (true)
            {
                string input = Console.ReadLine();

                if (input.Trim() == "end")
                {
                    gameService.EndTurn(gameId);
                }
                else if (input.Trim() == "disc")
                {
                    break;
                }
            }

            networkPlayer.Disconnect(true);
        }

        private static void RunPlayer(object obj)
        {
            var tcp = new TcpClient();
            tcp.Connect("127.0.0.1", 80);
            Console.WriteLine("Connected to server");

            networkPlayer = new NetworkPlayer(tcp);
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
                if (e.Message.MessageType == MessageTypes.GameUpdate)
                {
                    gameField = ((GameFieldMessage)e.Message.Data).Game;
                }
            }
        }
    }
}
