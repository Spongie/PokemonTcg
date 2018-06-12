using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Ember : Attack
    {
        public override int GetDamage(Player owner, Player opponent)
        {
            return 30;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var fireEnergy = owner.ActivePokemonCard.AttachedEnergy.First(energy => energy.EnergyType == EnergyTypes.Fire);
            owner.ActivePokemonCard.AttachedEnergy.Remove(fireEnergy);
            owner.DiscardPile.Add(fireEnergy);
        }
    }
}
