using CardEditor.Views;
using TCGCards.TrainerEffects;

namespace TCGCards.Core.Abilities
{
    public class HealPokemonAbility : Ability
    {
        private bool coinFlip;
        private int amount;
        private TargetingMode targetingMode;

        [DynamicInput("Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Amount to heal")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Coin flip", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        public HealPokemonAbility() :this(null)
        {

        }

        public HealPokemonAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            var target = CardUtil.AskForTargetFromTargetingMode(TargetingMode, game, owner, opponent, PokemonOwner);
            target.DamageCounters -= amount;

            if (target.DamageCounters < 0)
            {
                target.DamageCounters = 0;
            }
        }
    }
}
