using Entities;

namespace TCGCards.EnergyCards
{
    public class PsychicEnergy : EnergyCard
    {
        public PsychicEnergy()
        {
            EnergyType = EnergyTypes.Psychic;
        }

        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Psychic, 1);
        }

        public override string GetName()
        {
            return "Psychic Energy";
        }

        public override void OnAttached(PokemonCard attachedTo, bool fromHand) { }
    }
}
