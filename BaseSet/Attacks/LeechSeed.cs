using NetworkingCore.Messages;
using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class LeechSeed : Attack
    {
        public LeechSeed()
        {
            Name = "Leech Seed";
            Description = "Unless all damage from this attack is prevented, you may remove 1 damage counter from Bulbasaur.";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }

        public override void OnDamageDealt(int amount, Player owner)
        {
            if (amount <= 0)
            {
                return;
            }

            var activateMessage = new YesNoMessage { Message = Description }.ToNetworkMessage(owner.Id);
            var activateResponse = owner.NetworkPlayer.SendAndWaitForResponse<YesNoMessage>(activateMessage);

            if (activateResponse.AnsweredYes)
            {
                owner.ActivePokemonCard.DamageCounters -= 10;
            }
        }
    }
}
