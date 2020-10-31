﻿using CardEditor.Models;
using CardEditor.Util;
using CardEditor.Views;
using Entities.Models;
using NetworkingCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TCGCards;

namespace CardEditor.ViewModels
{
    public class PokemonsViewModel : DataModel
    {
		private ObservableCollection<PokemonViewModel> pokemonCards = new ObservableCollection<PokemonViewModel>();
		private List<PokemonViewModel> filteredPokemonCards = new List<PokemonViewModel>();
		private List<string> allPokemonNames = new List<string>();
		private PokemonViewModel pokemonCard;
		private Set selectedSet;
		private ObservableCollection<Set> sets = new ObservableCollection<Set>();

		public PokemonsViewModel()
		{

		}

		public PokemonsViewModel(ObservableCollection<Set> sets)
		{
			AddPokemonCommand = new RelayCommand(IsReady, AddNewPokemon);
			ImportPokemonCommand = new RelayCommand(IsReady, ImportPokemon);
			ImportPokemonSetCommand = new AsyncRelayCommand(IsSetSelected, ImportPokemonSet);
			Sets = sets;
			pokemonCards.CollectionChanged += PokemonCards_CollectionChanged;
			PropertyChanged += PokemonsViewModel_PropertyChanged;
			SelectedSet = Sets.FirstOrDefault();
		}

		private bool IsSetSelected(object obj) => SelectedSet != null;

        private async Task ImportPokemonSet(object arg)
		{
			using (var client = new WebClient())
			{
				var json = await client.DownloadStringTaskAsync($"https://api.pokemontcg.io/v1/cards?setCode={SelectedSet.SetCode}&supertype=Pok%C3%A9mon").ConfigureAwait(false);
				var headers = client.ResponseHeaders;

				int cardsFound = int.Parse(headers.Get("Count"));
				int pageSize = int.Parse(headers.Get("Page-Size"));

				if (cardsFound > pageSize)
				{
					//TODO
				}
				var pokemons = new List<PokemonViewModel>();
				foreach (var card in JsonConvert.DeserializeObject<JsonPokemonList>(json).Cards)
				{
					var pokemon = PokemonCreator.CreateCardFromSdkCard(card);

					pokemons.Add(new PokemonViewModel(pokemon));
				}

                foreach (var pokemon in pokemons.OrderBy(x => x.Card.Name))
                {
					PokemonCards.Add(pokemon);
                }

				SelectedCard = PokemonCards.First();
			}
		}

		private void ImportPokemon(object obj)
		{
			var dialog = new ImportPokemon();
			if (dialog.ShowDialog().Value)
			{
				using (var client = new WebClient())
				{
					try
					{
						var json = client.DownloadString(dialog.Url);
						var pokemonSdk = JsonConvert.DeserializeObject<JsonPokemon>(json);

						var pokemon = PokemonCreator.CreateCardFromSdkCard(pokemonSdk);

						PokemonCards.Add(new PokemonViewModel(pokemon));
						SelectedCard = PokemonCards.Last();
					}
					catch
					{

					}
				}
			}
		}

		private void AddNewPokemon(object obj)
		{
			PokemonCards.Add(new PokemonViewModel());
			SelectedCard = PokemonCards.Last();
		}

		private bool IsReady(object obj)
		{
			return true;
		}

		internal async Task Save()
		{
			var json = Serializer.Serialize(PokemonCards.Select(card => card.Card).ToList());

			await File.WriteAllTextAsync("Data/pokemon.json", json);
		}

		internal async Task Load()
		{
			if (!File.Exists("Data/pokemon.json"))
			{
				return;
			}

			var json = await File.ReadAllTextAsync("Data/pokemon.json");

			PokemonCards.Clear();

			foreach (var pokemon in Serializer.Deserialize<List<PokemonCard>>(json))
			{
				PokemonCards.Add(new PokemonViewModel(pokemon));
			}
		}

		private void PokemonCards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			UpdatePokemonFilter();
			AllPokemonNames = PokemonCards.Select(p => p.Card.Name).Distinct().ToList();
			AllPokemonNames.Insert(0, string.Empty);
		}

		private void PokemonsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(SelectedSet) || SelectedSet == null)
				return;

			UpdatePokemonFilter();
		}

		private void UpdatePokemonFilter()
		{
			if (SelectedSet == null)
			{
				FilteredPokemonCards = PokemonCards.ToList();
				return;
			}

			FilteredPokemonCards = pokemonCards.Where(card => card.Card.SetCode == SelectedSet.SetCode).ToList();
		}

		public ObservableCollection<Set> Sets
		{
			get { return sets; }
			set
			{
				sets = value;
				FirePropertyChanged();
			}
		}

		public Set SelectedSet
		{
			get { return selectedSet; }
			set
			{
				selectedSet = value;
				FirePropertyChanged();
			}
		}

		public List<PokemonViewModel> FilteredPokemonCards
		{
			get { return filteredPokemonCards; }
			set
			{
				filteredPokemonCards = value;
				FirePropertyChanged();
			}
		}

		public List<string> AllPokemonNames
		{
			get { return allPokemonNames; }
			set
			{
				allPokemonNames = value;
				FirePropertyChanged();
			}
		}

		public ObservableCollection<PokemonViewModel> PokemonCards
		{
			get { return pokemonCards; }
			set
			{
				pokemonCards = value;
				FirePropertyChanged();
			}
		}

		public PokemonViewModel SelectedCard
		{
			get { return pokemonCard; }
			set
			{
				pokemonCard = value;
				FirePropertyChanged();
				FirePropertyChanged(nameof(ImageSrc));
			}
		}

        public BitmapImage ImageSrc
        {
            get
            {
				if (SelectedCard == null)
					return null;
                return new BitmapImage(new Uri(SelectedCard.Card.ImageUrl, UriKind.Absolute));
            }
        }

        public ICommand ImportPokemonCommand { get; set; }
		public ICommand AddPokemonCommand { get; set; }
		public ICommand ImportPokemonSetCommand { get; set; }
	}
}
