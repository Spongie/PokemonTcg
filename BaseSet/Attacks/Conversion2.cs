using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.Attacks
{
    internal class Conversion2 : Attack
    {
        public Conversion2()
        {
            Name = "Conversion 2";
            Description = "Change Porygon's Resistance to a type of your choice other than Colorless.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var message = new SelectColorMessage("Change Porygon's Resistance to a type of your choice other than Colorless.").ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<SelectColorMessage>(message);

            owner.ActivePokemonCard.Resistance = response.Color;
        }
    }
}
