using Entities;

namespace TCGCards.EnergyCards
{
    public class LightningEnergy : EnergyCard
    {
        public LightningEnergy()
        {
            EnergyType = EnergyTypes.Electric;
        }

        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Electric, 1);
        }

        public override string GetName()
        {
            return "Lightning Energy";
        }

        public override void OnAttached(PokemonCard attachedTo, bool fromHand) { }
    }
}
