namespace Assets.Scripts.Cards.EnergyCards
{
    public class DoubleColorlessEnergy : IEnergyCard
    {
        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Colorless, 2);
        }

        public override string GetName()
        {
            return "Double Colorless Energy";
        }
    }
}
