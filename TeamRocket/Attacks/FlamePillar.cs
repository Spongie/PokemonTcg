using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class FlamePillar : Attack
    {
        public FlamePillar()
        {
            Name = "Flame Pillar";
            Description = "You may discard 1 [R] Energy card attached to Dark Rapidash when you use this attack. If you do and your opponent has any Benched Pokémon, choose 1 of them and this attack does 10 damage to it. (Don&#8217;t apply Weakness and Resistance for Benched Pokémon.)";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 2)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		//TODO:
    }
}
