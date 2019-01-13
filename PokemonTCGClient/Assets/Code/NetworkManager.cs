using NetworkingCore;
using System;
using System.Collections.Generic;
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

                    if (responseMapper.ContainsKey(message.ResponseTo))
                    {
                        responseMapper[message.ResponseTo].Invoke(message.Data);
                        responseMapper.Remove(message.ResponseTo);
                    }
                    if (messageConsumers.ContainsKey(message.MessageType))
                    {
                        messageConsumers[message.MessageType].Invoke(message.Data);
                    }
                }
            }
            
            if (networkPlayer.SpecificResponses.Count > 0)
            {
                var handledMessages = new List<NetworkId>();

                foreach (var item in networkPlayer.SpecificResponses.Values)
                {
                    if (responseMapper.ContainsKey(item.ResponseTo))
                    {
                        responseMapper[item.ResponseTo].Invoke(item.Data);
                        responseMapper.Remove(item.ResponseTo);
                        handledMessages.Add(item.ResponseTo);
                    }
                    if (messageConsumers.ContainsKey(item.MessageType))
                    {
                        messageConsumers[item.MessageType].Invoke(item.Data);
                        handledMessages.Add(item.ResponseTo);
                    }
                }

                foreach (var id in handledMessages)
                {
                    networkPlayer.SpecificResponses.TryRemove(id, out NetworkMessage _);
                }
            }
        }

        public NetworkMessage TryGetResponse(NetworkId messageId)
        {
            lock (lockObject)
            {
                if (networkPlayer.SpecificResponses.TryGetValue(messageId, out NetworkMessage response))
                {
                    return response;
                }

                return null;
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