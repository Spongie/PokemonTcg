using System;

namespace TCGCards.Core.Abilities
{
    public class AttackStopperAbility : Ability
    {
        public AttackStopperAbility() : this(null)
        {

        }

        public AttackStopperAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Attacked;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            
        }
    }
}
