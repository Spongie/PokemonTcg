namespace TCGCards.EnergyCards
{
    public class GrassEnergy : IEnergyCard
    {
        public GrassEnergy()
        {
            EnergyType = EnergyTypes.Grass;
        }

        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Grass, 1);
        }

        public override string GetName()
        {
            return "Grass Energy";
        }
    }
}
