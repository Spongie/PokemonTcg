using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using TCGCards.EnergyCards;
using TeamRocket.PokemonCards;

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

        [TestMethod]
        public void CanRetreat_Paralyzed()
        {
            var game = new GameField();
            game.InitTest();
            var pokmeon = new Magikarp(game.ActivePlayer);

            pokmeon.IsParalyzed = true;

            Assert.IsFalse(pokmeon.CanReatreat());
        }

        [TestMethod]
        public void CanAttack_Paralyzed()
        {
            var pokemon = new Magikarp();
            pokemon.IsParalyzed = true;

            Assert.IsFalse(pokemon.CanAttack());
        }

        [TestMethod]
        public void CanAttack_NotParalyzed()
        {
            var pokemon = new Magikarp();

            Assert.IsTrue(pokemon.CanAttack());
        }

        [TestMethod]
        public void EndOfTurn_PlayedThisTurn()
        {
            var pokemon = new Magikarp();
            pokemon.PlayedThisTurn = true;

            pokemon.EndTurn();

            Assert.IsFalse(pokemon.PlayedThisTurn);
        }

        [TestMethod]
        public void BurnEndOfTurn()
        {
            var pokemon = new Magikarp();
            pokemon.IsBurned = true;

            pokemon.EndTurn();

            Assert.AreEqual(20, pokemon.DamageCounters);
        }

        [TestMethod]
        public void ClearStatusEffects()
        {
            var pokemon = new Magikarp();
            pokemon.IsBurned = true;
            pokemon.IsParalyzed = true;
            pokemon.IsPoisoned = true;
            pokemon.IsAsleep = true;
            pokemon.IsConfused = true;

            pokemon.ClearStatusEffects();

            Assert.IsFalse(pokemon.IsParalyzed);
            Assert.IsFalse(pokemon.IsBurned);
            Assert.IsFalse(pokemon.IsPoisoned);
            Assert.IsFalse(pokemon.IsAsleep);
            Assert.IsFalse(pokemon.IsConfused);
        }

        [TestMethod]
        public void Poision_EndofTurn()
        {
            var pokemon = new Magikarp();
            pokemon.IsPoisoned = true;
            pokemon.EndTurn();

            Assert.AreEqual(10, pokemon.DamageCounters);
        }

        [TestMethod]
        public void CanAttack_Asleep()
        {
            var pokemon = new Magikarp();
            pokemon.IsAsleep = true;

            Assert.IsFalse(pokemon.CanAttack());
        }

        [TestMethod]
        public void Evolve_StatusClear()
        {
            var pokemon = new Magikarp();
            pokemon.IsBurned = true;

            pokemon.Evolve(pokemon);

            Assert.IsFalse(pokemon.IsBurned);
        }

        [TestMethod]
        public void Evolve_SetBase()
        {
            var pokemon = new Magikarp();
            pokemon.DamageCounters = 10;

            var evolution = new DarkGyarados();
            evolution.SetBase(pokemon);

            Assert.AreEqual(10, evolution.DamageCounters);
        }

        [TestMethod]
        public void CanEvolve_Valid()
        {
            var magikarp = new Magikarp();

            var gyarados = new DarkGyarados();

            Assert.IsTrue(magikarp.CanEvolveTo(gyarados));
        }

        [TestMethod]
        public void CanEvolve_InValid()
        {
            var magikarp = new Magikarp();

            var gyarados = new Magikarp();

            Assert.IsFalse(magikarp.CanEvolveTo(gyarados));
        }
    }
}
