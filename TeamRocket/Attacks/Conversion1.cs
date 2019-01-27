using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Attacks
{
    internal class Conversion1 : Attack
    {
        public Conversion1()
        {
            Name = "Conversion 1";
            Description = "If the defending pokemon has a weakness, you may change it to a type of your choice other than colerless";
            DamageText = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var message = new SelectColorMessage("Select a new weakness for your opponents active pokemon").ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<SelectColorMessage>(message);

            opponent.ActivePokemonCard.Weakness = response.Color;
        }
    }
}
