using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardCreator.Tests
{
    [TestClass()]
    public class PokemonCardTests
    {
        [TestMethod()]
        public void PokemonCardTest()
        {
            var pokemon = new PokemonCard(new PokemonTcgSdk.Models.PokemonCard
            {
                Hp = "60",
                Name = "Psyduck",
                Ability = new PokemonTcgSdk.Models.Ability(),
                Types = new List<string>
                {
                     "Water"
                },
                SubType = "Basic",
                Weaknesses = new List<PokemonTcgSdk.Models.Weakness>(),
                Resistances = new List<PokemonTcgSdk.Models.Weakness>(),
                RetreatCost = new List<string>(),
                Attacks = new List<PokemonTcgSdk.Models.Attack>()
            });

            Assert.AreEqual(60, pokemon.Hp);
            Assert.AreEqual("Psyduck", pokemon.Name);
            Assert.AreEqual("Water", pokemon.Type);
            Assert.AreEqual(0, pokemon.Stage);
            Assert.IsTrue(string.IsNullOrWhiteSpace(pokemon.EvolvesFrom));
        }

        [TestMethod()]
        public void PokemonCardTest_Stage1()
        {
            var pokemon = new PokemonCard(new PokemonTcgSdk.Models.PokemonCard
            {
                Hp = "60",
                Name = "Psyduck",
                Ability = new PokemonTcgSdk.Models.Ability(),
                Types = new List<string>
                {
                     "Water"
                },
                SubType = "Stage 1",
                Weaknesses = new List<PokemonTcgSdk.Models.Weakness>(),
                Resistances = new List<PokemonTcgSdk.Models.Weakness>(),
                RetreatCost = new List<string>(),
                Attacks = new List<PokemonTcgSdk.Models.Attack>()
            });

            Assert.AreEqual(1, pokemon.Stage);
        }
    }
}