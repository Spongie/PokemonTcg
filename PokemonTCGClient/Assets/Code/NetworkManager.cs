using NetworkingCore;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Assets.Code
{
    public class NetworkManager : MonoBehaviour
    {
        private NetworkingCore.NetworkPlayer networkPlayer;
        private List<NetworkMessage> messages;
        private object lockObject = new object();
        public AsyncUserService userService;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            messages = new List<NetworkMessage>();
            var tcp = new TcpClient();
            tcp.Connect("127.0.0.1", 1565);
            networkPlayer = new NetworkingCore.NetworkPlayer(tcp);

            userService = new AsyncUserService(networkPlayer);

            networkPlayer.DataReceived += NetworkPlayer_DataReceived;
        }

        private void NetworkPlayer_DataReceived(object sender, NetworkDataRecievedEventArgs e)
        {
            lock (lockObject)
            {
                messages.Add(e.Message);
            }
        }

        void Update()
        {
            lock (lockObject)
            {
                foreach (var message in messages)
                {
                    Debug.Log(message);
                }
            }
        }

        public static NetworkManager Instance { get; private set; }
    }
}