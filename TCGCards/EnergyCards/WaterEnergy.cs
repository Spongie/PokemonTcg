namespace Assets.Scripts.Cards.EnergyCards
{
    public class WaterEnergy : IEnergyCard
    {
        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Water, 1);
        }

        public override string GetName()
        {
            return "Water Energy";
        }
    }
}
