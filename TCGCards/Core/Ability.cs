namespace TCGCards.Core
{
    public abstract class Ability
    {
        protected Ability(PokemonCard pokemonOwner)
        {
            PokemonOwner = pokemonOwner;
        }

        public abstract void Activate(Player owner, Player opponent, int damageTaken);

        public abstract void SetTarget(Card target);

        public TriggerType TriggerType { get; protected set; }

        public PokemonCard PokemonOwner { get; protected set; }       
    }
}
