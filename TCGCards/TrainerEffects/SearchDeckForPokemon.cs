using CardEditor.Views;
using Entities;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.GameEvents;

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
        private bool evolveSource;

        [DynamicInput("Evolve user", InputControl.Boolean)]
        public bool EvolveSource
        {
            get { return evolveSource; }
            set
            {
                evolveSource = value;
                FirePropertyChanged();
            }
        }


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

            foreach (PokemonCard card in DeckSearchUtil.SearchDeck(game, caster, new List<IDeckFilter> { new PokemonWithNameOrTypeFilter(Names, EnergyType) { OnlyBasic = addToBench } }, Amount))
            {
                caster.Deck.Cards = new Stack<Card>(caster.Deck.Cards.Except(new[] { card }));
                
                if (revealCard)
                {
                    card.RevealToAll();
                }

                if (evolveSource)
                {
                    game.EvolvePokemon(pokemonSource, card, true);
                }
                else if (addToBench)
                {
                    caster.BenchedPokemon.Add(card);
                    game.SendEventToPlayers(new PokemonAddedToBenchEvent() { Player = caster.Id, Pokemon = card, Index = caster.BenchedPokemon.Count - 1 });
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
