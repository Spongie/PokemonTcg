using Xunit;
using System.Collections.Generic;

namespace CardCreator.Tests
{
    public class PokemonCardTests
    {
        [Fact]
        public void PokemonCardTest()
        {
            var pokemon = new CreatorPokemonCard(new PokemonTcgSdk.Models.PokemonCard
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

            Assert.Equal(60, pokemon.Hp);
            Assert.Equal("Psyduck", pokemon.Name);
            Assert.Equal("Water", pokemon.Type);
            Assert.Equal(0, pokemon.Stage);
            Assert.True(string.IsNullOrWhiteSpace(pokemon.EvolvesFrom));
        }

        [Fact]
        public void PokemonCardTest_Stage1()
        {
            var pokemon = new CreatorPokemonCard(new PokemonTcgSdk.Models.PokemonCard
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

            Assert.Equal(1, pokemon.Stage);
        }
    }
}