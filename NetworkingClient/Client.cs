using NetworkingClient.Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace NetworkingClient
{
    public class Client
    {
        private readonly Dictionary<MessageTypes, Action<NetworkMessage>> Actions;

        public Client()
        {
            Actions = new Dictionary<MessageTypes, Action<NetworkMessage>>
            {
                { MessageTypes.Connected, OnConnected }
            };
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
