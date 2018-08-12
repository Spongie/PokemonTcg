﻿using NetworkingClient.Common;
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
            ServerId = Guid.NewGuid();
            Players = new List<ServerPlayer>(2);
            Actions = new Dictionary<MessageTypes, Action<NetworkMessage>>
            {
                { MessageTypes.RegisterForGame, OnRegister },
                { MessageTypes.Attack, OnAttack },
                { MessageTypes.SelectedActive, OnActiveSelected }
            };
        }

        private void OnActiveSelected(NetworkMessage networkMessage)
        {
            var message = Serializer.Deserialize<ActiveSelectedMessage>(networkMessage.Data);

            gameField.OnActivePokemonSelected(message.Owner, message.ActivePokemon);
        }

        private void OnAttack(NetworkMessage message)
        {
            var attackMessage = Serializer.Deserialize<AttackMessage>(message.Data);

            gameField.Attack(attackMessage.Attack);

            SendGameToPlayers();
        }

        private void OnRegister(NetworkMessage message)
        {
            var registrationMessage = Serializer.Deserialize<RegisterMessage>(message.Data);

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
            gameField.Init();
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

                player.Id = Guid.NewGuid();
                player.Send(new NetworkMessage(MessageTypes.Connected, player.Id.ToString(), ServerId, Guid.NewGuid()));
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

                var networkMessage = new NetworkMessage(MessageTypes.GameUpdate, Serializer.Serialize(message), ServerId, Guid.NewGuid());
                player.Send(networkMessage);
            }
        }

        public Guid ServerId { get; protected set; }
    }
}