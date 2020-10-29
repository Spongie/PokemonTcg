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

		public MainViewModel()
		{
			SetViewModel = new SetsViewModel();
			PokemonsViewModel = new PokemonsViewModel(SetViewModel.Sets);
			energyCardsViewModel = new EnergyCardsViewModel(SetViewModel.Sets);
			trainerCardsViewModel = new TrainerCardsViewModel(SetViewModel.Sets);
		}

		public async Task Save()
		{
			Directory.CreateDirectory("Data");
			await SetViewModel.Save();
			await PokemonsViewModel.Save();
			await energyCardsViewModel.Save();
			await trainerCardsViewModel.Save();
		}

		public async Task Load()
		{
			if (!Directory.Exists("Data"))
				return;

			await SetViewModel.LoadSets();
			await PokemonsViewModel.Load();
			await energyCardsViewModel.Load();
			await trainerCardsViewModel.Load();
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
    }
}
