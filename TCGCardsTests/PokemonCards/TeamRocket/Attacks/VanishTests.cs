using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.PokemonCards.TeamRocket.Attacks.Tests
{
    [TestClass()]
    public class VanishTests
    {
        [TestMethod]
        public void ProcessEffectsTest()
        {
            var attack = new Vanish();

            var game = new GameField();
            game.Players.Add(new Player() { Id = Guid.NewGuid() });
            game.Players.Add(new Player() { Id = Guid.NewGuid() });

            game.ActivePlayer = game.Players.First();

            game.ActivePlayer.ActivePokemonCard = new Abra(game.ActivePlayer);
            game.NonActivePlayer.ActivePokemonCard = new Abra(game.NonActivePlayer);

            attack.ProcessEffects(game, game.ActivePlayer, game.NonActivePlayer);

            Assert.AreEqual(game.GameState, GameFieldState.ActivePlayerSelectingFromBench);
        }
    }
}