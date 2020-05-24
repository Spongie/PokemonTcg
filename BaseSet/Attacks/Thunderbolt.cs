using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Thunderbolt : Attack
    {
        public Thunderbolt()
        {
            Name = "Thunderbolt";
            Description = "Discard all Energy cards attached to Zapdos in order to use this attack.";
			DamageText = "100";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 4)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 100;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var energyCards = new List<EnergyCard>(owner.ActivePokemonCard.AttachedEnergy);

            foreach (var energyCard in energyCards)
            {
                owner.ActivePokemonCard.DiscardEnergyCard(energyCard);
            }
        }
    }
}
