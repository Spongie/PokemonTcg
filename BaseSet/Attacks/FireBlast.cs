using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class FireBlast : Attack
    {
        public FireBlast()
        {
            Name = "Fire Blast";
            Description = "Discard 1 Energy card attached to Ninetales in order to use this attack.";
			DamageText = "80";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 4)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            AttackUtils.DiscardAttachedEnergy(owner.ActivePokemonCard, 1);
            return 80;
        }
    }
}
