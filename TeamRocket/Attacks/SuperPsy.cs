using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class SuperPsy : Attack
    {
        public SuperPsy()
        {
            Name = "Super Psy";
            Description = "";
            DamageText = "50";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 50;
        }
		
    }
}
