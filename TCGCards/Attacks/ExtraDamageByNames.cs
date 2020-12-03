using CardEditor.Views;
using Entities;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class ExtraDamageByNames : Attack
    {
        private string names;
        private int extra;
        private bool alsoCheckOpponent;

        [DynamicInput("Count opponents pokemon also", InputControl.Boolean)]
        public bool AlsoCountOpponents
        {
            get { return alsoCheckOpponent; }
            set
            {
                alsoCheckOpponent = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Extra Damage")]
        public int ExtraDamage
        {
            get { return extra; }
            set
            {
                extra = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Names to search for (Split with ;)")]
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
            var names = Names.ToLower().Split(';');
            var damage = Damage;

            foreach (var pokemon in owner.GetAllPokemonCards())
            {
                if (names.Contains(pokemon.Name.ToLower()))
                {
                    damage += ExtraDamage;
                }
            }

            if (alsoCheckOpponent)
            {
                foreach (var pokemon in opponent.GetAllPokemonCards())
                {
                    if (names.Contains(pokemon.Name.ToLower()))
                    {
                        damage += ExtraDamage;
                    }
                }
            }

            return damage;
        }
    }
}
