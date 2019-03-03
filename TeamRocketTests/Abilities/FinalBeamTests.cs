using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using NSubstitute;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.EnergyCards;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class FinalBeamTests
    {
        [TestMethod]
        public void Pralyzed_NotActivated()
        {
            var game = new GameField();
            game.InitTest();

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new DarkMachamp(owner);
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new DarkGyarados(opponent);
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.IsParalyzed = true;

            game.Attack(owner.ActivePokemonCard.Attacks.First());

            Assert.AreEqual(0, owner.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void Confused_NotActivated()
        {
            var game = new GameField();
            game.InitTest();

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new DarkMachamp(owner);
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new DarkGyarados(opponent);
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.IsConfused = true;

            game.Attack(owner.ActivePokemonCard.Attacks.First());

            Assert.AreEqual(0, owner.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void Asleep_NotActivated()
        {
            var game = new GameField();
            game.InitTest();

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new DarkMachamp(owner);
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new DarkGyarados(opponent);
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.IsAsleep = true;

            game.Attack(owner.ActivePokemonCard.Attacks.First());

            Assert.AreEqual(0, owner.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void Activated_3_Energy()
        {
            GameField game = createGameField();

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new DarkMachamp(owner);
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new DarkGyarados(opponent);
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.DamageCounters = 100;

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            game.Attack(owner.ActivePokemonCard.Attacks.First());

            Assert.AreEqual(60, owner.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void Activated_2_Energy()
        {
            GameField game = createGameField();

            var owner = game.ActivePlayer;
            owner.ActivePokemonCard = new DarkMachamp(owner);
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());
            owner.ActivePokemonCard.AttachedEnergy.Add(new FightingEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new DarkGyarados(opponent);
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new WaterEnergy());
            opponent.ActivePokemonCard.DamageCounters = 100;

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            game.Attack(owner.ActivePokemonCard.Attacks.First());

            Assert.AreEqual(40, owner.ActivePokemonCard.DamageCounters);
        }

        private static GameField createGameField()
        {
            var game = new GameField();
            game.InitTest();

            game.ActivePlayer.PrizeCards.Add(new WaterEnergy());
            var activeNetworkPlayer = Substitute.For<INetworkPlayer>();
            activeNetworkPlayer.Id = game.ActivePlayer.Id;
            activeNetworkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<Card>
            {
                game.ActivePlayer.PrizeCards.First()
            }));

            game.ActivePlayer.SetNetworkPlayer(activeNetworkPlayer);

            var nonActiveNetworkPlayer = Substitute.For<INetworkPlayer>();
            nonActiveNetworkPlayer.Id = game.NonActivePlayer.Id;
            nonActiveNetworkPlayer.SendAndWaitForResponse<CardListMessage>(null).ReturnsForAnyArgs(new CardListMessage(new List<Card>
            {
                new Magikarp(game.NonActivePlayer)
            }));
            game.NonActivePlayer.SetNetworkPlayer(nonActiveNetworkPlayer);
            return game;
        }
    }
}
