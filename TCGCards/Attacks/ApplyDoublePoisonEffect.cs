using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class ApplyDoublePoisonEffect : Attack
    {
        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            opponent.ActivePokemonCard.ApplyStatusEffect(StatusEffect.Poison, game);
            opponent.ActivePokemonCard.DoublePoison = true;

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
