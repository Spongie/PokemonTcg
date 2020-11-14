namespace TCGCards.Core
{
    public abstract class PassiveAbility : Ability
    {
        protected PassiveAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Passive;
        }

        public PassiveModifierType ModifierType { get; set; }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
        }
    }
}
