using NetworkingClient.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using TCGCards.Core;

namespace NetworkingClient
{
    public class GameUpdatedEventArgs : EventArgs
    {
        public GameUpdatedEventArgs(GameField game)
        {
            Game = game;
        }

        public GameField Game { get; set; }
    }

    public class Client
    {
        public event EventHandler<GameUpdatedEventArgs> OnGameUpdated;

        private readonly Dictionary<MessageTypes, Action<NetworkMessage>> Actions;

        public Client()
        {
            Actions = new Dictionary<MessageTypes, Action<NetworkMessage>>
            {
                { MessageTypes.Connected, OnConnected },
                { MessageTypes.GameUpdate, OnGameServerUpdated }
            };
        }

        private void OnGameServerUpdated(NetworkMessage message)
        {
            OnGameUpdated?.Invoke(this, new GameUpdatedEventArgs(JsonConvert.DeserializeObject<GameField>(message.Data)));
        }

        public NetworkPlayer player { get; protected set; }

        Thread clientThread;
        
        public void Start(string ipAddress, int port)
        {
            var client = new TcpClient();
            client.Connect(ipAddress, port);

            player = new NetworkPlayer(client);
            player.DataReceived += Player_DataReceived;

            clientThread = new Thread(Run);
            clientThread.Start();
        }

        private void Run()
        {
            while(true)
            {
                Thread.Sleep(100);
            }
        }

        internal void Send(NetworkMessage message)
        {
            player.Send(message);
        }

        private void Player_DataReceived(object sender, NetworkDataRecievedEventArgs e)
        {
            Console.WriteLine("Server: " + e.Message.Data);

            if (Actions.ContainsKey(e.Message.MessageType))
                Actions[e.Message.MessageType].Invoke(e.Message);
        }

        private void OnConnected(NetworkMessage message)
        {
            player.Id = new Guid(message.Data);
        }
    }
}
