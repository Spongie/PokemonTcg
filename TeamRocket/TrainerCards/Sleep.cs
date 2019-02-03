using TCGCards;
using TCGCards.Core;

namespace TeamRocket.TrainerCards
{
    public class Sleep : TrainerCard
    {
        public Sleep()
        {
            Name = "Sleep! (Rockets Secret Machine)";
            Description = "Flip a coin. If Heads, the Defending Pokémon is now Asleep.";
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
