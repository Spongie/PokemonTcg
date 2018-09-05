namespace TCGCards.Core
{
    public abstract class Ability
    {
        protected Ability(PokemonCard owner)
        {
            Owner = owner;
        }

        public abstract void Activate(Player owner, Player opponent, int damageTaken);

        public abstract void SetTarget(Card target);

        public TriggerType TriggerType { get; protected set; }

        public PokemonCard Owner { get; protected set; }       
    }
}
