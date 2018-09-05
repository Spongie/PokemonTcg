using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class ActiveSelectedMessage : AbstractNetworkMessage
    {
        public ActiveSelectedMessage(PokemonCard active, Player owner)
        {
            SelectedPokemon = active;
            Owner = owner;
            messageType = MessageTypes.SelectedActive;
        }

        public PokemonCard SelectedPokemon { get; set; }
        public Player Owner { get; set; }
    }
}
