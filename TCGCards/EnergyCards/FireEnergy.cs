namespace TCGCards.EnergyCards
{
    public class FireEnergy : IEnergyCard
    {
        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Fire, 1);
        }

        public override string GetName()
        {
            return "Fire Energy";
        }
    }
}
