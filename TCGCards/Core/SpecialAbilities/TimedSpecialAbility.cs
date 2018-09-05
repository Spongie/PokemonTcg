namespace TCGCards.Core.SpecialAbilities
{
    public class TimedSpecialAbility
    {
        public TimedSpecialAbility() :this(2)
        {

        }

        public TimedSpecialAbility(int turnsLeft)
        {
            TurnsLeft = turnsLeft;
        }

        public int TurnsLeft { get; set; }
    }
}
