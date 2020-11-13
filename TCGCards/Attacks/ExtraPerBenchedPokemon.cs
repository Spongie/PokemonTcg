using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class ExtraPerBenchedPokemon : Attack
    {
        private int extra;

        [DynamicInput("Extra damage")]
        public int ExtraPerPokemon
        {
            get { return extra; }
            set
            {
                extra = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return Damage + ExtraPerPokemon * owner.BenchedPokemon.Count;
        }
    }
}
