using CardEditor.Views;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
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

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            var message = new DeckSearchMessage(caster.Deck, new List<IDeckFilter> { new PokemonWithNameOrTypeFilter(Names, EnergyType) }, Amount);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(caster.Id));


            foreach (var card in response.Cards.Select(id => game.FindCardById(id)))
            {
                caster.Deck.Cards = new Stack<Card>(caster.Deck.Cards.Except(new[] { card }));
                
                if (addToBench)
                {
                    caster.BenchedPokemon.Add((PokemonCard)card);
                }
                else
                {
                    caster.Hand.Add(card);
                }
            }
        }
    }
}
