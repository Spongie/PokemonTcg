using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Scrunch : Attack
    {
        public Scrunch()
        {
            Name = "Scrunch";
            Description = "Flip a coin. If heads, prevent all damage done to Chansey during your opponent's next turn. (Any other effects of attacks still happen.)";
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
		//TODO: Special effects
    }
}
