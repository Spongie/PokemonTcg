using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Barrier : Attack
    {
        public Barrier()
        {
            Name = "Barrier";
            Description = "Discard 1 Energy card attached to Mewtwo in order to use this attack. During your opponent's next turn, prevent all effects of attacks, including damage, done to Mewtwo.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO: Special effects
    }
}
