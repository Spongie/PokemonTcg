using TCGCards;
using TCGCards.Core;

namespace TeamRocket.TrainerCards
{
    public class Digger : TrainerCard
    {
        public override string GetName()
        {
            return "Flip a coin. If tails, do 10 damage to your active pokemon. " +
                "If heads your opponent flips a coin. If tails, your opponent does 10 damage to his or her active pokemon. " +
                "If heads, you flip a coin. Keep doing this until a player gets tails.";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            bool flip;
            Player target = caster;

            do
            {
                flip = CoinFlipper.FlipCoin();

                if (flip == CoinFlipper.TAILS)
                    target.ActivePokemonCard.DealDamage(new Damage(0, 10));
                else
                    target = target == caster ? opponent : caster;

            } while (flip == CoinFlipper.HEADS);
        }
    }
}
