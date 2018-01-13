using TCGCards.Core;

namespace TCGCards.PokemonCards.TeamRocket.Attacks
{
    public class Psyshock : Attack
    {
        public override int GetDamage(Player owner, Player opponent)
        {
            return 10;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (CoinFlipper.FlipCoin() == CoinFlipper.HEADS)
            {
                opponent.ActivePokemonCard.IsParalyzed = true;
            }
        }
    }
}
