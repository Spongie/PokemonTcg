using CardEditor.Views;
using System;

namespace TCGCards.Core.Abilities
{
    public class AttackStopperAbility : Ability
    {
        public AttackStopperAbility() : this(null)
        {

        }

        public AttackStopperAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Attacked;
        }

        private bool coinFlip;

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


        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            
        }

        public bool IsStopped(GameField game)
        {
            return CoinFlip && game.FlipCoins(1) == 1;
        }
    }
}
