using CardEditor.Models;
using Entities.Models;
using System.Collections.ObjectModel;

namespace CardEditor.ViewModels
{
    public class CopyAttackViewModel : DataModel
    {
        private ObservableCollection<PokemonAttack> pokemonAttacks = new ObservableCollection<PokemonAttack>();

        public CopyAttackViewModel()
        {
        }

        public ObservableCollection<PokemonAttack> PokemonAttacks
        {
            get { return pokemonAttacks; }
            set
            {
                pokemonAttacks = value;
                FirePropertyChanged();
            }
        }

    }
}
