namespace TCGCards.Core.SpecialAbilities
{
    public class RetreatStopper : TemporaryAbility
    {
        public RetreatStopper(PokemonCard owner, int turnCount) : base(owner, turnCount)
        {
            TriggerType = TriggerType.Passive;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            
        }
    }
}
