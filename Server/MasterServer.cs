using NetworkingCore;
using NetworkingCore.Messages;
using Server.DataLayer;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace Server
{
    public class MasterServer
    {
        private Thread serverThread;
        private TcpListener listener;
        private List<NetworkPlayer> clients;
        private Guid serverId;
        private Dictionary<string, IService> services;

        public void Start(int port)
        {
            clients = new List<NetworkPlayer>();

            services = new Dictionary<string, IService>
            {
                { typeof(UserService).FullName, new UserService() }
            };

            Logger.Instance.Log("Connecting to database");

            Database.Instance.Connect();

            Database.Instance.CheckAndUpdate();

            serverId = Guid.NewGuid();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();

            Console.WriteLine("Listening for connections...");

            serverThread = new Thread(Run);
            serverThread.Start();
        }

        private void Run()
        {
            while (true)
            {
                var client = listener.AcceptTcpClient();
                var player = new NetworkPlayer(client);
                player.Id = Guid.NewGuid();

                Console.WriteLine("Player connected with id: " + player.Id);

                player.Send(new NetworkMessage(MessageTypes.Connected, player.Id.ToString(), serverId, Guid.NewGuid()));
                player.DataReceived += Player_DataReceived;
                clients.Add(player);
            }
        }

        private void Player_DataReceived(object sender, NetworkDataRecievedEventArgs e)
        {
            if (e.Message.MessageType == MessageTypes.Generic)
            {
                var messageData = Serializer.Deserialize<GenericMessageData>(e.Message.Data);
                IService service = services[messageData.TargetClass];
                var target = Assembly.GetExecutingAssembly().GetTypes().First(type => type.FullName == messageData.TargetClass);
                var result = target.GetMethod(messageData.TargetMethod).Invoke(service, messageData.Parameters);

                clients.First(x => x.Id == e.Message.SenderId).Send(new NetworkMessage(MessageTypes.Test, result.ToString(), Guid.NewGuid(), Guid.NewGuid()));
            }
            else
            {
                throw new InvalidOperationException("Bad message");
            }
        }
    }
}
