using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    public class ContinuousFireball : Attack
    {
        public ContinuousFireball()
        {
            Name = "Continuous Fireball";
            Description = "50× damage. Flip a number of coins equal to the number for [R] Energy cards attached to Dark Charizard. This attack does 50 damage times the number of heads. Discard a number of [R] Energy cards attached to Dark Charizard equal to the number of heads.";
            DamageText = "50x";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            PokemonCard caster = owner.ActivePokemonCard;
            int headsCount = CoinFlipper.FlipCoins(caster.AttachedEnergy.Count(energy => energy.EnergyType == EnergyTypes.Fire));

            for (int i = 0; i < headsCount; i++)
            {
                var energyCard = caster.AttachedEnergy.FirstOrDefault(energy => energy.EnergyType == EnergyTypes.Fire);
                caster.AttachedEnergy.Remove(energyCard);
                owner.DiscardPile.Add(energyCard);
            }

            return headsCount * 50;
        }
    }
}
