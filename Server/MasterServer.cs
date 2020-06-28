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
using System.Diagnostics;

namespace Server
{
    public class MasterServer
    {
        private Thread serverThread;
        private TcpListener listener;
        private Dictionary<string, IService> services;
        private int defaultConsoleTop;

        internal void PrintInfo()
        {
            if (listener == null)
            {
                return;
            }

            var clientsToDisconnect = Clients.Values.Where(x => (DateTime.Now - x.LastMessage).TotalMinutes > 15);

            foreach (var client in clientsToDisconnect)
            {
                client.Disconnect(false);
            }

            Console.CursorTop = defaultConsoleTop;

            Console.WriteLine("Active Connections: " + Clients.Count.ToString().PadLeft(5));
            using (var process = Process.GetCurrentProcess())
            {
                var memory = (process.PrivateMemorySize64 / 1000000).ToString().PadLeft(8);
                Console.WriteLine($"Memory Usage: {memory} MB");
            }
            Console.WriteLine("Active Games: " + ((GameService)services[typeof(GameService).Name]).ActiveGames.Count);
        }

        public void Start(int port)
        {
            Instance = this;
            Clients = new ConcurrentDictionary<NetworkId, NetworkPlayer>();

            services = new Dictionary<string, IService>
            {
                { typeof(GameService).Name, new GameService() },
                { typeof(ImageService).Name, new ImageService() },
                { typeof(CardService).Name, new CardService() },
                { typeof(PlayerService).Name, new PlayerService() },
                { typeof(InfoService).Name, new InfoService() },
            };

            foreach (var service in services.Values)
            {
                service.InitTypes();
            }

            Id = NetworkId.Generate();
            listener = new TcpListener(IPAddress.Parse("0.0.0.0"), port);
            //listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();

            Console.WriteLine("Listening for connections...");

            defaultConsoleTop = Console.CursorTop;

            serverThread = new Thread(Run);
            serverThread.Start();
        }

        private void Run()
        {
            while (true)
            {
                var client = listener.AcceptTcpClient();
                var player = new NetworkPlayer(client);

                player.Id = NetworkId.Generate();

                Logger.Instance.Log("Player connected with id: " + player.Id);

                player.Send(new NetworkMessage(MessageTypes.Connected, player.Id, Id, NetworkId.Generate()));
                player.DataReceived += Player_DataReceived;
                player.OnDisconnected += Player_OnDisconnected;
                Clients.TryAdd(player.Id, player);
            }
        }

        private void Player_OnDisconnected(object sender, NetworkId playerId)
        {
            if (Clients.TryGetValue(playerId, out NetworkPlayer player))
            {
                Clients.TryRemove(playerId, out player);
                player.OnDisconnected -= Player_OnDisconnected;
                player.DataReceived -= Player_DataReceived;
                Logger.Instance.Log($"Player with id {playerId} disconnected");
            }
        }

        private void Player_DataReceived(object sender, NetworkDataRecievedEventArgs messageReceivedEvent)
        {
            if (messageReceivedEvent.Message.MessageType == MessageTypes.Generic)
            {
                var messageData = (GenericMessageData)messageReceivedEvent.Message.Data;
                IService service = services[messageData.TargetClass];
                var target = Assembly.GetExecutingAssembly().GetTypes().First(type => type.Name == messageData.TargetClass);
                object result;

                try
                {
                     result = target.GetMethod(messageData.TargetMethod).Invoke(service, messageData.Parameters);
                }
                catch (Exception e)
                {
                    Logger.Instance.Log(e.Message);

                    if (e.InnerException != null)
                    {
                        Logger.Instance.Log(e.InnerException.Message);
                    }

                    if (Clients.TryGetValue(messageReceivedEvent.Message.SenderId, out NetworkPlayer errorPlayer))
                    {
                        errorPlayer.Send(new ExceptionMessage(e).ToNetworkMessage(Id));
                    }

                    return;
                }

                if (Clients.TryGetValue(messageReceivedEvent.Message.SenderId, out NetworkPlayer networkPlayer))
                {
                    networkPlayer.Send(new NetworkMessage(MessageTypes.Generic, result, NetworkId.Generate(), NetworkId.Generate(), messageReceivedEvent.Message.MessageId)
                    {
                        RequiresResponse = false
                    });
                }
            }
            else
            {
                //throw new InvalidOperationException("Bad message");
            }
        }

        public static MasterServer Instance { get; private set; }

        public ConcurrentDictionary<NetworkId, NetworkPlayer> Clients { get; private set; }
        public NetworkId Id { get; set; }
    }
}
