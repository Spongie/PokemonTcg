using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core
{
    public class Player
    {
        private readonly int MaxBenchedPokemons = 5;
        private bool endedTurn = false;

        public Player()
        {
            Hand = new List<ICard>();
            BenchedPokemon = new List<IPokemonCard>();
            PrizeCards = new List<ICard>();
            Deck = new Deck();
        }

        internal void SetPrizeCards(int priceCards)
        {
            for(int _ = 0; _ < priceCards; _++)
            {
                PrizeCards.Add(Deck.DrawCard());
            }
        }

        public void DrawCardsFromDeck(IEnumerable<ICard> selectedCards)
        {
            Deck.Cards = new Stack<ICard>(Deck.Cards.Except(selectedCards));
            Hand.AddRange(selectedCards);
            Deck.Shuffle();
        }

        public void SetBenchedPokemon(IPokemonCard pokemon)
        {
            if(BenchedPokemon.Count < MaxBenchedPokemons && pokemon.Stage == 0)
            {
                if(Hand.Contains(pokemon))
                {
                    pokemon.PlayedThisTurn = true;
                    Hand.Remove(pokemon);
                }

                BenchedPokemon.Add(pokemon);
            }
        }

        public void ForceRetreatActivePokemon(IPokemonCard replacementPokemon)
        {
            var oldActivePokemon = ActivePokemonCard;
            ActivePokemonCard = replacementPokemon;
            BenchedPokemon.Remove(replacementPokemon);
            BenchedPokemon.Add(oldActivePokemon);
            oldActivePokemon.ClearStatusEffects();
        }

        public void RetreatActivePokemon(IPokemonCard replacementPokemon)
        {
            if(!ActivePokemonCard.CanReatreat())
                return;

            var oldActivePokemon = ActivePokemonCard;
            ActivePokemonCard = replacementPokemon;
            BenchedPokemon.Remove(replacementPokemon);
            BenchedPokemon.Add(oldActivePokemon);
            oldActivePokemon.ClearStatusEffects();
        }

        public void DrawPrizeCard(ICard prizeCard)
        {
            Hand.Add(prizeCard);
            PrizeCards.Remove(prizeCard);
        }

        public void EndTurn()
        {
            if(endedTurn)
                return;

            endedTurn = true;
            HasPlayedEnergy = false;
            ActivePokemonCard.EndTurn();

            foreach(var pokemon in BenchedPokemon)
            {
                pokemon.EndTurn();
            }
        }

        public void PlayCard(IPokemonCard card)
        {
            if(ActivePokemonCard == null)
                ActivePokemonCard = card;
            else
                BenchedPokemon.Add(card);
        }
        
        public void SetActivePokemon(IPokemonCard pokemon)
        {
            if(ActivePokemonCard != null)
                return;

            if(Hand.Contains(pokemon))
            {
                pokemon.PlayedThisTurn = true;
                Hand.Remove(pokemon);
            }
            else if(BenchedPokemon.Contains(pokemon))
                BenchedPokemon.Remove(pokemon);

            ActivePokemonCard = pokemon;
        }

        public void AttachEnergyToPokemon(IEnergyCard energyCard, IPokemonCard targetPokemonCard)
        {
            if(HasPlayedEnergy)
                return;

            HasPlayedEnergy = true;
            targetPokemonCard.AttachedEnergy.Add(energyCard);
        }

        internal void ResetTurn()
        {
            endedTurn = false;
        }

        public void SetDeck(Deck deck)
        {
            Deck = deck;

            deck.Shuffle();
        }

        public void DrawCards(int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                if (Deck.Cards.Count > 0)
                    Hand.Add(Deck.DrawCard());
            }
        }

        public List<IPokemonCard> BenchedPokemon { get; set; }

        public IPokemonCard ActivePokemonCard { get; set; }

        public List<ICard> PrizeCards { get; set; }

        public List<ICard> DiscardPile { get; set; }

        public Deck Deck { get; set; }

        public void SetNetworkPlayer(NetworkPlayer networkPlayer)
        {
            Id = networkPlayer.Id;
            NetworkPlayer = networkPlayer;
        }

        public List<ICard> Hand { get; set; }
        public Guid Id { get; set; }
        public NetworkPlayer NetworkPlayer { get; private set; }

        public bool HasPlayedEnergy { get; protected set; }

        public bool IsRegistered()
        {
            return Id != null && Deck != null;
        }
    }
}
