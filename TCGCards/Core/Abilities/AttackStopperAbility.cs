using CardEditor.Views;

namespace TCGCards.Core.Abilities
{
    public class AttackStopperAbility : Ability, IAttackStoppingAbility
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

        public bool IsStopped(GameField game, PokemonCard attacker, PokemonCard defender)
        {
            return CoinFlip && game.FlipCoins(1) == 1;
        }
    }
}
