using CardEditor.Views;
using Entities.Models;
using NetworkingCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TCGCards;
using TCGCards.TrainerEffects;

namespace CardEditor.ViewModels
{
    public class TrainerCardsViewModel : DataModel
    {
        private ObservableCollection<TrainerCard> energyCards = new ObservableCollection<TrainerCard>();
        private ObservableCollection<Set> sets;
        private List<TrainerCard> filteredCards = new List<TrainerCard>();
        private Set selectedSet;
        private TrainerCard selectedEnergyCard;
        private IEffect selectedEffect;

        public TrainerCardsViewModel()
        {

        }

        public TrainerCardsViewModel(ObservableCollection<Set> sets)
        {
            energyCards.CollectionChanged += EnergyCards_CollectionChanged;
            PropertyChanged += EnergyCardsViewModel_PropertyChanged;
            Sets = sets;
            AddTrainerCardCommand = new RelayCommand((x) => true, AddTrainerCard);
            ImportTrainerSetCommand = new AsyncRelayCommand((x) => true, ImportTrainerCardsFromSet);
            AddEffectCommand = new RelayCommand((x) => SelectedTrainerCard != null, AddEffect);
        }

        private void AddEffect(object obj)
        {
            var window = new AddTrainerEffectWindow();

            if (window.ShowDialog().Value)
            {
                SelectedTrainerCard.Effects.Add(window.SelectedEffect);
                SelectedEffect = SelectedTrainerCard.Effects.Last();
            }
        }

        internal async Task Save()
        {
            var json = Serializer.SerializeFormatted(TrainerCards);

            await File.WriteAllTextAsync("Data/trainers.json", json);
        }

        internal async Task Load()
        {
            if (!File.Exists("Data/trainers.json"))
            {
                return;
            }

            var json = await File.ReadAllTextAsync("Data/trainers.json");

            TrainerCards.Clear();

            foreach (var trainer in Serializer.Deserialize<List<TrainerCard>>(json))
            {
                if (trainer.CardId == null)
                {
                    trainer.CardId = NetworkId.Generate();
                }
                TrainerCards.Add(trainer);
            }
        }

        private void AddTrainerCard(object obj)
        {
            TrainerCards.Add(new TrainerCard() { Name = "New Trainer", CardId = NetworkId.Generate() });
            SelectedTrainerCard = TrainerCards.Last();
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

        private async Task ImportTrainerCardsFromSet(object arg)
        {
            using (var client = new WebClient())
            {
                var json = await client.DownloadStringTaskAsync($"https://api.pokemontcg.io/v1/cards?setCode={SelectedSet.SetCode}&supertype=Trainer").ConfigureAwait(false);
                var headers = client.ResponseHeaders;

                int cardsFound = int.Parse(headers.Get("Count"));
                int pageSize = int.Parse(headers.Get("Page-Size"));

                if (cardsFound > pageSize)
                {
                    //TODO
                }

                foreach (var card in JsonConvert.DeserializeObject<PokemonTcgSdk.Trainer>(json).Cards)
                {
                    TrainerCards.Add(new TrainerCard
                    {
                        Name = card.Name,
                        ImageUrl = card.ImageUrlHiRes,
                        Completed = false,
                        SetCode = card.SetCode,
                        CardId = NetworkId.Generate()
                    });
                }

                SelectedTrainerCard = TrainerCards.First();
            }
        }

        private void UpdateSetFilter()
        {
            if (SelectedSet == null)
            {
                FilteredCards = TrainerCards.ToList();
                return;
            }

            FilteredCards = TrainerCards.Where(card => card.SetCode == SelectedSet.SetCode).ToList();
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


        public List<TrainerCard> FilteredCards
        {
            get { return filteredCards; }
            set
            {
                filteredCards = value;
                FirePropertyChanged();
            }
        }


        public ObservableCollection<TrainerCard> TrainerCards
        {
            get { return energyCards; }
            set
            {
                energyCards = value;
                FirePropertyChanged();
            }
        }

        public TrainerCard SelectedTrainerCard
        {
            get { return selectedEnergyCard; }
            set
            {
                selectedEnergyCard = value;
                FirePropertyChanged();
                FirePropertyChanged(nameof(ImageSrc));
            }
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
                if (SelectedTrainerCard == null || SelectedTrainerCard.ImageUrl == null)
                    return null;
                return new BitmapImage(new Uri(SelectedTrainerCard.ImageUrl, UriKind.Absolute));
            }
        }

        public ICommand AddTrainerCardCommand { get; set; }
        public ICommand ImportTrainerSetCommand { get; set; }
        public ICommand AddEffectCommand { get; set; }
    }
}
