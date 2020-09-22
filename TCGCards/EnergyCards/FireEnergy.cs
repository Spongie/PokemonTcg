using Entities;

namespace TCGCards.EnergyCards
{
    public class FireEnergy : EnergyCard
    {
        public FireEnergy()
        {
            EnergyType = EnergyTypes.Fire;
        }

        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Fire, 1);
        }

        public override string GetName()
        {
            return "Fire Energy";
        }

        public override void OnAttached(PokemonCard attachedTo, bool fromHand) { }
    }
}
