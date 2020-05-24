using NetworkingCore;
using NetworkingCore.Messages;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;

namespace TeamRocket.TrainerCards
{
    public class Challenge : TrainerCard, IDeckSearcher
    {
        private int pokemonOnBench;

        public Challenge()
        {
            Name = "Challenge!";
            Description = "Ask your opponent if he or she accepts your challenge. If your opponent declines (or if both Benches are full), draw 2 cards. If your opponent accepts, each of you searches your decks for any number of Basic Pokémon cards and puts them face down onto your Benches. (A player can't do this if his or her Bench is full.) When you both have finished, shuffle your decks and turn those cards face up.";
        }

        public List<IDeckFilter> GetDeckFilters()
        {
            return new List<IDeckFilter>
            {
                new BasicPokemonFilter()
            };
        }

        public int GetNumberOfCards()
        {
            return GameField.BenchMaxSize - pokemonOnBench;
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            if (caster.BenchedPokemon.Count == GameField.BenchMaxSize && opponent.BenchedPokemon.Count == GameField.BenchMaxSize)
            {
                caster.DrawCards(2);
                return;
            }

            var response = opponent.NetworkPlayer.SendAndWaitForResponse<YesNoMessage>(new YesNoMessage().ToNetworkMessage(opponent.Id));

            if (!response.AnsweredYes)
            {
                caster.DrawCards(2);
                return;
            }

            var casterPokemon = new List<NetworkId>();
            var opponentPokemon = new List<NetworkId>();

            if (caster.BenchedPokemon.Count < GameField.BenchMaxSize)
            {
                pokemonOnBench = caster.BenchedPokemon.Count;
                casterPokemon = this.TriggerDeckSearch(caster);
            }
            if (opponent.BenchedPokemon.Count < GameField.BenchMaxSize)
            {
                pokemonOnBench = opponent.BenchedPokemon.Count;
                opponentPokemon = this.TriggerDeckSearch(opponent);
            }

            caster.BenchedPokemon.AddRange(casterPokemon.Select(id => (PokemonCard)game.FindCardById(id)));
            opponent.BenchedPokemon.AddRange(opponentPokemon.Select(id => (PokemonCard)game.FindCardById(id)));
        }
    }
}
