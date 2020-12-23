using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace CardCreator.Tests
{
    public class AttackTests
    {
        [Fact]
        public void ParseTest_Name()
        {
            Attack attack = new Attack(createTestPokemon().Attacks.First());

            Assert.Equal("Teleport Blast", attack.Name);
        }

        [Fact]
        public void ParseTest_Damage()
        {
            Attack attack = new Attack(createTestPokemon().Attacks.First());

            Assert.Equal("30", attack.Damage);
        }

        [Fact]
        public void ParseTest_Description()
        {
            Attack attack = new Attack(createTestPokemon().Attacks.First());

            Assert.Equal("You may switch Dark Alakazam with 1 of your Benched Pokémon. (Do the damage before switching the Pokémon.)", attack.Description);
        }

        [Fact]
        public void ParseTest_Name2()
        {
            Attack attack = new Attack(createTestPokemon2().Attacks.First());

            Assert.Equal("Dizziness", attack.Name);
        }

        [Fact]
        public void ParseTest_Damage2()
        {
            Attack attack = new Attack(createTestPokemon2().Attacks.First());

            Assert.Equal("0", attack.Damage);
        }

        [Fact]
        public void ParseTest_Description2()
        {
            Attack attack = new Attack(createTestPokemon2().Attacks.First());

            Assert.Equal("Draw a card.", attack.Description);
        }

        [Fact]
        public void ParseTest_NeedsMode_Plus()
        {
            Attack attack = new Attack(createTestPokemon2().Attacks.First());

            Assert.True(attack.NeedsMore);
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