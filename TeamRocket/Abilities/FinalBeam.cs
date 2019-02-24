using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class FinalBeam : Ability
    {
        public FinalBeam(PokemonCard owner) : base(owner)
        {
            TriggerType = TriggerType.KilledByAttack;
            Name = "Final Beam";
            Description = "When Dark Gyarados is knocked out by an attack flip a coin. If heads Dark Gyarados deals 20 damage for each attached water energy, to that pokemon";
        }

        protected override void Activate(Player player, Player opponent, int damageTake)
        {
            if(CoinFlipper.FlipCoin() != CoinFlipper.HEADS)
                return;

            var damage = PokemonOwner.AttachedEnergy.Count(energy => energy.EnergyType == EnergyTypes.Water);

            PokemonOwner.KnockedOutBy.DealDamage(new Damage(damage * 20));

            if(PokemonOwner.KnockedOutBy.IsDead())
                PokemonOwner.KnockedOutBy.KnockedOutBy = PokemonOwner;
        }
    }
}
