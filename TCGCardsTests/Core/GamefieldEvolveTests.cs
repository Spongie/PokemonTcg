using Entities;
using Xunit;
using TCGCards;
using TCGCards.Core;

namespace TCGCardsTests.Core
{
    public class GamefieldEvolveTests
    {
        [Fact]
        public void EvolveAndAttachEnergy()
        {
            var game = new GameField();
            game.InitTest();
            game.GameState = GameFieldState.InTurn;

            var player = game.ActivePlayer;
            player.TurnsTaken = 32;
            var pokemon = new PokemonCard(player) { Hp = 100, PokemonName = "Basic", Stage = 0, PlayedThisTurn = false };
            var evolution = new PokemonCard(player) { Hp = 100, EvolvesFrom = "Basic", Name = "Evolver", Stage = 1 };
            var energyCard = new EnergyCard() { Amount = 1, EnergyType = EnergyTypes.Colorless };
            player.ActivePokemonCard = pokemon;

            player.Hand.Add(evolution);
            player.Hand.Add(energyCard);

            game.EvolvePokemon(pokemon, evolution);

            Assert.Equal(evolution, player.ActivePokemonCard);

            game.PlayEnergyCard(energyCard, evolution);

            Assert.Single(evolution.AttachedEnergy);
        }
    }
}
