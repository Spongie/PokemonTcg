using NetworkingClient.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetworkingServer
{
    public class Server
    {
        private readonly Dictionary<MessageTypes, Action<NetworkMessage>> Actions;
        private List<NetworkPlayer> Players;
        private Thread serverThread;
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
            Players.First(p => p.Id == message.SenderId).Name = message.Data;
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
            }

            Console.WriteLine("Both players connected");
        }

        private void Player_DataReceived(object sender, NetworkDataRecievedEventArgs e)
        {
            Console.WriteLine(((NetworkPlayer)sender).Id + ": " + e.Message.Data);

            if(Actions.ContainsKey(e.Message.MessageType))
                Actions[e.Message.MessageType].Invoke(e.Message);
        }

        public Guid ServerId { get; protected set; }
    }
}
