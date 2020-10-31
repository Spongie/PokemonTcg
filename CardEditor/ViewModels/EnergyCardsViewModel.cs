using CardEditor.Models;
using CardEditor.Util;
using CardEditor.Views;
using Entities;
using Entities.Models;
using NetworkingCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TCGCards.TrainerEffects;

namespace CardEditor.ViewModels
{
    public class EnergyCardsViewModel : DataModel
    {
        private ObservableCollection<EnergyCard> energyCards = new ObservableCollection<EnergyCard>();
        private ObservableCollection<Set> sets;
        private List<EnergyCard> filteredCards = new List<EnergyCard>();
        private Set selectedSet; 
        private EnergyCard selectedEnergyCard;
        private IEffect selectedEffect;

        public EnergyCardsViewModel()
        {

        }

        public EnergyCardsViewModel(ObservableCollection<Set> sets)
        {
            energyCards.CollectionChanged += EnergyCards_CollectionChanged;
            PropertyChanged += EnergyCardsViewModel_PropertyChanged;
            Sets = sets;
            AddEnergyCardCommand = new RelayCommand((x) => true, AddEnergyCard);
            ImportEnergySetCommand = new AsyncRelayCommand((x) => true, ImportEnergyCardsFromSet);
            AddEffectCommand = new RelayCommand(CanAddEffect, AddEffect);
        }

        private void AddEffect(object obj)
        {
            var window = new AddTrainerEffectWindow();

            if (window.ShowDialog().Value)
            {
                SelectedEnergyCard.Effects.Add(window.SelectedEffect);
                SelectedEffect = SelectedEnergyCard.Effects.Last();
            }
        }

        private bool CanAddEffect(object obj)
        {
            return SelectedEnergyCard != null;
        }

        internal async Task Save()
        {
            var json = Serializer.Serialize(EnergyCards);

            await File.WriteAllTextAsync("Data/energy.json", json);
        }

        internal async Task Load()
        {
            if (!File.Exists("Data/energy.json"))
            {
                return;
            }

            var json = await File.ReadAllTextAsync("Data/energy.json");

            EnergyCards.Clear();

            foreach (var energy in Serializer.Deserialize<List<EnergyCard>>(json))
            {
                EnergyCards.Add(energy);
            }
        }

        private void AddEnergyCard(object obj)
        {
            EnergyCards.Add(new EnergyCard() { Name = "New Energy" });
            SelectedEnergyCard = EnergyCards.Last();
        }

        private void EnergyCardsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SelectedSet) || SelectedSet == null)
                return;

            UpdateSetFilter();
        }

        private void EnergyCards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateSetFilter();
        }

        private async Task ImportEnergyCardsFromSet(object arg)
        {
            using (var client = new WebClient())
            {
                var json = await client.DownloadStringTaskAsync($"https://api.pokemontcg.io/v1/cards?setCode={SelectedSet.SetCode}&supertype=Energy").ConfigureAwait(false);
                var headers = client.ResponseHeaders;

                int cardsFound = int.Parse(headers.Get("Count"));
                int pageSize = int.Parse(headers.Get("Page-Size"));

                if (cardsFound > pageSize)
                {
                    //TODO
                }

                foreach (var card in JsonConvert.DeserializeObject<PokemonTcgSdk.Energy>(json).Cards)
                {
                    EnergyCards.Add(new EnergyCard()
                    {
                        Name = card.Name,
                        ImageUrl = card.ImageUrlHiRes,
                        SetCode = card.SetCode,
                        IsBasic = card.SubType.ToLower() == "basic",
                        Amount = 1,
                        EnergyType = EnergyTypes.Colorless
                    });
                }

                SelectedEnergyCard = EnergyCards.Last();
            }
        }

        private void UpdateSetFilter()
        {
            if (SelectedSet == null)
            {
                FilteredCards = EnergyCards.ToList();
                return;
            }

            FilteredCards = EnergyCards.Where(card => card.SetCode == SelectedSet.SetCode).ToList();
        }

        public IEffect SelectedEffect
        {
            get { return selectedEffect; }
            set
            {
                selectedEffect = value;
                FirePropertyChanged();
            }
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


        public List<EnergyCard> FilteredCards
        {
            get { return filteredCards; }
            set
            {
                filteredCards = value;
                FirePropertyChanged();
            }
        }


        public ObservableCollection<EnergyCard> EnergyCards
        {
            get { return energyCards; }
            set
            {
                energyCards = value;
                FirePropertyChanged();
            }
        }

        public EnergyCard SelectedEnergyCard
        {
            get { return selectedEnergyCard; }
            set
            {
                selectedEnergyCard = value;
                FirePropertyChanged();
                FirePropertyChanged(nameof(ImageSrc));
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

        public BitmapImage ImageSrc
        {
            get
            {
                if (SelectedEnergyCard == null)
                    return null;
                return new BitmapImage(new Uri(SelectedEnergyCard.ImageUrl, UriKind.Absolute));
            }
        }

        public ICommand AddEnergyCardCommand { get; set; }
        public ICommand ImportEnergySetCommand { get; set; }
        public ICommand AddEffectCommand { get; set; }
    }
}
