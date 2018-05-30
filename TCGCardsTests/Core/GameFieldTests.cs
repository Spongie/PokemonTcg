using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TCGCards.EnergyCards;
using TeamRocket.PokemonCards;

namespace TCGCards.Core.Tests
{
    [TestClass()]
    public class GameFieldTests
    {
        [TestMethod]
        public void AttackTest_NoDeadPokemon()
        {
            var gameField = new GameField();
            gameField.InitTest();
            var originalActive = gameField.ActivePlayer;

            gameField.NonActivePlayer.Hand.Add(new WaterEnergy());
            gameField.ActivePlayer.Hand.Add(new WaterEnergy());

            var activePokemon = new Magikarp(gameField.ActivePlayer);
            var otherPokemon = new Magikarp(gameField.NonActivePlayer);

            gameField.ActivePlayer.SetActivePokemon(activePokemon);
            gameField.NonActivePlayer.SetActivePokemon(otherPokemon);

            gameField.Attack(activePokemon.Attacks.First());

            Assert.AreNotEqual(originalActive.Id, gameField.ActivePlayer.Id);
            Assert.AreEqual(GameFieldState.InTurn, gameField.GameState);
        }

        [TestMethod]
        public void AttackTest_DeadOpponentPokemon()
        {
            var gameField = new GameField();
            gameField.InitTest();
            var originalActive = gameField.ActivePlayer;

            gameField.NonActivePlayer.Hand.Add(new WaterEnergy());
            gameField.ActivePlayer.Hand.Add(new WaterEnergy());

            var activePokemon = new Magikarp(gameField.ActivePlayer);
            var otherPokemon = new Magikarp(gameField.NonActivePlayer);
            otherPokemon.DamageCounters += otherPokemon.Hp - 1;

            gameField.ActivePlayer.SetActivePokemon(activePokemon);
            gameField.NonActivePlayer.SetActivePokemon(otherPokemon);

            gameField.Attack(activePokemon.Attacks.First());

            Assert.AreEqual(originalActive.Id, gameField.ActivePlayer.Id);
            Assert.AreEqual(GameFieldState.ActivePlayerSelectingPrize, gameField.GameState);
        }

        [TestMethod]
        public void AttackTest_DeadActivePokemon()
        {
            var gameField = new GameField();
            gameField.InitTest();
            var originalActive = gameField.ActivePlayer;

            gameField.NonActivePlayer.Hand.Add(new WaterEnergy());
            gameField.ActivePlayer.Hand.Add(new WaterEnergy());

            var activePokemon = new Magikarp(gameField.ActivePlayer);
            var otherPokemon = new Magikarp(gameField.NonActivePlayer);

            activePokemon.DamageCounters = activePokemon.Hp;

            gameField.ActivePlayer.SetActivePokemon(activePokemon);
            gameField.NonActivePlayer.SetActivePokemon(otherPokemon);

            gameField.Attack(activePokemon.Attacks.First());

            Assert.AreEqual(originalActive.Id, gameField.ActivePlayer.Id);
            Assert.AreEqual(GameFieldState.UnActivePlayerSelectingPrize, gameField.GameState);
        }

        [TestMethod]
        public void SelectPrizeCard_ActivePlayer_HisNotDead()
        {
            var gameField = new GameField();
            gameField.InitTest();
            var originalActive = gameField.ActivePlayer;

            gameField.NonActivePlayer.Hand.Add(new WaterEnergy());
            gameField.ActivePlayer.Hand.Add(new WaterEnergy());

            var activePokemon = new Magikarp(gameField.ActivePlayer);
            var otherPokemon = new Magikarp(gameField.NonActivePlayer);

            gameField.ActivePlayer.SetActivePokemon(activePokemon);
            gameField.NonActivePlayer.SetActivePokemon(otherPokemon);

            gameField.GameState = GameFieldState.ActivePlayerSelectingPrize;
            originalActive.PrizeCards.Add(new WaterEnergy());
            gameField.SelectPrizeCard(originalActive.PrizeCards.First());

            Assert.AreNotEqual(originalActive.Id, gameField.ActivePlayer.Id);
            Assert.AreEqual(GameFieldState.InTurn, gameField.GameState);
        }

        [TestMethod]
        public void SelectPrizeCard_ActivePlayer_HisDead()
        {
            var gameField = new GameField();
            gameField.InitTest();
            var originalActive = gameField.ActivePlayer;

            gameField.NonActivePlayer.Hand.Add(new WaterEnergy());
            gameField.ActivePlayer.Hand.Add(new WaterEnergy());

            var activePokemon = new Magikarp(gameField.ActivePlayer);
            var otherPokemon = new Magikarp(gameField.NonActivePlayer);

            gameField.ActivePlayer.SetActivePokemon(activePokemon);
            gameField.NonActivePlayer.SetActivePokemon(otherPokemon);

            activePokemon.DamageCounters = 100000;
            originalActive.PrizeCards.Add(new WaterEnergy());

            gameField.Attack(activePokemon.Attacks.First());

            Assert.AreEqual(GameFieldState.UnActivePlayerSelectingPrize, gameField.GameState);

            gameField.SelectPrizeCard(originalActive.PrizeCards.First());

            Assert.AreEqual(originalActive.Id, gameField.ActivePlayer.Id);
            Assert.AreEqual(GameFieldState.ActivePlayerSelectingFromBench, gameField.GameState);
        }

        [TestMethod]
        public void SelectPrizeCard_NonActivePlayer_ActiveDead()
        {
            var gameField = new GameField();
            gameField.InitTest();
            var originalActive = gameField.ActivePlayer;

            gameField.NonActivePlayer.Hand.Add(new WaterEnergy());
            gameField.ActivePlayer.Hand.Add(new WaterEnergy());

            var activePokemon = new Magikarp(gameField.ActivePlayer);
            var otherPokemon = new Magikarp(gameField.NonActivePlayer);

            gameField.ActivePlayer.SetActivePokemon(activePokemon);
            gameField.NonActivePlayer.SetActivePokemon(otherPokemon);

            activePokemon.DamageCounters = 100000;

            originalActive.PrizeCards.Add(new WaterEnergy());
            gameField.NonActivePlayer.PrizeCards.Add(new WaterEnergy());

            gameField.Attack(activePokemon.Attacks.First());
            Assert.AreEqual(GameFieldState.UnActivePlayerSelectingPrize, gameField.GameState);

            gameField.SelectPrizeCard(originalActive.PrizeCards.First());

            Assert.AreEqual(originalActive.Id, gameField.ActivePlayer.Id);
            Assert.AreEqual(GameFieldState.ActivePlayerSelectingFromBench, gameField.GameState);
        }

        [TestMethod]
        public void PokemonDiesBurn()
        {
            var gameField = new GameField();
            gameField.InitTest();
            var originalActive = gameField.ActivePlayer;

            gameField.NonActivePlayer.Hand.Add(new WaterEnergy());
            gameField.ActivePlayer.Hand.Add(new WaterEnergy());

            var activePokemon = new Magikarp(gameField.ActivePlayer);
            var otherPokemon = new Magikarp(gameField.NonActivePlayer);

            gameField.ActivePlayer.SetActivePokemon(activePokemon);
            gameField.NonActivePlayer.SetActivePokemon(otherPokemon);

            activePokemon.DamageCounters = activePokemon.Hp - 1;
            activePokemon.IsBurned = true;

            gameField.EndTurn();

            Assert.AreEqual(GameFieldState.UnActivePlayerSelectingPrize, gameField.GameState);
            Assert.AreEqual(originalActive.Id, gameField.ActivePlayer.Id);
        }

        [TestMethod]
        public void EvolvePokemon()
        {
            var gameField = new GameField();
            gameField.InitTest();
            

            gameField.NonActivePlayer.Hand.Add(new WaterEnergy());
            gameField.ActivePlayer.Hand.Add(new WaterEnergy());

            IPokemonCard activePokemon = new Magikarp(gameField.ActivePlayer);
            IPokemonCard otherPokemon = new Magikarp(gameField.NonActivePlayer);

            var originalActive = activePokemon;
            var activeEvolution = new DarkGyarados(gameField.ActivePlayer);

            IPokemonCard evolution = activePokemon.Evolve(activeEvolution);
            activePokemon = evolution;

            Assert.AreEqual(activePokemon, activeEvolution);
            Assert.AreEqual(originalActive.PokemonName, activeEvolution.EvolvesFrom);
        }

        [TestMethod]
        public void KnockedOutBy_IsSet()
        {
            var gameField = new GameField();
            gameField.InitTest();

            gameField.NonActivePlayer.Hand.Add(new WaterEnergy());
            gameField.ActivePlayer.Hand.Add(new WaterEnergy());

            IPokemonCard activePokemon = new Magikarp(gameField.ActivePlayer);
            IPokemonCard otherPokemon = new Magikarp(gameField.NonActivePlayer);

            gameField.ActivePlayer.SetActivePokemon(activePokemon);
            gameField.NonActivePlayer.SetActivePokemon(otherPokemon);

            otherPokemon.DamageCounters = otherPokemon.Hp - 1;
            gameField.Attack(activePokemon.Attacks.First());

            Assert.AreEqual(activePokemon, otherPokemon.KnockedOutBy);
        }
    }
}