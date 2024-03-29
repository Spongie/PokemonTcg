﻿using Assets.Code.UI.Events;
using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using TCGCards.Core;
using UnityEngine;

namespace Assets.Code
{
    public class NetworkManager : MonoBehaviour
    {
        private NetworkingCore.NetworkPlayer networkPlayer;
        private Queue<NetworkMessage> messagesToPrint;
        private object lockObject = new object();
        public AsyncGameService gameService;
        public AsyncImageService imageService;
        public AsyncPlayerService playerService;
        public Dictionary<MessageTypes, Action<object, NetworkId>> messageConsumers;
        private Dictionary<NetworkId, Action<object>> responseMapper;

        internal void RegisterCallbackById(NetworkId responseId, Action<object> callback)
        {
            responseMapper.Add(responseId, callback);
        }

        internal void RegisterCallback(MessageTypes messageTypes, Action<object, NetworkId> callback)
        {
            messageConsumers.Add(messageTypes, callback);
        }

        internal void DeRegisterCallback(MessageTypes messageType)
        {
            messageConsumers.Remove(messageType);
        }

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            responseMapper = new Dictionary<NetworkId, Action<object>>();
            messageConsumers = new Dictionary<MessageTypes, Action<object, NetworkId>>();
            messagesToPrint = new Queue<NetworkMessage>();

            var tcp = new TcpClient();
            //tcp.Connect("85.90.244.171", 8080);
            tcp.Connect("localhost", 8080);
            networkPlayer = new NetworkingCore.NetworkPlayer(tcp);

            Me = networkPlayer;
            gameService = new AsyncGameService(networkPlayer);
            imageService = new AsyncImageService(networkPlayer);
            playerService = new AsyncPlayerService(networkPlayer);

            networkPlayer.DataReceived += NetworkPlayer_DataReceived;
            networkPlayer.OnDisconnected += NetworkPlayer_OnDisconnected;
        }

        private void NetworkPlayer_OnDisconnected(object sender, NetworkId e)
        {
            Debug.LogError("Disconnected");
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
                    if (RespondingTo != null || (GameEventHandler.Instance != null && GameEventHandler.Instance.AnyEventPending()))
                    {
                        break;
                    }

                    NetworkMessage message = messagesToPrint.Dequeue();
                    Debug.Log(message.Data);

                    if (responseMapper.ContainsKey(message.ResponseTo))
                    {
                        RespondingTo = message.RequiresResponse ? message.MessageId : null;
                        responseMapper[message.ResponseTo].Invoke(message.Data);
                        responseMapper.Remove(message.ResponseTo);
                    }
                    if (messageConsumers.ContainsKey(message.MessageType))
                    {
                        RespondingTo = message.RequiresResponse ? message.MessageId : null;
                        messageConsumers[message.MessageType].Invoke(message.Data, message.MessageId);
                    }
                }
            }
            
            if (networkPlayer.SpecificResponses.Count > 0)
            {
                var handledMessages = new List<NetworkId>();

                foreach (var item in networkPlayer.SpecificResponses.Values)
                {
                    if (RespondingTo != null)
                    {
                        break;
                    }

                    if (responseMapper.ContainsKey(item.ResponseTo))
                    {
                        handledMessages.Add(item.ResponseTo);
                        RespondingTo = item.RequiresResponse ? item.MessageId : null;
                        responseMapper[item.ResponseTo].Invoke(item.Data);
                        responseMapper.Remove(item.ResponseTo);
                    }
                    if (messageConsumers.ContainsKey(item.MessageType))
                    {
                        handledMessages.Add(item.ResponseTo);
                        RespondingTo = item.RequiresResponse ? item.MessageId : null;
                        messageConsumers[item.MessageType].Invoke(item.Data, item.MessageId);
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

        public void SendToServer(AbstractNetworkMessage networkMessage, bool isResponse)
        {
            var message = networkMessage.ToNetworkMessage(Me.Id);

            if (isResponse)
            {
                message.ResponseTo = RespondingTo;
            }

            Me.Send(message);

            if (isResponse)
            {
                RespondingTo = null;
            }
        }

        public NetworkId RespondingTo { get; set; }
        public static NetworkManager Instance { get; private set; }
        public NetworkingCore.NetworkPlayer Me { get; internal set; }
        public GameField CurrentGame { get; internal set; }
    }
}