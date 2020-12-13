using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class SelfDamageTailsExtraHeadsAttack : Attack
    {
        private int selfDamage;
        private int extraDamage;
        private int coins = 1;

        [DynamicInput("Number of coins")]
        public int Coins
        {
            get { return coins; }
            set
            {
                coins = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("SelfDamage when tails")]
        public int SelfDamage
        {
            get { return selfDamage; }
            set
            {
                selfDamage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Extra Damage when heads")]
        public int ExtraDamage
        {
            get { return extraDamage; }
            set
            {
                extraDamage = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int heads = game.FlipCoins(Coins);
            int tails = Coins - heads;

            if (tails > 0)
            {
                owner.ActivePokemonCard.DealDamage(SelfDamage * tails, game, owner.ActivePokemonCard, true);
            }

            return Damage + (extraDamage * heads);
        }
    }
}
