using System;

namespace NetworkingCore
{
    public abstract class AbstractNetworkMessage
    {
        protected MessageTypes messageType;

        public NetworkMessage ToNetworkMessage(NetworkId senderId)
        {
            return new NetworkMessage(messageType, this, senderId, NetworkId.Generate());
        }
    }
}
