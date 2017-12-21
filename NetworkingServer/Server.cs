using NetworkingClient.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace NetworkingServer
{
    public class Server
    {
        private List<NetworkPlayer> Players;

        public Server()
        {
            Players = new List<NetworkPlayer>(2);
        }

        public void Start(int port)
        {
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();

            while (Players.Count < 2)
            {
                Console.WriteLine("listening...");
                var client = listener.AcceptTcpClient();

                var player = new NetworkPlayer(client);
                player.DataReceived += Player_DataReceived;
                Console.WriteLine("Player connected...");
            }
        }

        private void Player_DataReceived(object sender, NetworkDataRecievedEventArgs e)
        {
            Console.WriteLine(e.Message.Data);
        }
    }
}
