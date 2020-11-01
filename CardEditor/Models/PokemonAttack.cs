using Entities.Models;
using TCGCards;

namespace CardEditor.Models
{
    public class PokemonAttack : DataModel
    {
        private Attack attack;
        private PokemonCard pokemon;

        public PokemonCard Pokemon
        {
            get { return pokemon; }
            set
            {
                pokemon = value;
                FirePropertyChanged();
            }
        }

        public Attack Attack
        {
            get { return attack; }
            set
            {
                attack = value;
                FirePropertyChanged();
            }
        }

        public string Display
        {
            get
            {
                return $"{Pokemon.Name} ({Pokemon.SetCode}) - {Attack.Name} {Attack.Damage}";
            }
        }
    }
}
