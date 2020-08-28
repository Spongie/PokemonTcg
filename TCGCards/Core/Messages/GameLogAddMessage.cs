using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class GameLogAddMessage : AbstractNetworkMessage
    {
        public GameLogAddMessage()
        {

        }

        public GameLogAddMessage(List<string> messages)
        {
            MessageType = MessageTypes.GameLogNewMessages;
            NewMessages = new List<string>(messages ?? new List<string>());

        }

        public override NetworkMessage ToNetworkMessage(NetworkId senderId)
        {
            var message = base.ToNetworkMessage(senderId);
            message.RequiresResponse = false;

            return message;
        }

        public List<string> NewMessages { get; set; }
    }
}
