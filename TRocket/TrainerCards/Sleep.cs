using TCGCards;
using TCGCards.Core;

namespace TeamRocket.TrainerCards
{
    public class Sleep : TrainerCard
    {
        public override string GetName()
        {
            return "Sleep! (Rockets Secret Machine)";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            if (CoinFlipper.FlipCoin())
            {
                opponent.ActivePokemonCard.IsAsleep = true;
            }
        }
    }
}
