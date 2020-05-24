using System;
using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class KarateChop : Attack
    {
        public KarateChop()
        {
            Name = "Karate Chop";
            Description = "Does 50 damage minus 10 for each damage counter on Machoke.";
			DamageText = "50";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            var damage = 50 - owner.ActivePokemonCard.DamageCounters;
            return Math.Max(damage, 0);
        }
    }
}
