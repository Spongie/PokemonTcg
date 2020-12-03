using TCGCards.Core.GameEvents;

namespace TCGCards.Core.Abilities
{
    public class DiscardSelfAbility : Ability
    {
        public DiscardSelfAbility() :this(null)
        {
            
        }

        public DiscardSelfAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
        }

        public override bool CanActivate(GameField game, Player caster, Player opponent)
        {
            return PokemonOwner.Owner.BenchedPokemon.Contains(PokemonOwner) && base.CanActivate(game, caster, opponent);
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            owner.DiscardPile.Add(PokemonOwner);
            foreach (var card in PokemonOwner.AttachedEnergy)
            {
                game?.SendEventToPlayers(new AttachedEnergyDiscardedEvent { FromPokemonId = PokemonOwner.Id, DiscardedCard = card });
            }
            owner.DiscardPile.AddRange(PokemonOwner.AttachedEnergy);
            
            PokemonOwner.AttachedEnergy.Clear();
            owner.BenchedPokemon.Remove(PokemonOwner);
            game?.SendEventToPlayers(new PokemonRemovedFromBench { PokemonId = target.Id });
        }
    }
}
