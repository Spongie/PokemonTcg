using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;

namespace NetworkingClientCore
{
    public struct NetworkMessage
    {
        public NetworkMessage(MessageTypes type, string data, Guid senderId)
        {
            MessageType = type;
            Data = data;
            SenderId = senderId;
        }

        public MessageTypes MessageType { get; set; }
        public string Data { get; set; }
        public Guid SenderId { get; set; }

        public void Send(NetworkStream stream)
        {
            var jsonBytes = Encoding.UTF8.GetBytes(Serializer.Serialize(this));

            var prefixBytes = BitConverter.GetBytes(jsonBytes.Length);
            stream.Write(prefixBytes, 0, prefixBytes.Length);

            stream.Write(jsonBytes, 0, jsonBytes.Length);
        }
    }
}
