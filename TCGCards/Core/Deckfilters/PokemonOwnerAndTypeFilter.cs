using Entities;
using NetworkingCore;

namespace TCGCards.Core.Deckfilters
{
    public class PokemonOwnerAndTypeFilter : IDeckFilter
    {
        public PokemonOwnerAndTypeFilter()
        {

        }

        public PokemonOwnerAndTypeFilter(NetworkId playerId) :this(playerId, EnergyTypes.All)
        {

        }

        public PokemonOwnerAndTypeFilter(NetworkId playerId, EnergyTypes type)
        {
            OwnerId = playerId;
            EnergyType = type;
        }

        public NetworkId OwnerId { get; set; }
        public EnergyTypes EnergyType { get; set; }

        public bool IsCardValid(Card card)
        {
            var pokemon = card as PokemonCard;

            if (pokemon == null)
            {
                return false;
            }

            bool validType = true;

            if (EnergyType != EnergyTypes.All && EnergyType != EnergyTypes.None)
            {
                validType = pokemon.Type == EnergyType;
            }

            return validType && card.Owner.Id.Equals(OwnerId);
        }
    }
}
