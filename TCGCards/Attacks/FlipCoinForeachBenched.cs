using CardEditor.Views;
using Entities;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class FlipCoinForeachBenched : Attack
    {
        private bool opponentsBench;

        [DynamicInput("Look at opponent bench?", InputControl.Boolean)]
        public bool OpponentsBench
        {
            get { return opponentsBench; }
            set
            {
                opponentsBench = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int count = OpponentsBench ? opponent.BenchedPokemon.Count : owner.BenchedPokemon.Count;
            return game.FlipCoins(count) * Damage;
        }
    }
}
