using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

namespace Assets.Code
{
    public class NetworkManager : MonoBehaviour
    {
        private NetworkingCore.NetworkPlayer networkPlayer;
        private List<NetworkMessage> messages;
        private Queue<NetworkMessage> messagesToPrint;
        private object lockObject = new object();
        public AsyncUserService userService;
        public AsyncGameService gameService;
        public Dictionary<MessageTypes, IMessageConsumer> messageConsumers;
        private Dictionary<NetworkId, Action<object>> responseMapper;

        internal void RegisterCallback(NetworkId responseId, Action<object> callback)
        {
            responseMapper.Add(responseId, callback);
        }

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            responseMapper = new Dictionary<NetworkId, Action<object>>();
            messageConsumers = new Dictionary<MessageTypes, IMessageConsumer>();
            messages = new List<NetworkMessage>();
            messagesToPrint = new Queue<NetworkMessage>();

            var tcp = new TcpClient();
            tcp.Connect("127.0.0.1", 1565);
            networkPlayer = new NetworkingCore.NetworkPlayer(tcp);

            Me = networkPlayer;;
            userService = new AsyncUserService(networkPlayer);
            gameService = new AsyncGameService(networkPlayer);

            networkPlayer.DataReceived += NetworkPlayer_DataReceived;
        }

        private void NetworkPlayer_DataReceived(object sender, NetworkDataRecievedEventArgs e)
        {
            lock (lockObject)
            {
                messages.Add(e.Message);
                messagesToPrint.Enqueue(e.Message);
            }
        }

        void Update()
        {
            lock (lockObject)
            {
                while (messagesToPrint.Count > 0)
                {
                    Debug.Log(messagesToPrint.Dequeue().Data);
                }
            }

            if (networkPlayer.SpecificResponses.Count > 0)
            {
                foreach (var item in networkPlayer.SpecificResponses)
                {
                    if (responseMapper.ContainsKey(item.ResponseTo))
                    {
                        responseMapper[item.ResponseTo].Invoke(item);
                        responseMapper.Remove(item.ResponseTo);
                    }
                }
            }
        }

        public NetworkMessage TryGetResponse(NetworkId messageId)
        {
            lock (lockObject)
            {
                return networkPlayer.SpecificResponses.FirstOrDefault(message => message.ResponseTo.Equals(messageId));
            }
        }

        public static NetworkManager Instance { get; private set; }
        public NetworkingCore.NetworkPlayer Me { get; internal set; }
    }
}