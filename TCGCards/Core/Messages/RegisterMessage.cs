using NetworkingCore;
using TCGCards.Core;

namespace TCGCards.Core.Messages
{
    public class RegisterMessage : AbstractNetworkMessage
    {
        public RegisterMessage(string name, Deck deck)
        {
            Name = name;
            Deck = deck;
            messageType = MessageTypes.Register;
        }

        public string Name { get; set; }
        public Deck Deck { get; set; }
    }
}
