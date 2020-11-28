using Entities;
using TCGCards.Core;
using TCGCards.Core.Abilities;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards.Attacks
{
    public class ApplyAttackFailOnTails : Attack
    {
        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            opponent.ActivePokemonCard.TemporaryAbilities.Add(new AttackStopperSpecificAbility(owner.ActivePokemonCard)
            {
                CoinFlip = true,
                OnlyCurrentTarget = true,
                CurrentTarget = opponent.ActivePokemonCard
            });

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
