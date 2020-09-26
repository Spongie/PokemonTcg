using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TCGCards.Core.GameEvents
{
    public class GameFieldInfo
    {
        public PlayerInfo Opponent { get; set; }
        public PlayerInfo Me { get; set; }
        public List<Card> CardsInMyHand { get; set; }
        public NetworkId ActivePlayer { get; set; }
        public GameFieldState CurrentState { get; set; }
    }

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
