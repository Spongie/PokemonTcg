using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
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
            opponent.ActivePokemonCard.DamageCounters = 100;

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            game.Attack(owner.ActivePokemonCard.Attacks.First());

            Assert.AreEqual(60, owner.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void Activated_2_Energy()
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
            opponent.ActivePokemonCard.DamageCounters = 100;

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            game.Attack(owner.ActivePokemonCard.Attacks.First());

            Assert.AreEqual(40, owner.ActivePokemonCard.DamageCounters);
        }
    }
}
