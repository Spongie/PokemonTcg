using DataLayer;
using NetworkingCore;
using NetworkingCore.Messages;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Entities;

namespace Server
{
    public class MasterServer
    {
        private Thread serverThread;
        private TcpListener listener;
        private ConcurrentDictionary<Guid, NetworkPlayer> clients;
        private Guid serverId;
        private Dictionary<string, IService> services;

        public void Start(int port)
        {
            clients = new ConcurrentDictionary<Guid, NetworkPlayer>();

            services = new Dictionary<string, IService>
            {
                { typeof(UserService).Name, new UserService() }
            };

            Logger.Instance.Log("Connecting to database");
            Database.Instance.Connect();
            Database.Instance.CheckAndUpdate();

            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;

            serverId = Guid.NewGuid();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();

            Console.WriteLine("Listening for connections...");

            serverThread = new Thread(Run);
            serverThread.Start();
        }

        private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            Database.Instance.CheckAndUpdate();
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
                clients.TryAdd(player.Id, player);
            }
        }

        private void Player_DataReceived(object sender, NetworkDataRecievedEventArgs messageReceivedEvent)
        {
            if (messageReceivedEvent.Message.MessageType == MessageTypes.Generic)
            {
                var messageData = Serializer.Deserialize<GenericMessageData>(messageReceivedEvent.Message.Data);
                IService service = services[messageData.TargetClass];
                var target = Assembly.GetExecutingAssembly().GetTypes().First(type => type.Name == messageData.TargetClass);
                var result = target.GetMethod(messageData.TargetMethod).Invoke(service, messageData.Parameters);

                if (clients.TryGetValue(messageReceivedEvent.Message.SenderId, out NetworkPlayer networkPlayer))
                {
                    networkPlayer.Send(new NetworkMessage(MessageTypes.Test, Serializer.Serialize(result), Guid.NewGuid(), Guid.NewGuid(), messageReceivedEvent.Message.MessageId));
                }
            }
            else
            {
                throw new InvalidOperationException("Bad message");
            }
        }
    }
}
