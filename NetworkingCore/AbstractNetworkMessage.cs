namespace NetworkingCore
{
    public abstract class AbstractNetworkMessage
    {
        public virtual NetworkMessage ToNetworkMessage(NetworkId senderId)
        {
            return new NetworkMessage(MessageType, this, senderId, NetworkId.Generate());
        }

        public MessageTypes MessageType { get; set; }
    }
}
