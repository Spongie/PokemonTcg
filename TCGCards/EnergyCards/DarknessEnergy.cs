using Entities;

namespace TCGCards.EnergyCards
{
    public class DarknessEnergy : EnergyCard
    {
        public DarknessEnergy()
        {
            EnergyType = EnergyTypes.Darkness;
        }

        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Darkness, 1);
        }

        public override string GetName()
        {
            return "Darkness Energy";
        }

        public override void OnAttached(PokemonCard attachedTo, bool fromHand) { }
    }
}
