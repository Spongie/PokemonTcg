using NetworkingClient.Common;
using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace NetworkingServer
{
    public class GameServerInstance
    {
        private readonly Dictionary<MessageTypes, Action<NetworkMessage>> Actions;
        private List<ServerPlayer> Players;
        private Thread serverThread;
        private GameField gameField;
        TcpListener listener;

        public GameServerInstance()
        {
            ServerId = NetworkId.Generate();
            Players = new List<ServerPlayer>(2);
            Actions = new Dictionary<MessageTypes, Action<NetworkMessage>>
            {
                { MessageTypes.RegisterForGame, OnRegister },
                { MessageTypes.Attack, OnAttack },
                { MessageTypes.SelectedActive, OnActiveSelected },
                { MessageTypes.SelectedBench, OnBenchedSelected }
            };
        }

        private void OnActiveSelected(NetworkMessage networkMessage)
        {
            var message = (ActiveSelectedMessage)networkMessage.Data;

            gameField.OnActivePokemonSelected(message.Owner.Id, message.SelectedPokemon);
        }

        private void OnBenchedSelected(NetworkMessage networkMessage)
        {
            var message = (BenchSelectedMessage)networkMessage.Data;

            gameField.OnBenchPokemonSelected(message.Owner, message.SelectedPokemon);
        }

        private void OnAttack(NetworkMessage message)
        {
            var attackMessage = (AttackMessage)message.Data;

            gameField.Attack(attackMessage.Attack);

            SendGameToPlayers();
        }

        private void OnRegister(NetworkMessage message)
        {
            var registrationMessage = (RegisterMessage)message.Data;

            ServerPlayer player = GetPlayerById(message);
            player.Name = registrationMessage.Name;
            player.Player.SetDeck(registrationMessage.Deck);
        }

        private ServerPlayer GetPlayerById(NetworkMessage message)
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
            gameField = new GameField();
            WaitForConnections();
            WaitForRegistrations();
        }

        private void WaitForRegistrations()
        {
            Console.WriteLine("waiting for registration messages...");

            while(!gameField.Players.All(x => x.IsRegistered()))
            {
                Thread.Sleep(100);
            }

            Console.WriteLine("Both players registered!");
            Console.WriteLine("Starting game");

            gameField.StartGame();
            SendGameToPlayers();
        }

        private void InitGame()
        {
            Players[0].Player = gameField.Players[0];
            gameField.Players[0].SetNetworkPlayer(Players[0]);

            Players[1].Player = gameField.Players[1];
            gameField.Players[1].Id = Players[1].Id;

            gameField.GameState = GameFieldState.WaitingForRegistration;
            SendGameToPlayers();
        }

        private void WaitForConnections()
        {
            while(Players.Count < 2)
            {
                Console.WriteLine("listening...");
                var client = listener.AcceptTcpClient();

                var player = new ServerPlayer(client);
                player.DataReceived += Player_DataReceived;
                Players.Add(player);
                Console.WriteLine("Player connected...");

                player.Id = NetworkId.Generate();
                player.Send(new NetworkMessage(MessageTypes.Connected, player.Id, ServerId, NetworkId.Generate()));
                SendGameToPlayers();
            }

            InitGame();
            Console.WriteLine("Both players connected");
        }

        private void Player_DataReceived(object sender, NetworkDataRecievedEventArgs e)
        {
            if(Actions.ContainsKey(e.Message.MessageType))
            {
                Actions[e.Message.MessageType].Invoke(e.Message);
            }
        }

        private void SendGameToPlayers()
        {
            foreach(var player in Players)
            {
                var message = new GameFieldMessage(gameField);

                var networkMessage = new NetworkMessage(MessageTypes.GameUpdate, message, ServerId, NetworkId.Generate());
                player.Send(networkMessage);
            }
        }

        public NetworkId ServerId { get; protected set; }
    }
}
