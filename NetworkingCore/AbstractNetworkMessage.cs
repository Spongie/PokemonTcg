using System;

namespace NetworkingCore
{
    public abstract class AbstractNetworkMessage
    {
        protected MessageTypes messageType;

        public NetworkMessage ToNetworkMessage(Guid senderId)
        {
            return new NetworkMessage(messageType, Serializer.Serialize(this), senderId, Guid.NewGuid());
        }
    }
}
