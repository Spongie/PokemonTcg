using Entities;
using NetworkingCore;
using TCGCards.Core;

namespace TCGCards.EnergyCards
{
    public class BuzzardEnergy : EnergyCard
    {
        private readonly PokemonCard createdBy;

        public BuzzardEnergy() :base()
        {

        }

        public BuzzardEnergy(PokemonCard pokemonOwner, EnergyTypes energyType) :base()
        {
            createdBy = pokemonOwner;
            EnergyType = energyType;
            IgnoreInBuilder = true;
            Amount = 2;
        }

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
