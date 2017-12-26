using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using TCGCards.EnergyCards;
using TCGCards.PokemonCards.TeamRocket;

namespace TCGCardsTests.Core
{
    [TestClass]
    public class IPokemonCardTests
    {
        [TestMethod]
        public void CanRetreat_NoBench()
        {
            var game = new GameField();
            game.InitTest();
            var pokmeon = new Magikarp(game.ActivePlayer);

            Assert.IsFalse(pokmeon.CanReatreat());
        }

        [TestMethod]
        public void CanRetreat_Bench_NoEnergy()
        {
            var game = new GameField();
            game.InitTest();
            var pokmeon = new Magikarp(game.ActivePlayer);

            game.ActivePlayer.SetActivePokemon(pokmeon);
            game.ActivePlayer.SetBenchedPokemon(new Magikarp());

            Assert.IsFalse(pokmeon.CanReatreat());
        }

        [TestMethod]
        public void CanRetreat_Bench_Energy()
        {
            var game = new GameField();
            game.InitTest();
            var pokmeon = new Magikarp(game.ActivePlayer);

            game.ActivePlayer.SetActivePokemon(pokmeon);
            game.ActivePlayer.SetBenchedPokemon(new Magikarp(game.ActivePlayer));

            game.ActivePlayer.AttachEnergyToPokemon(new WaterEnergy(), game.ActivePlayer.ActivePokemonCard);

            Assert.IsTrue(pokmeon.CanReatreat());
        }

        [TestMethod]
        public void CanRetreat_NoBench_Energy()
        {            
            var game = new GameField();
            game.InitTest();
            var pokmeon = new Magikarp(game.ActivePlayer);

            game.ActivePlayer.SetActivePokemon(pokmeon);
            game.ActivePlayer.AttachEnergyToPokemon(new WaterEnergy(), game.ActivePlayer.ActivePokemonCard);

            Assert.IsFalse(pokmeon.CanReatreat());
        }
    }
}
