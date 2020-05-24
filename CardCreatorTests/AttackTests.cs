using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CardCreator.Tests
{
    [TestClass()]
    public class AttackTests
    {
        [TestMethod()]
        public void ParseTest_Name()
        {
            Attack attack = new Attack(createTestPokemon().Attacks.First());

            Assert.AreEqual("Teleport Blast", attack.Name);
        }

        [TestMethod()]
        public void ParseTest_Damage()
        {
            Attack attack = new Attack(createTestPokemon().Attacks.First());

            Assert.AreEqual("30", attack.Damage);
        }

        [TestMethod()]
        public void ParseTest_Description()
        {
            Attack attack = new Attack(createTestPokemon().Attacks.First());

            Assert.AreEqual("You may switch Dark Alakazam with 1 of your Benched Pokémon. (Do the damage before switching the Pokémon.)", attack.Description);
        }

        [TestMethod()]
        public void ParseTest_Name2()
        {
            Attack attack = new Attack(createTestPokemon2().Attacks.First());

            Assert.AreEqual("Dizziness", attack.Name);
        }

        [TestMethod()]
        public void ParseTest_Damage2()
        {
            Attack attack = new Attack(createTestPokemon2().Attacks.First());

            Assert.AreEqual("0", attack.Damage);
        }

        [TestMethod()]
        public void ParseTest_Description2()
        {
            Attack attack = new Attack(createTestPokemon2().Attacks.First());

            Assert.AreEqual("Draw a card.", attack.Description);
        }

        [TestMethod()]
        public void ParseTest_NeedsMode_Plus()
        {
            Attack attack = new Attack(createTestPokemon2().Attacks.First());

            Assert.IsTrue(attack.NeedsMore);
        }

        private PokemonTcgSdk.Models.PokemonCard createTestPokemon()
        {
            return new PokemonTcgSdk.Models.PokemonCard
            {
                Attacks = new List<PokemonTcgSdk.Models.Attack>
                {
                    new PokemonTcgSdk.Models.Attack()
                    {
                        Name = "Teleport Blast",
                        Damage = "30",
                        Text = "You may switch Dark Alakazam with 1 of your Benched Pokémon. (Do the damage before switching the Pokémon.)",
                        Cost = new List<string>
                        {
                            "Psychic",
                            "Psychic",
                            "Colorless"
                        }
                    }
                }
            };
        }

        private PokemonTcgSdk.Models.PokemonCard createTestPokemon2()
        {
            return new PokemonTcgSdk.Models.PokemonCard
            {
                Attacks = new List<PokemonTcgSdk.Models.Attack>
                {
                    new PokemonTcgSdk.Models.Attack()
                    {
                        Name = "Dizziness",
                        Damage = "",
                        Text = "Draw a card.",
                        Cost = new List<string>
                        {
                            "Psychic"
                        }
                    }
                }
            };
        }
    }
}