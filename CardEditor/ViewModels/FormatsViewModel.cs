using Entities;
using Entities.Models;
using NetworkingCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TCGCards;

namespace CardEditor.ViewModels
{
    public class FormatsViewModel : DataModel
    {
        private ObservableCollection<Format> formats;
        private Format selectedFormat;
        private ObservableCollection<Set> sets;

        public FormatsViewModel()
        {
            Formats = new ObservableCollection<Format>();
            AddNewCommand = new RelayCommand((x) => true, (x) =>
            {
                Formats.Add(new Format() { Name = "New format", Id = NetworkId.Generate() });
                SelectedFormat = Formats.Last();
            });
            DeleteRestriction = new RelayCommand((x) => SelectedRestriction != null, (x) =>
            {
                SelectedFormat.Restrictions.Remove((Restriction)x);

                if (SelectedFormat.Restrictions.Count > 0)
                {
                    SelectedRestriction = SelectedFormat.Restrictions.First();
                }
                else
                {
                    SelectedRestriction = null;
                }
            });
            AddRestriction = new RelayCommand((x) => SelectedFormat != null, (x) =>
            {
                SelectedFormat.Restrictions.Add(new Restriction() { Name = "New restriction" });
                SelectedRestriction = SelectedFormat.Restrictions.Last();
            });
        }

        public FormatsViewModel(MainViewModel mainView, ObservableCollection<Set> sets) :this()
        {
            this.mainView = mainView;
            Sets = sets;
        }

        public Format SelectedFormat
        {
            get { return selectedFormat; }
            set
            {
                selectedFormat = value;
                FirePropertyChanged();
            }
        }

        public ObservableCollection<Format> Formats
        {
            get { return formats; }
            set
            {
                formats = value;
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

        public ObservableCollection<Card> AllCards
        {
            get { return allCards; }
            set
            {
                allCards = value;
                FirePropertyChanged();
            }
        }

        private Restriction selectedRestriction;
        private ObservableCollection<Card> allCards;
        private readonly MainViewModel mainView;

        public Restriction SelectedRestriction
        {
            get { return selectedRestriction; }
            set
            {
                selectedRestriction = value;
                FirePropertyChanged();
            }
        }

        public ICommand AddNewCommand { get; set; }
        public ICommand DeleteRestriction { get; set; }
        public ICommand AddRestriction { get; set; }

        internal async Task Save()
        {
            var json = Serializer.SerializeFormatted(Formats.ToList());

            await File.WriteAllTextAsync("Data/formats.json", json);
        }

        internal async Task Load()
        {
            if (!File.Exists("Data/formats.json"))
            {
                return;
            }

            var json = await File.ReadAllTextAsync("Data/formats.json");

            Formats = new ObservableCollection<Format>(Serializer.Deserialize<List<Format>>(json));

            foreach (var format in Formats)
            {
                if (format.Id == null)
                {
                    format.Id = NetworkId.Generate();
                }
            }

            var cards = new ObservableCollection<Card>(mainView.PokemonsViewModel.PokemonCards.Select(x => x.Card));

            foreach (var card in mainView.TrainerCardViewModel.TrainerCards)
            {
                cards.Add(card);
            }

            AllCards = new ObservableCollection<Card>(cards.OrderBy(x => x.SetCode).ThenBy(x => x.Name).ToList());
        }
    }
}
