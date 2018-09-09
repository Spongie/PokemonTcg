namespace TCGCards.EnergyCards
{
    public class ElectricEnergy : EnergyCard
    {
        public ElectricEnergy()
        {
            EnergyType = EnergyTypes.Electric;
        }

        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Electric, 1);
        }

        public override string GetName()
        {
            return "Electric Energy";
        }

        public override void OnAttached(PokemonCard attachedTo, bool fromHand) { }
    }
}
