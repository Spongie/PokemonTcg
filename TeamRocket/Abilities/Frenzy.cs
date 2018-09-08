using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class Frenzy : Ability
    {
        public Frenzy(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.DealsDamage;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken)
        {
            ((PokemonCard)target).DealDamage(new Damage(30));
        }

        public override bool CanActivate()
        {
            return PokemonOwner.IsConfused;
        }
    }
}
