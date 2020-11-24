using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.Attacks
{
    public class DisableAttackAttack : Attack
    {
        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (opponent.ActivePokemonCard.Attacks.Count == 1)
            {
                opponent.ActivePokemonCard.Attacks[0].Disabled = true;
                game.GameLog.AddMessage($"{owner.NetworkPlayer?.Name} disables attack {opponent.ActivePokemonCard.Attacks[0].Name}");
                return;
            }

            var attackMessage = new SelectAttackMessage(opponent.ActivePokemonCard.Attacks).ToNetworkMessage(owner.Id);
            var chosenAttack = owner.NetworkPlayer.SendAndWaitForResponse<AttackMessage>(attackMessage).Attack;

            game.GameLog.AddMessage($"{owner.NetworkPlayer?.Name} disables attack {chosenAttack.Name}");
            opponent.ActivePokemonCard.Attacks.First(x => x.Id.Equals(chosenAttack.Id)).Disabled = true;

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
