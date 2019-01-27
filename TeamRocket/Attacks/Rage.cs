using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Rage : Attack
    {
        public Rage()
        {
            Name = "Rage";
            Description = "Does 10 damage plus 10 more damage for each damage counter on Dark Flareon.";
            DamageText = "10+";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10 + (10 * owner.ActivePokemonCard.DamageCounters / 10);
        }
    }
}
