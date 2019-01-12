using NetworkingCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core.Messages;

namespace TCGCards.Core
{
    public class Player
    {
        private readonly int MaxBenchedPokemons = 5;
        private bool endedTurn = false;

        public Player()
        {
            Hand = new List<Card>();
            BenchedPokemon = new List<PokemonCard>();
            PrizeCards = new List<Card>();
            Deck = new Deck();
            DiscardPile = new List<Card>();
        }

        public Player(NetworkPlayer networkPlayer) :this()
        {
            SetNetworkPlayer(networkPlayer);
        }

        public void SetPrizeCards(int priceCards)
        {
            for(int _ = 0; _ < priceCards; _++)
            {
                PrizeCards.Add(Deck.DrawCard());
            }
        }

        public void DiscardCards(IEnumerable<Card> cards)
        {
            DiscardPile.AddRange(cards);
            foreach (var card in cards)
            {
                Hand.Remove(card);
            }
        }

        public void DiscardCard(Card card)
        {
            DiscardPile.Add(card);
            Hand.Remove(card);
        }

        public void DrawCardsFromDeck(IEnumerable<Card> selectedCards)
        {
            Deck.Cards = new Stack<Card>(Deck.Cards.Except(selectedCards));
            Hand.AddRange(selectedCards);
            Deck.Shuffle();
        }

        public void SetBenchedPokemon(PokemonCard pokemon)
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

        public void ForceRetreatActivePokemon(PokemonCard replacementPokemon)
        {
            var oldActivePokemon = ActivePokemonCard;
            ActivePokemonCard = replacementPokemon;
            BenchedPokemon.Remove(replacementPokemon);
            BenchedPokemon.Add(oldActivePokemon);
            oldActivePokemon.ClearStatusEffects();
        }

        public void RetreatActivePokemon(PokemonCard replacementPokemon, IEnumerable<EnergyCard> payedEnergy)
        {
            if(!ActivePokemonCard.CanReatreat())
                return;

            foreach (var energyCard in payedEnergy)
            {
                ActivePokemonCard.AttachedEnergy.Remove(energyCard);
            }

            var oldActivePokemon = ActivePokemonCard;
            ActivePokemonCard = replacementPokemon;
            BenchedPokemon.Remove(replacementPokemon);
            BenchedPokemon.Add(oldActivePokemon);
            oldActivePokemon.ClearStatusEffects();
        }

        public void DrawPrizeCard(Card prizeCard)
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

        public void PlayCard(PokemonCard card)
        {
            if(ActivePokemonCard == null)
                ActivePokemonCard = card;
            else
                BenchedPokemon.Add(card);
        }
        
        public void SetActivePokemon(PokemonCard pokemon)
        {
            if (ActivePokemonCard != null)
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

        public void AttachEnergyToPokemon(EnergyCard energyCard, PokemonCard targetPokemonCard)
        {
            if(HasPlayedEnergy)
                return;

            HasPlayedEnergy = true;
            targetPokemonCard.AttachedEnergy.Add(energyCard);

            energyCard.OnAttached(targetPokemonCard, Hand.Contains(energyCard));
            Hand.Remove(energyCard);
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

        public void SetNetworkPlayer(NetworkPlayer networkPlayer)
        {
            Id = networkPlayer.Id;
            NetworkPlayer = networkPlayer;
        }

        public List<PokemonCard> BenchedPokemon { get; set; }
        public PokemonCard ActivePokemonCard { get; set; }
        public List<Card> PrizeCards { get; set; }
        public List<Card> DiscardPile { get; set; }
        public Deck Deck { get; set; }
        public List<Card> Hand { get; set; }
        public bool HasPlayedEnergy { get; protected set; }
        public NetworkId Id { get; set; }
        [JsonIgnore]
        public NetworkPlayer NetworkPlayer { get; private set; }

        public bool IsRegistered()
        {
            return Id != null && Deck != null;
        }
    }
}
