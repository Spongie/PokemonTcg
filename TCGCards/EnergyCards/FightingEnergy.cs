using Entities;

namespace TCGCards.EnergyCards
{
    public class FightingEnergy : EnergyCard
    {
        public FightingEnergy()
        {
            EnergyType = EnergyTypes.Fighting;
        }

        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Fighting, 1);
        }

        public override string GetName()
        {
            return "Fightning Energy";
        }

        public override void OnAttached(PokemonCard attachedTo, bool fromHand) { }
    }
}
