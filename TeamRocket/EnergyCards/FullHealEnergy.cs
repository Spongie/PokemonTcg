using TCGCards;

namespace TeamRocket.EnergyCards
{
    public class FullHealEnergy : EnergyCard
    {
        public FullHealEnergy()
        {
            IsBasic = false;
            Set = Singleton.Get<Set>();
        }

        public override Energy GetEnergry() => new Energy(EnergyTypes.Colorless, 1);

        public override string GetName() => "Full Heal Energy";

        public override void OnAttached(PokemonCard attachedTo, bool fromHand)
        {
            if (!fromHand)
                return;

            attachedTo.IsAsleep = false;
            attachedTo.IsConfused = false;
            attachedTo.IsPoisoned = false;
            attachedTo.IsParalyzed = false;
        }
    }
}
