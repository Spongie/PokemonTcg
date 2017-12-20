namespace Assets.Scripts.Cards.EnergyCards
{
    public class GrassEnergy : IEnergyCard
    {
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
