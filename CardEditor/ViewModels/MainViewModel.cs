using Entities.Models;
using System.IO;
using System.Threading.Tasks;

namespace CardEditor.ViewModels
{
    public class MainViewModel : DataModel
    {
		private SetsViewModel setViewModel;
		private PokemonsViewModel pokemonsViewModel;
		private EnergyCardsViewModel energyCardsViewModel;
		private TrainerCardsViewModel trainerCardsViewModel;
		private FormatsViewModel formatViewModel;

		public MainViewModel()
		{
			SetViewModel = new SetsViewModel();
			PokemonsViewModel = new PokemonsViewModel(SetViewModel.Sets);
			EnergyCardsViewModel = new EnergyCardsViewModel(SetViewModel.Sets);
			TrainerCardViewModel = new TrainerCardsViewModel(SetViewModel.Sets);
			FormatViewModel = new FormatsViewModel(this, SetViewModel.Sets);
		}

        internal async Task Commit()
        {
			await Save();

			File.Copy("Data/sets.json", @"E:\Programming\PokemonTcg\Data\sets.json", true);
			File.Copy("Data/pokemon.json", @"E:\Programming\PokemonTcg\Data\pokemon.json", true);
			File.Copy("Data/energy.json", @"E:\Programming\PokemonTcg\Data\energy.json", true);
			File.Copy("Data/trainers.json", @"E:\Programming\PokemonTcg\Data\trainers.json", true);
			File.Copy("Data/formats.json", @"E:\Programming\PokemonTcg\Data\formats.json", true);
		}

        public async Task Save()
		{
			Directory.CreateDirectory("Data");
			await SetViewModel.Save();
			await PokemonsViewModel.Save();
			await energyCardsViewModel.Save();
			await trainerCardsViewModel.Save();
			await formatViewModel.Save();
		}

		public async Task Load()
		{
			if (!Directory.Exists("Data"))
				return;

			await SetViewModel.LoadSets();
			await PokemonsViewModel.Load();
			await energyCardsViewModel.Load();
			await trainerCardsViewModel.Load();
			await formatViewModel.Load();
		}

        public TrainerCardsViewModel TrainerCardViewModel
        {
            get { return trainerCardsViewModel; }
            set
            {
                trainerCardsViewModel = value;
                FirePropertyChanged();
            }
        }

        public PokemonsViewModel PokemonsViewModel
		{
			get { return pokemonsViewModel; }
			set
			{
				pokemonsViewModel = value;
				FirePropertyChanged();
			}
		}

		public SetsViewModel SetViewModel
		{
			get { return setViewModel; }
			set
			{
				setViewModel = value;
				FirePropertyChanged();
			}
		}

        public EnergyCardsViewModel EnergyCardsViewModel
        {
            get { return energyCardsViewModel; }
            set
            {
                energyCardsViewModel = value;
                FirePropertyChanged();
            }
        }

		public FormatsViewModel FormatViewModel
		{
			get { return formatViewModel; }
			set
			{
				formatViewModel = value;
				FirePropertyChanged();
			}
		}
	}
}
