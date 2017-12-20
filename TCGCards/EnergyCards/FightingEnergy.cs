namespace Assets.Scripts.Cards.EnergyCards
{
    public class FightingEnergy : IEnergyCard
    {
        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Fighting, 1);
        }

        public override string GetName()
        {
            return "Fightning Energy";
        }
    }
}
