using TCGCards;
using TCGCards.Core;

namespace NetworkingClient.Messages
{
    public struct ActiveSelectedMessage
    {
        public ActiveSelectedMessage(IPokemonCard active, Player owner)
        {
            ActivePokemon = active;
            Owner = owner;
        }

        public IPokemonCard ActivePokemon { get; set; }
        public Player Owner { get; set; }
    }
}
