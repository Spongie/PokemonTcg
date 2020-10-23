using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class ApplyDoublePoisonEffect : Attack
    {
        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            opponent.ActivePokemonCard.IsPoisoned = true;
            opponent.ActivePokemonCard.DoublePoison = true;
        }
    }
}
