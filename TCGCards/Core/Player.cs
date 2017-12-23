using System;
using System.Collections.Generic;
using TCGCards.EnergyCards;
using TCGCards.PokemonCards.TeamRocket;

namespace TCGCards.Core
{
    public class Player
    {
        public Player()
        {
            Hand = new List<ICard>();
            BenchedPokemon = new List<IPokemonCard>();
        }

        public void InitTestData()
        {
            Deck = new Deck();
            Deck.Cards.Push(new WaterEnergy());
            Deck.Cards.Push(new WaterEnergy());
            Deck.Cards.Push(new WaterEnergy());
            Deck.Cards.Push(new WaterEnergy());
            Deck.Cards.Push(new Magikarp(this));
            Deck.Cards.Push(new Magikarp(this));
            Deck.Cards.Push(new Magikarp(this));
            Deck.Cards.Push(new Magikarp(this));
            Deck.Shuffle();
            DrawCards(5);
        }

        public void SetBenchedPokemon(IPokemonCard pokemon)
        {
            BenchedPokemon.Add(pokemon);
        }

        public void RetreatActivePokemon(IPokemonCard replacementPokemon)
        {
            if(!ActivePokemonCard.CanReatreat())
                return;

            var oldActivePokemon = ActivePokemonCard;
            ActivePokemonCard = replacementPokemon;
            BenchedPokemon.Remove(replacementPokemon);
            BenchedPokemon.Add(oldActivePokemon);
        }

        public void PlayCard(ICard card)
        {

        }

        public void SetActivePokemon(IPokemonCard pokemon)
        {
            if(Hand.Contains(pokemon))
                Hand.Remove(pokemon);
            else if(BenchedPokemon.Contains(pokemon))
                BenchedPokemon.Remove(pokemon);

            ActivePokemonCard = pokemon;
        }

        public void AttachEnergyToPokemon(IEnergyCard energyCard, IPokemonCard targetPokemonCard)
        {
            targetPokemonCard.AttachedEnergy.Add(energyCard);
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

        public List<ICard> Hand { get; set; }
        public Guid Id { get; set; }

        public bool IsRegistered()
        {
            return Id != null && Deck != null;
        }
    }
}
