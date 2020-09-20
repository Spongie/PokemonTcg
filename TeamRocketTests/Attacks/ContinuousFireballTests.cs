using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards.Core;
using TCGCards.EnergyCards;
using TeamRocket.Attacks;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Attacks
{
    [TestClass]
    public class ContinuousFireballTests
    {
        [TestMethod]
        public void GetDamage_DiscardsEnergy()
        {
            var player = new Player();
            var charizard = new DarkCharizard(player);
            var attack = charizard.Attacks.OfType<ContinuousFireball>().First();
            player.ActivePokemonCard = charizard;

            charizard.AttachedEnergy.Add(new FireEnergy());
            charizard.AttachedEnergy.Add(new FireEnergy());
            charizard.AttachedEnergy.Add(new FireEnergy());

            CoinFlipper.ForcedNextFlips.Clear();
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);

            var damage = attack.GetDamage(player, null);

            Assert.AreEqual(100, damage.NormalDamage);
            Assert.AreEqual(2, player.DiscardPile.Count);
            Assert.AreEqual(1, charizard.AttachedEnergy.Count);
        }
    }
}
