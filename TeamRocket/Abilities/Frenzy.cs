using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class Frenzy : Ability
    {
        public Frenzy(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.DealsDamage;
            Name = "Frenzy";
            Description = "If Dark Primeape is confused and deals damage it deals an additional 30 damage";
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            ((PokemonCard)target).DealDamage(new Damage(30), log);
        }

        public override bool CanActivate()
        {
            return PokemonOwner.IsConfused;
        }
    }
}
