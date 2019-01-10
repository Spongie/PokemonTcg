using NetworkingClient.Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using TCGCards.Core;
using NetworkingCore;
using TCGCards.Core.Messages;

namespace NetworkingClient
{
    public class GameClient
    {
        public event EventHandler<GameUpdatedEventArgs> OnGameUpdated;

        private readonly Dictionary<MessageTypes, Action<NetworkMessage>> Actions;
        public bool RegistrationSent { get; private set; }

        public GameClient()
        {
            Actions = new Dictionary<MessageTypes, Action<NetworkMessage>>
            {
                { MessageTypes.Connected, OnConnected },
                { MessageTypes.GameUpdate, OnGameServerUpdated }
            };
        }

        private void OnGameServerUpdated(NetworkMessage message)
        {
            //OnGameUpdated?.Invoke(this, new GameUpdatedEventArgs(Serializer.Deserialize<GameFieldMessage>(message.Data).Game));
        }

        public ServerPlayer Player { get; protected set; }

        Thread clientThread;
        
        public void Start(string ipAddress, int port)
        {
            var client = new TcpClient();
            client.Connect(ipAddress, port);

            Player = new ServerPlayer(client);
            Player.DataReceived += Player_DataReceived;

            clientThread = new Thread(Run);
            clientThread.Start();
        }

        public void Register(string v, Deck deck)
        {
            if(RegistrationSent)
                return;

            RegistrationSent = true;
            var registerMessage = new RegisterMessage(v, deck);
            Send(new NetworkMessage(MessageTypes.RegisterForGame, registerMessage, Player.Id, NetworkId.Generate()));
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
            Player.Send(message);
        }

        private void Player_DataReceived(object sender, NetworkDataRecievedEventArgs e)
        {
            Console.WriteLine("Server: " + e.Message.Data);

            if (Actions.ContainsKey(e.Message.MessageType))
                Actions[e.Message.MessageType].Invoke(e.Message);
        }

        private void OnConnected(NetworkMessage message)
        {
            Player.Id = (NetworkId)message.Data;
        }
    }
}
