namespace Assets.Scripts.Cards.EnergyCards
{
    public class PsychicEnergy : IEnergyCard
    {
        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Psychic, 1);
        }

        public override string GetName()
        {
            return "Psychic Energy";
        }
    }
}
