using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class BenchManipulation : Attack
    {
        public BenchManipulation()
        {
            Name = "Bench Manipulation";
            Description = "20× damage. You opponent flips a number of coins equal to the number of Pokémon on his or her Bench. This attack does 20 damage times the number of tails. Don&#8217;t apply Weakness and Resistance for this attack. (Any other effects that would happen after applying Weakness and Resistance still happen.)";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO:
    }
}
