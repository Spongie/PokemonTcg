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
        private Queue<NetworkMessage> messagesToPrint;
        private object lockObject = new object();
        public AsyncUserService userService;
        //public Dictionary<MessageTypes, IMessageConsumer>; remember this

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            messages = new List<NetworkMessage>();
            messagesToPrint = new Queue<NetworkMessage>();

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

            //if (networkPlayer.SpecificResponses.Count > 0)
            //{
            //    foreach (var item in networkPlayer.SpecificResponses)
            //    {
            //        Debug.Log(item.Data);
            //    }
            //}
        }

        public static NetworkManager Instance { get; private set; }
    }
}