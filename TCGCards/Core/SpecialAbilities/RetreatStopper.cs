namespace TCGCards.Core.SpecialAbilities
{
    public class RetreatStopper : Ability
    {
        public RetreatStopper() :this(null, UNTIL_YOUR_NEXT_TURN)
        {

        }

        public RetreatStopper(PokemonCard owner, int turns) : base(owner)
        {
            TriggerType = TriggerType.Passive;
            TurnDuration = turns;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            
        }
    }
}
