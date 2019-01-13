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
using System.IO;

namespace Server
{
    public class MasterServer
    {
        private static MasterServer _instance;
        private Thread serverThread;
        private TcpListener listener;
        private Dictionary<string, IService> services;
        private static object lockObject = new object();

        public void Start(int port)
        {
            _instance = this;
            Clients = new ConcurrentDictionary<NetworkId, NetworkPlayer>();

            services = new Dictionary<string, IService>
            {
                { typeof(UserService).Name, new UserService() },
                { typeof(GameService).Name, new GameService() }
            };

            foreach (var service in services.Values)
            {
                service.InitTypes();
            }

            LoadCardDlls();

            Logger.Instance.Log("Connecting to database");
            Database.Instance.Connect();
            Database.Instance.CheckAndUpdate();


            Id = NetworkId.Generate();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();

            Console.WriteLine("Listening for connections...");

            serverThread = new Thread(Run);
            serverThread.Start();
        }

        private static void LoadCardDlls()
        {
            Logger.Instance.Log("Loading card dlls...");

            foreach (var file in Directory.GetFiles("Cards/", "*.dll"))
            {
                var assemblyName = AssemblyName.GetAssemblyName(file);
                Assembly.Load(assemblyName);
            }

            Logger.Instance.Log("Card dlls loaded!");
        }

        private void Run()
        {
            while (true)
            {
                var client = listener.AcceptTcpClient();
                var player = new NetworkPlayer(client);
                
                player.Id = NetworkId.Generate();

                Console.WriteLine("Player connected with id: " + player.Id);

                player.Send(new NetworkMessage(MessageTypes.Connected, player.Id, Id, NetworkId.Generate()));
                player.DataReceived += Player_DataReceived;
                player.OnDisconnected += Player_OnDisconnected;
                Clients.TryAdd(player.Id, player);
            }
        }

        private void Player_OnDisconnected(object sender, NetworkId playerId)
        {
            if(Clients.TryGetValue(playerId, out NetworkPlayer player))
            {
                try
                {
                    player.Disconnect(false);
                }
                catch (Exception)
                {
                    
                }
                finally
                {
                    Clients.TryRemove(playerId, out player);
                    Logger.Instance.Log($"Player with id {playerId} disconnected");
                }
            }
        }

        private void Player_DataReceived(object sender, NetworkDataRecievedEventArgs messageReceivedEvent)
        {
            if (messageReceivedEvent.Message.MessageType == MessageTypes.Generic)
            {
                var messageData = (GenericMessageData)messageReceivedEvent.Message.Data;
                IService service = services[messageData.TargetClass];
                var target = Assembly.GetExecutingAssembly().GetTypes().First(type => type.Name == messageData.TargetClass);
                var result = target.GetMethod(messageData.TargetMethod).Invoke(service, messageData.Parameters);

                if (Clients.TryGetValue(messageReceivedEvent.Message.SenderId, out NetworkPlayer networkPlayer))
                {
                    networkPlayer.Send(new NetworkMessage(MessageTypes.Generic, result, NetworkId.Generate(), NetworkId.Generate(), messageReceivedEvent.Message.MessageId));
                }
            }
            else
            {
                throw new InvalidOperationException("Bad message");
            }
        }

        public static MasterServer Instance
        {
            get
            {
                return _instance;
            }
        }

        public ConcurrentDictionary<NetworkId, NetworkPlayer> Clients { get; private set; }
        public NetworkId Id { get; set; }
    }
}
