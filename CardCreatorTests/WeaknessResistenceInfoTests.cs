using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CardCreator.Tests
{
    [TestClass()]
    public class WeaknessResistenceInfoTests
    {
        [TestMethod()]
        public void ParseTest_Resistence()
        {
            var weaknessResistence = CreateWeaknessResistenceFromHtml();
            Assert.AreEqual("Fighting", weaknessResistence.Resistence);
        }

        [TestMethod()]
        public void ParseTest_Weakness()
        {
            var weaknessResistence = CreateWeaknessResistenceFromHtml();
            Assert.AreEqual("Psychic", weaknessResistence.Weakness);
        }

        [TestMethod()]
        public void ParseTest_Retreat()
        {
            var weaknessResistence = CreateWeaknessResistenceFromHtml();
            Assert.AreEqual(1, weaknessResistence.RetreatCost);
        }

        private static WeaknessResistenceInfo CreateWeaknessResistenceFromHtml()
        {
            return new WeaknessResistenceInfo(new PokemonTcgSdk.Models.PokemonCard
            {
                RetreatCost = new List<string>
                {
                    "Colorless"
                },
                Weaknesses = new List<PokemonTcgSdk.Models.Weakness>
                {
                    new PokemonTcgSdk.Models.Weakness
                    {
                        Type = "Psychic",
                        Value = "x2"
                    }
                },
                Resistances = new List<PokemonTcgSdk.Models.Weakness>
                {
                    new PokemonTcgSdk.Models.Weakness
                    {
                        Type = "Fighting",
                        Value = "-30"
                    }
                }
            });
        }
    }
}