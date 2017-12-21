using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;

namespace NetworkingClient.Common
{
    public struct NetworkMessage
    {
        public NetworkMessage(MessageTypes type, string data)
        {
            MessageType = type;
            Data = data;
        }

        public MessageTypes MessageType { get; set; }
        public string Data { get; set; }

        public void Send(NetworkStream stream)
        {
            var jsonBytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(this));

            var prefixBytes = BitConverter.GetBytes(jsonBytes.Length);
            stream.Write(prefixBytes, 0, prefixBytes.Length);

            stream.Write(jsonBytes, 0, jsonBytes.Length);
        }
    }
}
