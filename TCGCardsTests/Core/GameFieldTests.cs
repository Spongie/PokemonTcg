using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TCGCards.EnergyCards;
using TeamRocket.PokemonCards;
using TeamRocket.TrainerCards;

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

            PokemonCard activePokemon = new Magikarp(gameField.ActivePlayer);
            PokemonCard otherPokemon = new Magikarp(gameField.NonActivePlayer);

            var originalActive = activePokemon;
            var activeEvolution = new DarkGyarados(gameField.ActivePlayer);

            PokemonCard evolution = activePokemon.Evolve(activeEvolution);
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

            PokemonCard activePokemon = new Magikarp(gameField.ActivePlayer);
            PokemonCard otherPokemon = new Magikarp(gameField.NonActivePlayer);

            gameField.ActivePlayer.SetActivePokemon(activePokemon);
            gameField.NonActivePlayer.SetActivePokemon(otherPokemon);

            otherPokemon.DamageCounters = otherPokemon.Hp - 1;
            gameField.Attack(activePokemon.Attacks.First());

            Assert.AreEqual(activePokemon, otherPokemon.KnockedOutBy);
        }

        [TestMethod]
        public void GameState_Start()
        {
            var gameField = new GameField();
            gameField.InitTest();
            gameField.StartGame();

            Assert.AreEqual(GameFieldState.BothSelectingActive, gameField.GameState);
        }

        [TestMethod]
        public void GameState_Start_BothActivate_Selected()
        {
            var gameField = new GameField();
            gameField.InitTest();
            gameField.StartGame();

            gameField.OnActivePokemonSelected(gameField.ActivePlayer.Id, new PokemonCard(gameField.ActivePlayer));
            
            Assert.AreEqual(GameFieldState.BothSelectingActive, gameField.GameState);
            gameField.OnActivePokemonSelected(gameField.NonActivePlayer.Id, new PokemonCard(gameField.NonActivePlayer));

            Assert.AreEqual(GameFieldState.BothSelectingBench, gameField.GameState);
        }

        [TestMethod]
        public void GameState_Start_BothBench_Selected()
        {
            var gameField = new GameField();
            gameField.InitTest();
            gameField.StartGame();

            FillPlayerDecksWithJunk(gameField);

            gameField.OnActivePokemonSelected(gameField.ActivePlayer.Id, new PokemonCard(gameField.ActivePlayer));
            gameField.OnActivePokemonSelected(gameField.NonActivePlayer.Id, new PokemonCard(gameField.NonActivePlayer));

            gameField.OnBenchPokemonSelected(gameField.ActivePlayer, new[] { new PokemonCard(gameField.ActivePlayer) }.ToList());

            Assert.AreEqual(GameFieldState.BothSelectingBench, gameField.GameState);
            gameField.OnBenchPokemonSelected(gameField.NonActivePlayer, new[] { new PokemonCard(gameField.NonActivePlayer) }.ToList());

            Assert.AreEqual(GameFieldState.InTurn, gameField.GameState);
        }

        [TestMethod]
        public void TrainerCard_Player_DiscardPile()
        {
            var gameField = new GameField();
            gameField.InitTest();

            gameField.ActivePlayer.Hand.Add(new HereComesTeamRocket());
            gameField.ActivePlayer.Hand.Add(new HereComesTeamRocket());
            gameField.ActivePlayer.Hand.Add(new HereComesTeamRocket());
            gameField.ActivePlayer.Hand.Add(new HereComesTeamRocket());
            gameField.ActivePlayer.Hand.Add(new HereComesTeamRocket());

            TrainerCard trainerCard = gameField.ActivePlayer.Hand.OfType<TrainerCard>().First();
            gameField.PlayerTrainerCard(trainerCard);

            Assert.AreEqual(trainerCard.Id, gameField.ActivePlayer.DiscardPile.Last().Id);
        }

        public void EvolvePokemon_ActivePokemon()
        {
            var gameField = new GameField();
            gameField.InitTest();
            FillPlayerDecksWithJunk(gameField);

            gameField.GameState = GameFieldState.InTurn;
            gameField.ActivePlayer.ActivePokemonCard = new Ekans(gameField.ActivePlayer);

            var arbok = new DarkArbok(gameField.ActivePlayer);
            gameField.EvolvePokemon(gameField.ActivePlayer.ActivePokemonCard, arbok);

            Assert.AreEqual(gameField.ActivePlayer.ActivePokemonCard.Id, arbok.Id);
            Assert.IsTrue(gameField.ActivePlayer.ActivePokemonCard is DarkArbok);
        }

        public void EvolvePokemon_BenchedPokemon()
        {
            var gameField = new GameField();
            gameField.InitTest();
            FillPlayerDecksWithJunk(gameField);

            gameField.GameState = GameFieldState.InTurn;
            gameField.ActivePlayer.ActivePokemonCard = new Ekans(gameField.ActivePlayer);

            PokemonCard first = new Ekans(gameField.ActivePlayer);
            PokemonCard second = new Ekans(gameField.ActivePlayer);

            gameField.ActivePlayer.BenchedPokemon.Add(first);
            gameField.ActivePlayer.BenchedPokemon.Add(second);

            var arbok = new DarkArbok(gameField.ActivePlayer);
            gameField.EvolvePokemon(first, arbok);

            Assert.AreEqual(first.Id, arbok.Id);
            Assert.IsTrue(first is DarkArbok);

            Assert.AreNotEqual(second.Id, arbok.Id);
            Assert.IsTrue(second is Ekans);
        }

        private void FillPlayerDecksWithJunk(GameField gameField)
        {
            foreach (var player in gameField.Players)
            {
                for (int i = 0; i < 50; i++)
                {
                    player.Deck.Cards.Push(new PokemonCard(player));
                }
            }
        }
    }
}