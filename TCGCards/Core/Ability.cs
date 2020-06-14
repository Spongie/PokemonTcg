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

        protected abstract void Activate(Player owner, Player opponent, int damageTaken, GameLog log);

        public TriggerType TriggerType { get; protected set; }

        public PokemonCard PokemonOwner { get; protected set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public NetworkId Id { get; protected set; }
        public int Usages { get; set; } = 1;
        public int UsedTimes { get; set; }

        public void Trigger(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            if (CanActivate())
            {
                UsedTimes++;
                Activate(owner, opponent, damageTaken, log);
            }
            else
            {
                log.AddMessage(Name + " did not activate because of something");
            }
        }

        public virtual bool CanActivate()
        {
            return !PokemonOwner.AbilityDisabled && !PokemonOwner.IsAsleep && !PokemonOwner.IsConfused && !PokemonOwner.IsParalyzed && UsedTimes < Usages;
        }

        public virtual void SetTarget(Card target)
        {
            this.target = target;
        }
    }
}
