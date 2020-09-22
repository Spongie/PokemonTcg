using CardEditor.Models;
using CardEditor.Views;
using Entities;
using Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CardEditor.ViewModels
{
    public class PokemonsViewModel : DataModel
    {
		private ObservableCollection<PokemonCard> pokemonCards = new ObservableCollection<PokemonCard>();
		private List<PokemonCard> filteredPokemonCards = new List<PokemonCard>();
		private List<string> allPokemonNames = new List<string>();
		private PokemonCard pokemonCard;
		private Set selectedSet;
		private ObservableCollection<Set> sets = new ObservableCollection<Set>();

		public PokemonsViewModel()
		{

		}

		public PokemonsViewModel(ObservableCollection<Set> sets)
		{
			AddPokemonCommand = new RelayCommand(IsReady, AddNewPokemon);
			ImportPokemonCommand = new RelayCommand(IsReady, ImportPokemon);
			Sets = sets;
			pokemonCards.CollectionChanged += PokemonCards_CollectionChanged;
			PropertyChanged += PokemonsViewModel_PropertyChanged;
			SelectedSet = Sets.FirstOrDefault();
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

						var pokemon = new PokemonCard
						{
							Name = pokemonSdk.Card.Name,
							Hp = int.Parse(pokemonSdk.Card.Hp),
							ImageUrl = pokemonSdk.Card.ImageUrlHiRes,
							EvolvesFrom = pokemonSdk.Card.EvolvesFrom.Replace("-", string.Empty),
							SetCode = pokemonSdk.Card.SetCode,
							Stage = SubTypToStage(pokemonSdk.Card.SubType),
							RetreatCost = pokemonSdk.Card.ConvertedRetreatCost,
							Type = convertFullTypeToType(pokemonSdk.Card.Types.First())
						};

						if (pokemonSdk.Card.Weaknesses != null)
						{
							pokemon.Weakness = convertFullTypeToType(pokemonSdk.Card.Weaknesses.First().Type);
						}
						else
						{
							pokemon.Weakness = EnergyTypes.None;
						}
						if (pokemonSdk.Card.Resistances != null)
						{
							pokemon.Resistance = convertFullTypeToType(pokemonSdk.Card.Resistances.First().Type);
						}
						else
						{
							pokemon.Resistance = EnergyTypes.None;
						}

						PokemonCards.Add(pokemon);
						SelectedCard = PokemonCards.Last();
					}
					catch
					{

					}
				}
			}
		}

		private static EnergyTypes convertFullTypeToType(string type)
		{
			switch (type)
			{
				case "Psychic":
					return EnergyTypes.Psychic;
				case "Grass":
					return EnergyTypes.Grass;
				case "Fire":
					return EnergyTypes.Fire;
				case "Water":
					return EnergyTypes.Water;
				case "Colorless":
					return EnergyTypes.Colorless;
				case "Fighting":
					return EnergyTypes.Fighting;
				case "Lightning":
					return EnergyTypes.Electric;
				case "":
				case "none":
				case null:
					return EnergyTypes.None;
				default:
					throw new InvalidOperationException(type);
			}
		}

		private int SubTypToStage(string subtype)
		{
			switch (subtype.ToLower())
			{
				case "basic":
					return 0;
				case "stage 1":
					return 1;
				case "stage 2":
					return 2;
				default:
					return 0;
			}
		}

		private void AddNewPokemon(object obj)
		{
			PokemonCards.Add(new PokemonCard());
			SelectedCard = PokemonCards.Last();
		}

		private bool IsReady(object obj)
		{
			return true;
		}

		internal async Task Save()
		{
			var json = JsonConvert.SerializeObject(PokemonCards.ToList());

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

			foreach (var pokemon in JsonConvert.DeserializeObject<List<PokemonCard>>(json))
			{
				PokemonCards.Add(pokemon);
			}
		}

		private void PokemonCards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			UpdatePokemonFilter();
			AllPokemonNames = PokemonCards.Select(p => p.Name).Distinct().ToList();
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

			FilteredPokemonCards = pokemonCards.Where(card => card.SetCode == SelectedSet.SetCode).ToList();
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

		public List<PokemonCard> FilteredPokemonCards
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

		public ObservableCollection<PokemonCard> PokemonCards
		{
			get { return pokemonCards; }
			set
			{
				pokemonCards = value;
				FirePropertyChanged();
			}
		}

		public PokemonCard SelectedCard
		{
			get { return pokemonCard; }
			set
			{
				pokemonCard = value;
				FirePropertyChanged();
			}
		}

		public ICommand ImportPokemonCommand { get; set; }
		public ICommand AddPokemonCommand { get; set; }
	}
}
