using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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

        public void Send(NetworkStream stream)
        {
            string message = Serializer.Serialize(this);

            if (NetworkPlayer.logRequests)
            {
                File.WriteAllText("D:\\Dump\\request.json", message);
            }

            var jsonBytes = Encoding.UTF8.GetBytes(message);

            var prefixBytes = BitConverter.GetBytes(jsonBytes.Length);
            stream.Write(prefixBytes, 0, prefixBytes.Length);
            
            stream.Write(jsonBytes, 0, jsonBytes.Length);
        }
    }
}
