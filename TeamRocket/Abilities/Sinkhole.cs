using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class Sinkhole : Ability
    {
        public Sinkhole(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.OpponentRetreats;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken)
        {
            opponent.ActivePokemonCard.DealDamage(new Damage(0, 20));
        }
    }
}
