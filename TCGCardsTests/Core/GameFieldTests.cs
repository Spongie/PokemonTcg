using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core.Messages;
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

            gameField.NonActivePlayer.PrizeCards.Add(new WaterEnergy());
            gameField.NonActivePlayer.PrizeCards.Add(new WaterEnergy());
            gameField.NonActivePlayer.PrizeCards.Add(new WaterEnergy());

            gameField.ActivePlayer.PrizeCards.Add(new WaterEnergy());
            gameField.ActivePlayer.PrizeCards.Add(new WaterEnergy());
            gameField.ActivePlayer.PrizeCards.Add(new WaterEnergy());

            gameField.ActivePlayer.SetActivePokemon(activePokemon);
            gameField.NonActivePlayer.SetActivePokemon(otherPokemon);

            var choice = new Magikarp(gameField.NonActivePlayer);
            gameField.NonActivePlayer.BenchedPokemon.Add(choice);

            var nonActiveNetworkPlayer = Substitute.For<INetworkPlayer>();
            nonActiveNetworkPlayer.Id = gameField.NonActivePlayer.Id;
            nonActiveNetworkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId>
            {
                choice.Id
            }));
            gameField.NonActivePlayer.SetNetworkPlayer(nonActiveNetworkPlayer);

            var activeNetworkPlayer = Substitute.For<INetworkPlayer>();
            activeNetworkPlayer.Id = gameField.ActivePlayer.Id;
            activeNetworkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId>
            {
                gameField.ActivePlayer.PrizeCards.First().Id
            }));
            gameField.ActivePlayer.SetNetworkPlayer(activeNetworkPlayer);

            gameField.Attack(activePokemon.Attacks.First());

            Assert.AreEqual(originalActive.Id, gameField.NonActivePlayer.Id);
            Assert.AreEqual(GameFieldState.InTurn, gameField.GameState);
            Assert.AreEqual(2, gameField.NonActivePlayer.PrizeCards.Count);
            Assert.AreEqual(3, gameField.ActivePlayer.PrizeCards.Count);
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

            gameField.NonActivePlayer.PrizeCards.Add(new WaterEnergy());
            gameField.NonActivePlayer.PrizeCards.Add(new WaterEnergy());
            gameField.NonActivePlayer.PrizeCards.Add(new WaterEnergy());

            gameField.ActivePlayer.PrizeCards.Add(new WaterEnergy());
            gameField.ActivePlayer.PrizeCards.Add(new WaterEnergy());
            gameField.ActivePlayer.PrizeCards.Add(new WaterEnergy());

            var choice = new Magikarp(gameField.ActivePlayer);
            gameField.ActivePlayer.BenchedPokemon.Add(choice);

            var activeNetworkPlayer = Substitute.For<INetworkPlayer>();
            activeNetworkPlayer.Id = gameField.ActivePlayer.Id;
            activeNetworkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId>
            {
                choice.Id
            }));
            gameField.ActivePlayer.SetNetworkPlayer(activeNetworkPlayer);

            var nonActiveNetworkPlayer = Substitute.For<INetworkPlayer>();
            nonActiveNetworkPlayer.Id = gameField.NonActivePlayer.Id;
            nonActiveNetworkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId>
            {
                gameField.NonActivePlayer.PrizeCards.First().Id
            }));
            gameField.NonActivePlayer.SetNetworkPlayer(nonActiveNetworkPlayer);

            gameField.Attack(activePokemon.Attacks.First());

            Assert.AreEqual(originalActive.Id, gameField.NonActivePlayer.Id);
            Assert.AreEqual(GameFieldState.InTurn, gameField.GameState);
            Assert.AreEqual(3, gameField.NonActivePlayer.PrizeCards.Count);
            Assert.AreEqual(2, gameField.ActivePlayer.PrizeCards.Count);
        }

        [TestMethod]
        public void PokemonDiesBurn()
        {
            var gameField = new GameField();
            gameField.InitTest();
            var originalActive = gameField.ActivePlayer;

            gameField.NonActivePlayer.Hand.Add(new WaterEnergy());
            gameField.NonActivePlayer.PrizeCards.Add(new WaterEnergy());
            gameField.NonActivePlayer.PrizeCards.Add(new WaterEnergy());
            gameField.NonActivePlayer.PrizeCards.Add(new WaterEnergy());
            gameField.ActivePlayer.Hand.Add(new WaterEnergy());
            gameField.ActivePlayer.BenchedPokemon.Add(new Magikarp(gameField.ActivePlayer));

            var activeNetworkPlayer = Substitute.For<INetworkPlayer>();
            activeNetworkPlayer.Id = gameField.ActivePlayer.Id;
            activeNetworkPlayer.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId> { gameField.ActivePlayer.BenchedPokemon.First().Id }));
            gameField.ActivePlayer.SetNetworkPlayer(activeNetworkPlayer);

            var networkPlayer = Substitute.For<INetworkPlayer>();
            networkPlayer.Id = gameField.NonActivePlayer.Id;
            networkPlayer.SendAndWaitForResponse<CardListMessage>(Arg.Any<NetworkMessage>()).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId> { gameField.NonActivePlayer.PrizeCards.First().Id }));
            gameField.NonActivePlayer.SetNetworkPlayer(networkPlayer);

            var activePokemon = new Magikarp(gameField.ActivePlayer);
            var otherPokemon = new Magikarp(gameField.NonActivePlayer);

            gameField.ActivePlayer.SetActivePokemon(activePokemon);
            gameField.NonActivePlayer.SetActivePokemon(otherPokemon);

            activePokemon.DamageCounters = activePokemon.Hp - 1;
            activePokemon.IsBurned = true;

            gameField.EndTurn();

            Assert.AreEqual(2, gameField.ActivePlayer.PrizeCards.Count);
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

            var activeNetworkPlayer = Substitute.For<INetworkPlayer>();
            activeNetworkPlayer.Id = gameField.ActivePlayer.Id;
            activeNetworkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId>
            {

            }));
            gameField.ActivePlayer.SetNetworkPlayer(activeNetworkPlayer);

            var benched = new Magikarp(gameField.NonActivePlayer);
            gameField.NonActivePlayer.BenchedPokemon.Add(benched);

            var nonActiveNetworkPlayer = Substitute.For<INetworkPlayer>();
            nonActiveNetworkPlayer.Id = gameField.NonActivePlayer.Id;
            nonActiveNetworkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<NetworkId>
            {
                benched.Id
            }));
            gameField.NonActivePlayer.SetNetworkPlayer(nonActiveNetworkPlayer);

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

        [TestMethod]
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

        [TestMethod]
        public void EvolvePokemon_RemovedFromHand()
        {
            var gameField = new GameField();
            gameField.InitTest();
            FillPlayerDecksWithJunk(gameField);

            gameField.GameState = GameFieldState.InTurn;
            gameField.ActivePlayer.ActivePokemonCard = new Ekans(gameField.ActivePlayer);

            var arbok = new DarkArbok(gameField.ActivePlayer);
            gameField.ActivePlayer.Hand.Add(arbok);
            gameField.EvolvePokemon(gameField.ActivePlayer.ActivePokemonCard, arbok);

            Assert.AreEqual(gameField.ActivePlayer.ActivePokemonCard.Id, arbok.Id);
            Assert.IsTrue(gameField.ActivePlayer.ActivePokemonCard is DarkArbok);
            Assert.IsFalse(gameField.ActivePlayer.Hand.Contains(arbok));
        }

        [TestMethod]
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

            Assert.AreEqual(gameField.ActivePlayer.BenchedPokemon[0].Id, arbok.Id);
            Assert.IsTrue(gameField.ActivePlayer.BenchedPokemon[0] is DarkArbok);

            Assert.AreNotEqual(gameField.ActivePlayer.BenchedPokemon[1].Id, arbok.Id);
            Assert.IsTrue(gameField.ActivePlayer.BenchedPokemon[1] is Ekans);
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