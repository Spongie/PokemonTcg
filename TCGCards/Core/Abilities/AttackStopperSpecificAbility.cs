using CardEditor.Views;
using System;

namespace TCGCards.Core.Abilities
{
    public class AttackStopperSpecificAbility : TemporaryAbility, IAttackStoppingAbility
    {
        private bool onlySelf = true;
        private bool onlyCurrentTarget;
        private bool coinFlip;

        public AttackStopperSpecificAbility() :this(null)
        {

        }

        public AttackStopperSpecificAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Attacked;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            
        }

        public bool IsStopped(GameField game, PokemonCard attacker, PokemonCard defender)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
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
