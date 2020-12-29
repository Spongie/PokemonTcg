using CardEditor.Views;
using Entities;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.GameEvents;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class SearchDeckForCardEffect : DataModel, IEffect
    {
        private CardType cardType;
        private EnergyTypes energyType = EnergyTypes.None;
        private bool revealCard;
        private bool useLastDiscardCount;
        private SearchDeckResult result = SearchDeckResult.PutInHand; 
        private TargetingMode targetingMode;

        [DynamicInput("Card type", InputControl.Dropdown, typeof(CardType))]
        public CardType CardType
        {
            get { return cardType; }
            set
            {
                cardType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Energy type (when basic energy)", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Reaveal searched card?", InputControl.Boolean)]
        public bool RevealCard
        {
            get { return revealCard; }
            set
            {
                revealCard = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Search for last discard amount", InputControl.Boolean)]
        public bool UseLastDiscardCount
        {
            get { return useLastDiscardCount; }
            set
            {
                useLastDiscardCount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Result", InputControl.Dropdown, typeof(SearchDeckResult))]
        public SearchDeckResult ResultType
        {
            get { return result; }
            set
            {
                result = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }


        public string EffectType
        {
            get
            {
                return "Search your deck for a card";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            switch (result)
            {
                case SearchDeckResult.PutOnBench:
                    return caster.BenchedPokemon.Count < GameField.BenchMaxSize;
                default:
                    break;
            }

            return caster.Deck.Cards.Count > 0;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var filter = CardUtil.GetCardFilters(CardType, EnergyType).ToList();
            int amount = useLastDiscardCount ? game.LastDiscard : 1;

            foreach (var card in DeckSearchUtil.SearchDeck(game, caster, filter, amount))
            {
                if (revealCard)
                {
                    card.RevealToAll();
                }

                switch (result)
                {
                    case SearchDeckResult.PutInHand:
                        caster.DrawCardsFromDeck(new List<Card> { card });
                        break;
                    case SearchDeckResult.PutInDiscard:
                        {
                            caster.Deck.Cards.Except(new[] { card });
                            caster.DiscardPile.Add(card);
                            caster.TriggerDiscardEvent(new List<Card>() { card });
                        }
                        break;
                    case SearchDeckResult.PutOnBench:
                        {
                            caster.Deck.Cards.Except(new[] { card });
                            int benchIndex = caster.BenchedPokemon.GetNextFreeIndex();
                            caster.BenchedPokemon.Add((PokemonCard)card);
                            game.SendEventToPlayers(new PokemonAddedToBenchEvent()
                            {
                                Player = caster.Id,
                                Pokemon = (PokemonCard)card,
                                Index = benchIndex
                            });
                        }
                        break;
                    case SearchDeckResult.AttachToTarget:
                        {
                            caster.Deck.Cards.Except(new[] { card });
                            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource);
                            target.AttachEnergy((EnergyCard)card, game);
                        }
                        break;
                    default:
                        break;
                }
            }

            caster.Deck.Shuffle();
        }
    }
}
