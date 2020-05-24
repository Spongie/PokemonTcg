using BaseSet.Attacks;
using BaseSet.PokemonCards;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards.Core;
using TCGCards.EnergyCards;

namespace BaseSetTests.Attacks
{
    [TestClass]
    public class HydroPumpTests
    {
        [TestMethod]
        public void NoExtraEnergy()
        {
            var game = new GameField();
            game.InitTest();
            game.IgnorePostAttack = true;

            var player = game.ActivePlayer;
            
            var blastoise = new Blastoise(player);
            player.ActivePokemonCard = blastoise;

            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new Blastoise(opponent);

            game.Attack(blastoise.Attacks.OfType<HydroPump>().First());

            Assert.AreEqual(40, opponent.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void NoExtraEnergy_1Extra()
        {
            var game = new GameField();
            game.InitTest();
            game.IgnorePostAttack = true;

            var player = game.ActivePlayer;

            var blastoise = new Blastoise(player);
            player.ActivePokemonCard = blastoise;

            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new Blastoise(opponent);

            game.Attack(blastoise.Attacks.OfType<HydroPump>().First());

            Assert.AreEqual(50, opponent.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void NoExtraEnergy_2Extra()
        {
            var game = new GameField();
            game.InitTest();
            game.IgnorePostAttack = true;

            var player = game.ActivePlayer;

            var blastoise = new Blastoise(player);
            player.ActivePokemonCard = blastoise;

            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new Blastoise(opponent);

            game.Attack(blastoise.Attacks.OfType<HydroPump>().First());

            Assert.AreEqual(60, opponent.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void NoExtraEnergy_5Extra()
        {
            var game = new GameField();
            game.InitTest();
            game.IgnorePostAttack = true;

            var player = game.ActivePlayer;

            var blastoise = new Blastoise(player);
            player.ActivePokemonCard = blastoise;

            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new Blastoise(opponent);

            game.Attack(blastoise.Attacks.OfType<HydroPump>().First());

            Assert.AreEqual(60, opponent.ActivePokemonCard.DamageCounters);
        }

        [TestMethod]
        public void NoExtraEnergy_2Extra_Only_1_Water()
        {
            var game = new GameField();
            game.InitTest();
            game.IgnorePostAttack = true;

            var player = game.ActivePlayer;

            var blastoise = new Blastoise(player);
            player.ActivePokemonCard = blastoise;

            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new WaterEnergy());
            blastoise.AttachedEnergy.Add(new FireEnergy());

            var opponent = game.NonActivePlayer;
            opponent.ActivePokemonCard = new Blastoise(opponent);

            game.Attack(blastoise.Attacks.OfType<HydroPump>().First());

            Assert.AreEqual(50, opponent.ActivePokemonCard.DamageCounters);
        }
    }
}
