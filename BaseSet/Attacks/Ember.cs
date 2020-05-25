using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Ember : Attack
    {
        public Ember()
        {
            Name = "Ember";
            Description = "Discard 1 Energy card attached to Charmander in order to use this attack.";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override void PayExtraCosts(GameField game, Player owner, Player opponent)
        {
            AttackUtils.DiscardAttachedEnergy(owner.ActivePokemonCard, 1);
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
    }
}