using NetworkingClient;
using NetworkingClient.Common;
using NetworkingClient.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TCGCards.Core;

namespace NetworkingServer
{
    public class Server
    {
        private readonly Dictionary<MessageTypes, Action<NetworkMessage>> Actions;
        private List<NetworkPlayer> Players;
        private Thread serverThread;
        private GameField gameField;
        TcpListener listener;

        public Server()
        {
            ServerId = Guid.NewGuid();
            Players = new List<NetworkPlayer>(2);
            Actions = new Dictionary<MessageTypes, Action<NetworkMessage>>
            {
                { MessageTypes.RegisterName, OnRegisterName }
            };
        }

        private void OnRegisterName(NetworkMessage message)
        {
            var registrationMessage = JsonConvert.DeserializeObject<RegisterMessage>(message.Data);

            NetworkPlayer player = GetPlayerById(message);
            player.Name = registrationMessage.Name;
            player.Player.SetDeck(registrationMessage.Deck);
        }

        private NetworkPlayer GetPlayerById(NetworkMessage message)
        {
            return Players.First(p => p.Id == message.SenderId);
        }

        public void Start(int port)
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();

            serverThread = new Thread(Run);
            serverThread.Start();
        }

        internal void Send(NetworkMessage message)
        {
            foreach(var player in Players)
            {
                player.Send(message);
            }
        }

        private void Run()
        {
            WaitForConnections();
            WaitForRegistrations();
        }

        private void WaitForRegistrations()
        {
            while(!gameField.Players.All(x => x.IsRegistered()))
            {
                Console.WriteLine("waiting for registration messages...");
                Thread.Sleep(100);
            }

            Console.WriteLine("Both players registered!");
            Console.WriteLine("Starting game");

            gameField.StartGame();
            SendGameToPlayers();
        }

        private void InitGame()
        {
            gameField = new GameField();
            Players[0].Player = gameField.Players[0];
            Players[0].Player.Id = gameField.Players[0].Id;

            Players[1].Player = gameField.Players[1];
            Players[1].Player.Id = gameField.Players[1].Id;
        }

        private void WaitForConnections()
        {
            while(Players.Count < 2)
            {
                Console.WriteLine("listening...");
                var client = listener.AcceptTcpClient();

                var player = new NetworkPlayer(client);
                player.DataReceived += Player_DataReceived;
                Players.Add(player);
                Console.WriteLine("Player connected...");

                player.Id = Guid.NewGuid();
                player.Send(new NetworkMessage(MessageTypes.Connected, player.Id.ToString(), ServerId));

                InitGame();
                Console.WriteLine("Both players connected");
            }
        }

        private void Player_DataReceived(object sender, NetworkDataRecievedEventArgs e)
        {
            Console.WriteLine(((NetworkPlayer)sender).Id + ": " + e.Message.Data);

            if(Actions.ContainsKey(e.Message.MessageType))
                Actions[e.Message.MessageType].Invoke(e.Message);
        }

        private void SendGameToPlayers()
        {
            foreach(var player in Players)
            {
                var message = new GameFieldMessage(gameField);

                var networkMessage = new NetworkMessage(MessageTypes.GameUpdate, JsonConvert.SerializeObject(message), ServerId);
                player.Send(networkMessage);
            }
        }

        public Guid ServerId { get; protected set; }
    }
}
