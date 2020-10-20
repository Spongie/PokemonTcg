using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.Core;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.PokemonCards
{
    [TestClass]
    public class CharmanderTests
    {
        [TestMethod]
        public void Serialize()
        {
            var charmander = new Charmander(new Player());

            Assert.IsNotNull(charmander.Ability.PokemonOwner);

            var json = Serializer.Serialize(charmander);
            var clone = Serializer.Deserialize<Charmander>(json);

            Assert.IsNotNull(clone.Ability.PokemonOwner);
        }
    }
}
