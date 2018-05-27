using NetworkingCore;
using TCGCards;
using TCGCards.Core;

namespace TCGCards.Core.Messages
{ 
    public class ActiveSelectedMessage : AbstractNetworkMessage
    {
        public ActiveSelectedMessage(IPokemonCard active, Player owner)
        {
            ActivePokemon = active;
            Owner = owner;
            messageType = MessageTypes.SelectedActive;
        }

        public IPokemonCard ActivePokemon { get; set; }
        public Player Owner { get; set; }
    }
}
