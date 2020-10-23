using CardEditor.Views;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards.Attacks
{
    public class ApplyAttackPrevention : Attack
    {
        private bool coinFlip;

        [DynamicInput("Coin flipped", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            owner.ActivePokemonCard.AttackStoppers.Add(new AttackStopper((x) => x == owner.ActivePokemonCard ));
        }
    }
}
