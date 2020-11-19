using Entities;
using TCGCards.Core;

namespace TCGCards.EnergyCards
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
        }

        public override Energy GetEnergry() => new Energy(energyType, 2);

        public override string GetName() => PokemonNames.Electrode;

        public override void OnAttached(PokemonCard attachedTo, bool fromHand, GameField game)
        {

        }

        public override void OnPutInDiscard(Player owner)
        {
            owner.DiscardPile.Add(createdBy);
            owner.DiscardPile.Remove(this);
        }
    }
}
