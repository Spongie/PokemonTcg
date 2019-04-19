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

        public NetworkMessage(MessageTypes type, object data, NetworkId senderId, NetworkId messageId) :this(type, data, senderId, messageId, NetworkId.Empty)
        {

        }

        public NetworkMessage(MessageTypes type, object data, NetworkId senderId, NetworkId messageId, NetworkId responseTo)
        {
            MessageType = type;
            Data = data;
            SenderId = senderId;
            MessageId = messageId;
            ResponseTo = responseTo;
        }

        public MessageTypes MessageType { get; set; }
        public object Data { get; set; }
        public NetworkId SenderId { get; set; }
        public NetworkId MessageId { get; set; }
        public NetworkId ResponseTo { get; set; }
        public DateTime Received { get; set; }
        public bool RequiresResponse { get; set; } = true;

        public void Send(NetworkStream stream)
        {
            string message = Serializer.Serialize(this);

            var jsonBytes = Encoding.UTF8.GetBytes(message);

            var prefixBytes = BitConverter.GetBytes(jsonBytes.Length);
            stream.Write(prefixBytes, 0, prefixBytes.Length);
            
            stream.Write(jsonBytes, 0, jsonBytes.Length);
        }
    }
}
