using NetworkingCore;

namespace TCGCards.Core
{
    public abstract class Ability
    {
        protected Card target;

        protected Ability(PokemonCard pokemonOwner)
        {
            PokemonOwner = pokemonOwner;
            Id = NetworkId.Generate();
        }

        protected abstract void Activate(Player owner, Player opponent, int damageTaken);

        public TriggerType TriggerType { get; protected set; }

        public PokemonCard PokemonOwner { get; protected set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public NetworkId Id { get; protected set; }

        public void Trigger(Player owner, Player opponent, int damageTaken)
        {
            if (CanActivate())
                Activate(owner, opponent, damageTaken);
        }

        public virtual bool CanActivate()
        {
            return !PokemonOwner.IsAsleep && !PokemonOwner.IsConfused && !PokemonOwner.IsParalyzed;
        }

        public virtual void SetTarget(Card target)
        {
            this.target = target;
        }
    }
}
