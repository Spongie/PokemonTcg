using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Agility : Attack
    {
        public Agility()
        {
            Name = "Agility";
            Description = "Flip a coin. If heads, during your opponent's next turn, prevent all effects of attacks, including damage, done to Raichu.";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 1),
				new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		//TODO: Special effects
    }
}
