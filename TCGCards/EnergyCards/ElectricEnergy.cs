namespace TCGCards.EnergyCards
{
    public class ElectricEnergy : IEnergyCard
    {
        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Electric, 1);
        }

        public override string GetName()
        {
            return "Electric Energy";
        }
    }
}
