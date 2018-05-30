namespace TCGCards.Core
{
    public abstract class Ability
    {
        protected Ability(IPokemonCard owner)
        {
            Owner = owner;
        }

        public abstract void Activate(Player owner, Player opponent);

        public abstract void SetTarget(IPokemonCard target);

        public TriggerType TriggerType { get; protected set; }

        public IPokemonCard Owner { get; protected set; }       
    }
}
