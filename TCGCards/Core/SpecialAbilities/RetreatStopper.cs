namespace TCGCards.Core.SpecialAbilities
{
    public class RetreatStopper : TemporaryAbility
    {
        public RetreatStopper() :this(null, 2)
        {

        }

        public RetreatStopper(PokemonCard owner, int turnCount) : base(owner, turnCount)
        {
            TriggerType = TriggerType.Passive;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            
        }
    }
}
