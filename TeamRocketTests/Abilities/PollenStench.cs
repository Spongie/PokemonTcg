using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class PollenStench
    {
        [TestMethod]
        public void Activate_Heads()
        {
            var player = new Player();
            var opponent = new Player();
            player.ActivePokemonCard = new DarkGloom(player);
            opponent.ActivePokemonCard = new Oddish(opponent);

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            player.ActivePokemonCard.Ability.Trigger(player, opponent, 0);

            Assert.IsTrue(opponent.ActivePokemonCard.IsConfused);
            Assert.IsFalse(player.ActivePokemonCard.IsConfused);
        }

        [TestMethod]
        public void Activate_Tails()
        {
            var player = new Player();
            var opponent = new Player();
            player.ActivePokemonCard = new DarkGloom(player);
            opponent.ActivePokemonCard = new Oddish(opponent);

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);

            player.ActivePokemonCard.Ability.Trigger(player, opponent, 0);

            Assert.IsFalse(opponent.ActivePokemonCard.IsConfused);
            Assert.IsTrue(player.ActivePokemonCard.IsConfused);
        }
    }
}
