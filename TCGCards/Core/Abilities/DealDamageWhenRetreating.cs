using CardEditor.Views;

namespace TCGCards.Core.Abilities
{
    public class DealDamageWhenRetreating : Ability
    {
        public DealDamageWhenRetreating() :this(null)
        {
        }

        public DealDamageWhenRetreating(PokemonCard owner) :base(owner)
        {
            TriggerType = TriggerType.OpponentRetreats;
        }

        private bool coinFlip;
        private int damage;

        [DynamicInput("Coin flip?", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage")]
        public int Damage
        {
            get { return damage; }
            set
            {
                damage = value;
                FirePropertyChanged();
            }
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            if (target == null)
            {
                return;
            }

            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            ((PokemonCard)target).DealDamage(Damage, game, PokemonOwner, false);
        }
    }
}
