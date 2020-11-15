using System;
using System.Collections.Generic;
using System.Text;

namespace TCGCards.Core.Abilities
{
    public class BecomeActivePokemon : Ability
    {
        public BecomeActivePokemon() :this(null)
        {

        }

        public BecomeActivePokemon(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
        }

        public override bool CanActivate()
        {
            return PokemonOwner.Owner.BenchedPokemon.Contains(PokemonOwner) && base.CanActivate();
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            owner.SwapActivePokemon(PokemonOwner);
        }
    }
}
