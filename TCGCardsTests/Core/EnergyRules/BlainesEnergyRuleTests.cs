using Entities;
using NetworkingCore;
using TCGCards;
using TCGCards.Core.EnergyRules;
using Xunit;

namespace TCGCardsTests.Core.EnergyRules
{
    public class BlainesEnergyRuleTests
    {
        [Theory]
        [InlineData(EnergyTypes.Water)]
        [InlineData(EnergyTypes.Colorless)]
        [InlineData(EnergyTypes.Darkness)]
        [InlineData(EnergyTypes.Dragon)]
        [InlineData(EnergyTypes.Electric)]
        [InlineData(EnergyTypes.Fairy)]
        [InlineData(EnergyTypes.Fighting)]
        [InlineData(EnergyTypes.Fire)]
        [InlineData(EnergyTypes.Grass)]
        [InlineData(EnergyTypes.Psychic)]
        [InlineData(EnergyTypes.Steel)]
        public void CanAttachOnlyOneEnergyToNonBlain(EnergyTypes energyType)
        {
            var card = new EnergyCard { EnergyType = energyType };
            var pokemon = new PokemonCard { Name = "Pickachu" };

            var rule = new BlainesEnergyRule();

            Assert.True(rule.CanPlayEnergyCard(card, pokemon));

            rule.CardPlayed(card, pokemon);

            Assert.False(rule.CanPlayEnergyCard(card, pokemon));
        }

        [Theory]
        [InlineData(EnergyTypes.Water)]
        [InlineData(EnergyTypes.Colorless)]
        [InlineData(EnergyTypes.Darkness)]
        [InlineData(EnergyTypes.Dragon)]
        [InlineData(EnergyTypes.Electric)]
        [InlineData(EnergyTypes.Fairy)]
        [InlineData(EnergyTypes.Fighting)]
        [InlineData(EnergyTypes.Grass)]
        [InlineData(EnergyTypes.Psychic)]
        [InlineData(EnergyTypes.Steel)]
        public void CanAttachOnlyOneNonFireEnergyToBlain(EnergyTypes energyType)
        {
            var card = new EnergyCard { EnergyType = energyType };
            var pokemon = new PokemonCard { Name = "Blaine's Pickachu" };

            var rule = new BlainesEnergyRule();

            Assert.True(rule.CanPlayEnergyCard(card, pokemon));

            rule.CardPlayed(card, pokemon);

            Assert.False(rule.CanPlayEnergyCard(card, pokemon));
        }

        [Fact]
        public void CanAttach2FireToBlainesPokemon()
        {
            var card = new EnergyCard { EnergyType = EnergyTypes.Fire };
            var pokemon = new PokemonCard { Name = "Blaine's Pickachu" };

            var rule = new BlainesEnergyRule();

            Assert.True(rule.CanPlayEnergyCard(card, pokemon));

            rule.CardPlayed(card, pokemon);

            Assert.True(rule.CanPlayEnergyCard(card, pokemon));
        }
    }
}
