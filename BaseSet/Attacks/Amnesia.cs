using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.Attacks
{
    internal class Amnesia : Attack
    {
        public Amnesia()
        {
            Name = "Amnesia";
            Description = "Choose 1 of defenders attacks. Defender cannot use that attack next turn.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (opponent.ActivePokemonCard.Attacks.Count == 1)
            {
                opponent.ActivePokemonCard.Attacks[0].Disabled = true;
                game.GameLog.AddMessage($"{owner.NetworkPlayer?.Name} disables attack {opponent.ActivePokemonCard.Attacks[0].Name}");
                return;
            }

            var attackMessage = new SelectAttackMessage(opponent.ActivePokemonCard.Attacks).ToNetworkMessage(owner.Id);
            var chosenAttack = owner.NetworkPlayer.SendAndWaitForResponse<Attack>(attackMessage);

            opponent.ActivePokemonCard.Attacks.First(x => x.Id.Equals(chosenAttack.Id)).Disabled = true;
        }
    }
}
