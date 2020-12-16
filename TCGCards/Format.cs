using Entities.Models;
using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TCGCards.Core;

namespace TCGCards
{
    public class Format : DataModel
    {
        private string name;
        private ObservableCollection<Set> sets; 
        private ObservableCollection<string> setCodes;
        private NetworkId id;

        public Format()
        {
            Sets = new ObservableCollection<Set>();
            SetCodes = new ObservableCollection<string>();
            Restrictions = new ObservableCollection<Restriction>();
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged();
            }
        }

        public NetworkId Id
        {
            get { return id; }
            set
            {
                id = value;
                FirePropertyChanged();
            }
        }


        public ObservableCollection<string> SetCodes
        {
            get { return setCodes; }
            set
            {
                setCodes = value;
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

        private ObservableCollection<Restriction> restrictions;

        public ObservableCollection<Restriction> Restrictions
        {
            get { return restrictions; }
            set
            {
                restrictions = value;
                FirePropertyChanged();
            }
        }

        private bool IgnoreCard(Card card)
        {
            var energyCard = card as EnergyCard;

            return energyCard != null && energyCard.IsBasic;
        }

        public DeckValidationResult ValidateDeck(Deck deck)
        {
            var validationResult = new DeckValidationResult
            {
                Result = true,
                Messages = new List<string>()
            };

            var setCodes = new HashSet<string>(Sets.Select(x => x.SetCode));
            var checkedIds = new HashSet<NetworkId>();

            foreach (var cardId in deck.Cards.Where(c => !IgnoreCard(c)).Select(x => x.CardId).Distinct())
            {
                if (checkedIds.Contains(cardId))
                {
                    continue;
                }

                var cards = deck.Cards.Count(x => x.CardId.Equals(cardId));
                var restrictedCard = deck.Cards.FirstOrDefault(card => card.CardId.Equals(cardId));

                if (cards > 4)
                {
                    validationResult.Messages.Add($"You are only allowed up to 4 of {restrictedCard?.Name} but you have {cards}");
                    validationResult.Result = false;
                }
                else if (setCodes.Count > 0 && !setCodes.Contains(restrictedCard.SetCode))
                {
                    validationResult.Messages.Add($"{restrictedCard.Name} from {restrictedCard.SetCode} is not available in this format");
                    validationResult.Result = false;
                }

                checkedIds.Add(cardId);
            }

            foreach (var restriction in Restrictions)
            {
                int cards = deck.Cards.Count(card => card.CardId.Equals(restriction.RestrictedId));
                var restrictedCard = deck.Cards.FirstOrDefault(card => card.CardId.Equals(restriction.RestrictedId));

                if (cards > restriction.MaxCount)
                {
                    validationResult.Messages.Add($"You are only allowed up to {restriction.MaxCount} of {restrictedCard?.Name} but you have {cards}");
                    validationResult.Result = false;
                }
            }

            if (deck.Cards.OfType<PokemonCard>().Count(pokemon => pokemon.Stage == 0) == 0)
            {
                validationResult.Messages.Add("You must have atleast 1 basic Pokémon in your deck");
                validationResult.Result = false;
            }

            if (deck.Cards.Count != 60)
            {
                validationResult.Messages.Add("Your deck must contain 60 cards");
                validationResult.Result = false;
            }

            return validationResult;
        }
    }
}
