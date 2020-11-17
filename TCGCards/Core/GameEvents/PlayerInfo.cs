using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.GameEvents
{
    public class PlayerInfo
    {
        public NetworkId Id { get; set; }
        public int CardsInHand { get; set; }
        public int CardsInDeck { get; set; }
        public List<Card> PrizeCards { get; set; }
        public List<Card> CardsInDiscard { get; set; }
        public List<Card> BenchedPokemon { get; set; }
        public Card ActivePokemon { get; set; }
    }
}
