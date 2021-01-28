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
using System.Threading.Tasks;
using System.Text;

namespace Server
{
    public class MasterServer
    {
        private Thread serverThread;
        private TcpListener listener;
        private int defaultConsoleTop;

        internal void PrintInfo()
        {
            if (listener == null)
            {
                return;
            }

            //Console.CursorTop = defaultConsoleTop;

            //Console.WriteLine("Active Connections: " + Clients.Count.ToString().PadLeft(5));
            //Console.WriteLine("Active Games: " + ((GameService)services[typeof(GameService).Name]).ActiveGames.Count);
        }

        public void Start(int port)
        {
            Instance = this;
            Clients = new ConcurrentDictionary<NetworkId, NetworkPlayer>();

            Services = new Dictionary<string, IService>
            {
                { typeof(GameService).Name, new GameService() },
                { typeof(ImageService).Name, new ImageService() },
                { typeof(CardService).Name, new CardService() },
                { typeof(PlayerService).Name, new PlayerService() },
                { typeof(InfoService).Name, new InfoService() },
            };

            foreach (var service in Services.Values)
            {
                service.InitTypes();
            }

            Id = NetworkId.Generate();
            listener = new TcpListener(IPAddress.Parse("0.0.0.0"), port);
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

                Logger.Instance.Info("Player connected with id: " + player.Id);

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
                Logger.Instance.Info($"Player with id {playerId} disconnected");
            }
        }

        private void Player_DataReceived(object sender, NetworkDataRecievedEventArgs messageReceivedEvent)
        {
            if (messageReceivedEvent.Message.MessageType == MessageTypes.Generic)
            {
                var messageData = (GenericMessageData)messageReceivedEvent.Message.Data;
                IService service = Services[messageData.TargetClass];
                var target = Assembly.GetExecutingAssembly().GetTypes().First(type => type.Name == messageData.TargetClass);

                Task.Run(() => 
                {
                    try
                    {
                        var method = target.GetMethod(messageData.TargetMethod);
                        var parameters = new List<object>(messageData.Parameters);

                        if (method.GetParameters().Length == messageData.Parameters.Length + 1)
                        {
                            parameters.Add(((INetworkPlayer)sender).Id);
                        }

                        object result = method.Invoke(service, parameters.ToArray());

                        if (Clients.TryGetValue(messageReceivedEvent.Message.SenderId, out NetworkPlayer networkPlayer))
                        {
                            networkPlayer.Send(new NetworkMessage(MessageTypes.Generic, result, NetworkId.Generate(), NetworkId.Generate(), messageReceivedEvent.Message.MessageId)
                            {
                                RequiresResponse = false
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        var message = new StringBuilder();
                        message.Append(e.Message);

                        if (e.InnerException != null)
                        {
                            message.Append(Environment.NewLine);
                            message.Append(e.InnerException.Message);
                        }

                        message.Append(Environment.NewLine);
                        message.Append(e.StackTrace);

                        if (e.InnerException != null)
                        {
                            message.Append(e.InnerException.StackTrace);
                        }

                        message.Append(Environment.NewLine);
                        Logger.Instance.Error(message.ToString());

                        if (Clients.TryGetValue(messageReceivedEvent.Message.SenderId, out NetworkPlayer errorPlayer))
                        {
                            errorPlayer.Send(new ExceptionMessage(e).ToNetworkMessage(Id));
                        }
                    }
                });
            }
            else
            {
                //Hmm mauybe do something i dont know
            }
        }

        public static MasterServer Instance { get; private set; }

        public ConcurrentDictionary<NetworkId, NetworkPlayer> Clients { get; private set; }
        public NetworkId Id { get; set; }

        public Dictionary<string, IService> Services { get; private set; }
    }
}
