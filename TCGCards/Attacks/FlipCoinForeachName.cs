using CardEditor.Views;
using Entities;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class FlipCoinForeachName : Attack
    {
        private bool opponents;
        private string names;

        [DynamicInput("Look at opponents Pokémon?", InputControl.Boolean)]
        public bool Opponents
        {
            get { return opponents; }
            set
            {
                opponents = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Valid names (split with ;)")]
        public string Names
        {
            get { return names; }
            set
            {
                names = value;
                FirePropertyChanged();
            }
        }
        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int coins = 0;
            string[] names = Names.Split(';');

            foreach (var pokemon in owner.GetAllPokemonCards())
            {
                if (names.Contains(pokemon.Name))
                {
                    coins++;
                }
            }

            if (opponents)
            {
                foreach (var pokemon in opponent.GetAllPokemonCards())
                {
                    if (names.Contains(pokemon.Name))
                    {
                        coins++;
                    }
                }
            }

            return Damage * game.FlipCoins(coins);
        }
    }
}
