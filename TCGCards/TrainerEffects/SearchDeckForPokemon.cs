using CardEditor.Views;
using Entities;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class SearchDeckForPokemon : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Search your deck for Pokémon";
            }
        }

        private bool addToBench;
        private EnergyTypes energyType = EnergyTypes.None;
        private string names;
        private int amount;
        private bool coinFlip;
        private bool revealCard;

        [DynamicInput("Coin Flip", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Names to search for (Split with ;)")]
        public string Names
        {
            get { return names; }
            set
            {
                names = value;
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

        [DynamicInput("Only of type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Add to bench", InputControl.Boolean)]
        public bool AddToBench
        {
            get { return addToBench; }
            set
            {
                addToBench = value;
                FirePropertyChanged();
            }
        }


        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            if (AddToBench)
            {
                return GameField.BenchMaxSize - caster.BenchedPokemon.Count - amount >= 0;
            }

            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            foreach (var card in DeckSearchUtil.SearchDeck(game, caster, new List<IDeckFilter> { new PokemonWithNameOrTypeFilter(Names, EnergyType) { OnlyBasic = addToBench } }, Amount))
            {
                caster.Deck.Cards = new Stack<Card>(caster.Deck.Cards.Except(new[] { card }));
                
                if (revealCard)
                {
                    card.IsRevealed = true;
                }

                if (addToBench)
                {
                    caster.BenchedPokemon.Add((PokemonCard)card);
                    game.SendEventToPlayers(new PokemonAddedToBenchEvent() { Player = caster.Id, Pokemon = (PokemonCard)card });
                }
                else
                {
                    caster.Hand.Add(card);
                    game.SendEventToPlayers(new DrawCardsEvent() { Amount = 1, Player = caster.Id, Cards = new List<Card>() { card } });
                }
            }
        }
    }
}
