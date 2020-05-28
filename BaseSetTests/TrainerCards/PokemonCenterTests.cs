using BaseSet.PokemonCards;
using BaseSet.TrainerCards;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using TCGCards.EnergyCards;

namespace BaseSetTests.TrainerCards
{
    [TestClass]
    public class PokemonCenterTests
    {
        [TestMethod]
        public void Process()
        {
            var player = new Player();

            player.ActivePokemonCard = new Pikachu(player);
            player.BenchedPokemon.Add(new Pikachu(player));
            player.BenchedPokemon.Add(new Pikachu(player));
            player.BenchedPokemon.Add(new Pikachu(player));

            player.BenchedPokemon[0].DamageCounters = 20;
            player.BenchedPokemon[0].AttachedEnergy.Add(new WaterEnergy());
            player.BenchedPokemon[0].AttachedEnergy.Add(new WaterEnergy());


            player.BenchedPokemon[1].AttachedEnergy.Add(new WaterEnergy());

            new PokemonCenter().Process(null, player, null);

            Assert.AreEqual(0, player.BenchedPokemon[0].DamageCounters);
            Assert.AreEqual(0, player.BenchedPokemon[0].AttachedEnergy.Count);

            Assert.AreEqual(0, player.BenchedPokemon[1].DamageCounters);
            Assert.AreEqual(1, player.BenchedPokemon[1].AttachedEnergy.Count);
        }
    }
}
