using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class RegisterMessage : AbstractNetworkMessage
    {
        public RegisterMessage(string name, Deck deck)
        {
            Name = name;
            Deck = deck;
            MessageType = MessageTypes.RegisterForGame;
        }

        public string Name { get; set; }
        public Deck Deck { get; set; }
    }
}
