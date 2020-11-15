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

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            owner.DiscardPile.Add(PokemonOwner);
            owner.DiscardPile.AddRange(PokemonOwner.AttachedEnergy);
            
            PokemonOwner.AttachedEnergy.Clear();
            owner.BenchedPokemon.Remove(PokemonOwner);
        }
    }
}
