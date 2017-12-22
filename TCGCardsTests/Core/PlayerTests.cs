using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.PokemonCards.TeamRocket;

namespace TCGCards.Core.Tests
{
    [TestClass()]
    public class PlayerTests
    {
        [TestMethod()]
        public void SetActiePokemon_CardInHand()
        {
            var p = new Player();
            var card = new Magikarp();
            p.Hand.Add(card);

            p.SetActivePokemon(card);

            Assert.IsFalse(p.Hand.Contains(card));
        }

        [TestMethod()]
        public void SetActiePokemon_CardOnBench()
        {
            var p = new Player();
            var card = new Magikarp();
            p.BenchedPokemon.Add(card);

            p.SetActivePokemon(card);

            Assert.IsFalse(p.BenchedPokemon.Contains(card));
        }
    }
}