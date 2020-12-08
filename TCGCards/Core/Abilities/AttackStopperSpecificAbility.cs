using CardEditor.Views;

namespace TCGCards.Core.Abilities
{
    public class AttackStopperSpecificAbility : Ability, IAttackStoppingAbility
    {
        private bool onlySelf = true;
        private bool onlyCurrentTarget;
        private bool coinFlip;
        private bool stopOnTails;

        public AttackStopperSpecificAbility() :this(null)
        {

        }

        public AttackStopperSpecificAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Attacked;
            TurnDuration = UNTIL_YOUR_NEXT_TURN;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            
        }

        public bool IsStopped(GameField game, PokemonCard attacker, PokemonCard defender)
        {
            if (!game.IsSuccessfulFlip(CoinFlip, false, StopOnTails))
            {
                return false;
            }

            if (onlySelf && defender != PokemonOwner)
            {
                return false;
            }

            if (onlyCurrentTarget && CurrentTarget != attacker)
            {
                return false;
            }

            return true;
        }

        [DynamicInput("Stop on tails instead", InputControl.Boolean)]
        public bool StopOnTails
        {
            get { return stopOnTails; }
            set
            {
                stopOnTails = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Coin Flip", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only prevent on self", InputControl.Boolean)]
        public bool OnlySelf
        {
            get { return onlySelf; }
            set
            {
                onlySelf = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only prevent from current target", InputControl.Boolean)]
        public bool OnlyCurrentTarget
        {
            get { return onlyCurrentTarget; }
            set
            {
                onlyCurrentTarget = value;
                FirePropertyChanged();
            }
        }

        public PokemonCard CurrentTarget { get; set; }
    }
}
