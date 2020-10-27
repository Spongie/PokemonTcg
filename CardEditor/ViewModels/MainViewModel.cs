using CardEditor.Views;
using Entities.Models;
using PokemonTcgSdk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CardEditor.ViewModels
{
    public class MainViewModel : DataModel
    {
		private SetsViewModel setViewModel;
		private PokemonsViewModel pokemonsViewModel;
		private EnergyCardsViewModel energyCardsViewModel;

		public MainViewModel()
		{
			SetViewModel = new SetsViewModel();
			PokemonsViewModel = new PokemonsViewModel(SetViewModel.Sets);
			energyCardsViewModel = new EnergyCardsViewModel(SetViewModel.Sets);
		}

		public async Task Save()
		{
			Directory.CreateDirectory("Data");
			await SetViewModel.Save();
			await PokemonsViewModel.Save();
			await energyCardsViewModel.Save();
		}

		public async Task Load()
		{
			if (!Directory.Exists("Data"))
				return;

			await SetViewModel.LoadSets();
			await PokemonsViewModel.Load();
			await energyCardsViewModel.Load();
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
