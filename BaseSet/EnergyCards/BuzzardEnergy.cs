using TCGCards;
using TCGCards.Core;

namespace BaseSet.EnergyCards
{
    public class BuzzardEnergy : EnergyCard
    {
        private readonly PokemonCard createdBy;
        private readonly EnergyTypes energyType;

        public BuzzardEnergy(PokemonCard pokemonOwner, EnergyTypes energyType)
        {
            createdBy = pokemonOwner;
            this.energyType = energyType;
            IgnoreInBuilder = true;
            Set = Singleton.Get<Set>();
        }

        public override Energy GetEnergry() => new Energy(energyType, 2);

        public override string GetName() => PokemonNames.Electrode;

        public override void OnAttached(PokemonCard attachedTo, bool fromHand)
        {
            
        }

        public override void OnPutInDiscard(Player owner)
        {
            owner.DiscardPile.Add(createdBy);
            owner.DiscardPile.Remove(this);
        }
    }
}
