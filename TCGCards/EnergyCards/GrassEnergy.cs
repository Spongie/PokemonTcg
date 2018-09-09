namespace TCGCards.EnergyCards
{
    public class GrassEnergy : EnergyCard
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

        public override void OnAttached(PokemonCard attachedTo, bool fromHand) { }
    }
}
