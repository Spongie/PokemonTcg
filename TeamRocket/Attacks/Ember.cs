using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Ember : Attack
    {
        public Ember()
        {
            Name = "Ember";
            Description = "Discard 1 Fire Energy card attached to Ponyta in order to use this attack";
            DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 1),
                new Energy(EnergyTypes.Colorless, 1),
            };
        }

        public override void PayExtraCosts(GameField game, Player owner, Player opponent)
        {
            var fireEnergy = owner.ActivePokemonCard.AttachedEnergy.First(energy => energy.EnergyType == EnergyTypes.Fire);
            owner.ActivePokemonCard.AttachedEnergy.Remove(fireEnergy);
            owner.DiscardPile.Add(fireEnergy);
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 30;
        }
    }
}
