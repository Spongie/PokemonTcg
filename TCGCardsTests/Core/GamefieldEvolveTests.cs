using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards;
using TCGCards.Core;

namespace TCGCardsTests.Core
{
    [TestClass]
    public class GamefieldEvolveTests
    {
        [TestMethod]
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

            Assert.AreEqual(evolution, player.ActivePokemonCard);

            game.ActivePlayer.PlayEnergyCard(energyCard, evolution, game);

            Assert.AreEqual(1, evolution.AttachedEnergy.Count);
        }
    }
}
