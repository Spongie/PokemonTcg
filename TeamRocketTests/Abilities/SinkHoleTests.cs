using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using TCGCards.EnergyCards;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class SinkHoleTests
    {
        [TestMethod]
        public void PokemonRetreats_Active()
        {
            var game = new GameField();
            game.InitTest();
            var owner = game.NonActivePlayer;
            var opponent = game.ActivePlayer;

            owner.ActivePokemonCard = new DarkDugtrio(owner);
            opponent.ActivePokemonCard = new Oddish(opponent);
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());

            opponent.BenchedPokemon.Add(new DarkGolbat(opponent));

            game.OnPokemonRetreated(opponent.BenchedPokemon.First(), new List<EnergyCard>(opponent.ActivePokemonCard.AttachedEnergy));

            Assert.AreEqual(20, opponent.BenchedPokemon.First().DamageCounters);
        }

        [TestMethod]
        public void PokemonRetreats_Benched()
        {
            var game = new GameField();
            game.InitTest();
            var owner = game.NonActivePlayer;
            var opponent = game.ActivePlayer;

            owner.ActivePokemonCard = new Oddish(owner);
            owner.BenchedPokemon.Add(new DarkDugtrio(owner));
            opponent.ActivePokemonCard = new Oddish(opponent);
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            opponent.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());

            opponent.BenchedPokemon.Add(new DarkGolbat(opponent));

            game.OnPokemonRetreated(opponent.BenchedPokemon.First(), new List<EnergyCard>(opponent.ActivePokemonCard.AttachedEnergy));

            Assert.AreEqual(20, opponent.BenchedPokemon.First().DamageCounters);
        }
    }
}
