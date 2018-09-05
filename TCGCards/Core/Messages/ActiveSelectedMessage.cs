using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class ActiveSelectedMessage : AbstractNetworkMessage
    {
        public ActiveSelectedMessage(IPokemonCard active, Player owner)
        {
            SelectedPokemon = active;
            Owner = owner;
            messageType = MessageTypes.SelectedActive;
        }

        public IPokemonCard SelectedPokemon { get; set; }
        public Player Owner { get; set; }
    }
}
