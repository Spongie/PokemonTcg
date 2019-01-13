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
        public AsyncImageService imageService;
        public Dictionary<MessageTypes, Action<object>> messageConsumers;
        private Dictionary<NetworkId, Action<object>> responseMapper;

        internal void RegisterCallback(NetworkId responseId, Action<object> callback)
        {
            responseMapper.Add(responseId, callback);
        }

        internal void RegisterCallback(MessageTypes messageTypes, Action<object> callback)
        {
            messageConsumers.Add(messageTypes, callback);
        }

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            responseMapper = new Dictionary<NetworkId, Action<object>>();
            messageConsumers = new Dictionary<MessageTypes, Action<object>>();
            messages = new List<NetworkMessage>();
            messagesToPrint = new Queue<NetworkMessage>();

            var tcp = new TcpClient();
            tcp.Connect("127.0.0.1", 1565);
            networkPlayer = new NetworkingCore.NetworkPlayer(tcp);

            Me = networkPlayer;;
            userService = new AsyncUserService(networkPlayer);
            gameService = new AsyncGameService(networkPlayer);
            imageService = new AsyncImageService(networkPlayer);

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
                    NetworkMessage message = messagesToPrint.Dequeue();
                    Debug.Log(message.Data);
                    CheckMessageHandlers(message);
                }
            }

            if (networkPlayer.SpecificResponses.Count > 0)
            {
                foreach (var item in networkPlayer.SpecificResponses)
                {
                    CheckMessageHandlers(item);
                }
            }
        }

        private void CheckMessageHandlers(NetworkMessage item)
        {
            if (responseMapper.ContainsKey(item.ResponseTo))
            {
                responseMapper[item.ResponseTo].Invoke(item.Data);
                responseMapper.Remove(item.ResponseTo);
            }
            if (messageConsumers.ContainsKey(item.MessageType))
            {
                messageConsumers[item.MessageType].Invoke(item.Data);
            }
        }

        public NetworkMessage TryGetResponse(NetworkId messageId)
        {
            lock (lockObject)
            {
                return networkPlayer.SpecificResponses.FirstOrDefault(message => message.ResponseTo.Equals(messageId));
            }
        }

        private void OnDestroy()
        {
            Me.Disconnect(true);
        }

        public static NetworkManager Instance { get; private set; }
        public NetworkingCore.NetworkPlayer Me { get; internal set; }
    }
}