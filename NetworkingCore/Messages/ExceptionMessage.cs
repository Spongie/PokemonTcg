using System;

namespace NetworkingCore.Messages
{
    public class ExceptionMessage : AbstractNetworkMessage
    {
        public ExceptionMessage() :this(null)
        {

        }

        public ExceptionMessage(Exception exception)
        {
            Exception = exception;
            MessageType = MessageTypes.Error;
        }

        public override NetworkMessage ToNetworkMessage(NetworkId senderId)
        {
            var message = base.ToNetworkMessage(senderId);
            message.RequiresResponse = false;

            return message;
        }

        public Exception Exception { get; }
    }
}
