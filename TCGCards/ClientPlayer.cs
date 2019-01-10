using System.Collections.Generic;
using NetworkingCore;
using TCGCards.Core;

namespace TCGCards
{
    public class ClientPlayer
    {
        public ClientPlayer(Player player)
        {
            Id = player.Id;
            Hand = player.Hand;
            BenchedPokemon = player.BenchedPokemon;
            PrizeCards = player.PrizeCards;
            DiscardPile = player.DiscardPile;
            Deck = player.Deck;
            HasPlayedEnergy = player.HasPlayedEnergy;
        }

        public NetworkId Id { get; set; }
        public List<Card> Hand { get; set; }
        public List<PokemonCard> BenchedPokemon { get; set; }
        public PokemonCard ActivePokemonCard { get; set; }
        public List<Card> PrizeCards { get; set; }
        public List<Card> DiscardPile { get; set; }
        public Deck Deck { get; set; }
        public bool HasPlayedEnergy { get; protected set; }
    }
}