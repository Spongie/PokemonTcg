namespace Assets.Scripts.Cards
{
    public struct Energy
    {
        public Energy(EnergyTypes energyType, int amount)
        {
            Amount = amount;
            EnergyType = energyType;
        }

        public EnergyTypes EnergyType { get; set; }
        public int Amount { get; set; }
    }
}
