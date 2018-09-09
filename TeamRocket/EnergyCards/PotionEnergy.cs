using TCGCards;

namespace TeamRocket.EnergyCards
{
    public class PotionEnergy : EnergyCard
    {
        public override Energy GetEnergry() => new Energy(EnergyTypes.Colorless, 1);

        public override string GetName() => "Potion Energy";

        public override void OnAttached(PokemonCard attachedTo, bool fromHand)
        {
            if (!fromHand)
                return;

            if (attachedTo.DamageCounters > 10)
                attachedTo.DamageCounters -= 10;
        }
    }
}
