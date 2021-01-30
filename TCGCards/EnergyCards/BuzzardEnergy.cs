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
            IgnoreInBuilder = true;
            Amount = 2;
            AttachedIconOverridden = true;
            EnergyOverrideType = EnergyOverriders.Buzzap;
        }

        public BuzzardEnergy(PokemonCard pokemonOwner, EnergyTypes energyType) :this()
        {
            createdBy = pokemonOwner;
            EnergyType = energyType;
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
