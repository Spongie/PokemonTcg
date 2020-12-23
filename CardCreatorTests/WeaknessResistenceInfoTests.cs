using Xunit;
using System.Collections.Generic;

namespace CardCreator.Tests
{
    public class WeaknessResistenceInfoTests
    {
        [Fact]
        public void ParseTest_Resistence()
        {
            var weaknessResistence = CreateWeaknessResistenceFromHtml();
            Assert.Equal("Fighting", weaknessResistence.Resistence);
        }

        [Fact]
        public void ParseTest_Weakness()
        {
            var weaknessResistence = CreateWeaknessResistenceFromHtml();
            Assert.Equal("Psychic", weaknessResistence.Weakness);
        }

        [Fact]
        public void ParseTest_Retreat()
        {
            var weaknessResistence = CreateWeaknessResistenceFromHtml();
            Assert.Equal(1, weaknessResistence.RetreatCost);
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