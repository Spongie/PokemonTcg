using System;
using System.Net.Sockets;
using System.Text;

namespace NetworkingCore
{
    public class NetworkMessage
    {
        public NetworkMessage()
        {

        }

        public NetworkMessage(MessageTypes type, string data, Guid senderId, Guid messageId) :this(type, data, senderId, messageId, Guid.Empty)
        {

        }

        public NetworkMessage(MessageTypes type, string data, Guid senderId, Guid messageId, Guid responseTo)
        {
            MessageType = type;
            Data = data;
            SenderId = senderId;
            MessageId = messageId;
            ResponseTo = responseTo;
        }

        public MessageTypes MessageType { get; set; }
        public string Data { get; set; }
        public Guid SenderId { get; set; }
        public Guid MessageId { get; set; }
        public Guid ResponseTo { get; set; }

        public void Send(NetworkStream stream)
        {
            var jsonBytes = Encoding.UTF8.GetBytes(Serializer.Serialize(this));

            var prefixBytes = BitConverter.GetBytes(jsonBytes.Length);
            stream.Write(prefixBytes, 0, prefixBytes.Length);

            stream.Write(jsonBytes, 0, jsonBytes.Length);
        }
    }
}
