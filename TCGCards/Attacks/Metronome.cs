using Entities;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.Attacks
{
    public class Metronome : Attack
    {
        private Attack chosenAttack;

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var attacks = opponent.ActivePokemonCard.Attacks;

            if (attacks.Count == 1)
            {
                chosenAttack = attacks[0];
            }
            else
            {
                var attackMessage = new SelectAttackMessage(opponent.ActivePokemonCard.Attacks.ToList()).ToNetworkMessage(owner.Id);
                chosenAttack = owner.NetworkPlayer.SendAndWaitForResponse<AttackMessage>(attackMessage).Attack;
            }

            return chosenAttack.GetDamage(owner, opponent, game);
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (chosenAttack == null)
            {
                return;
            }

            chosenAttack.ProcessEffects(game, owner, opponent);
            chosenAttack = null;

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
