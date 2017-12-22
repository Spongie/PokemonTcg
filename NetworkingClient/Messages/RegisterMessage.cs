using TCGCards.Core;

namespace NetworkingClient
{
    public struct RegisterMessage
    {
        public RegisterMessage(string name, Deck deck)
        {
            Name = name;
            Deck = deck;
        }

        public string Name { get; set; }
        public Deck Deck { get; set; }
    }
}
