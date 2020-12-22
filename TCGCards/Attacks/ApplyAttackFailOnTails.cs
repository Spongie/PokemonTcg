using TCGCards.Core;
using TCGCards.Core.Abilities;

namespace TCGCards.Attacks
{
    public class ApplyAttackFailOnTails : Attack
    {
        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            opponent.ActivePokemonCard.TemporaryAbilities.Add(new AttackStopperSpecificAbility(owner.ActivePokemonCard)
            {
                CoinFlip = true,
                OnlyCurrentTarget = false,
                StopOnTails = true,
                IsBuff = true
            });

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
