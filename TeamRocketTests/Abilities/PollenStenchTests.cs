using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using TeamRocket.Abilities;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class PollenStenchTests
    {
        [TestMethod]
        public void Activate_Heads()
        {
            var player = new Player();
            var opponent = new Player();
            player.ActivePokemonCard = new DarkGloom(player);
            opponent.ActivePokemonCard = new Oddish(opponent);

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            var ability = new PollenStench(player.ActivePokemonCard);
            ability.Trigger(player, opponent, 0, new GameLog());

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

            var ability = new PollenStench(player.ActivePokemonCard);
            ability.Trigger(player, opponent, 0, new GameLog());

            Assert.IsFalse(opponent.ActivePokemonCard.IsConfused);
            Assert.IsTrue(player.ActivePokemonCard.IsConfused);
        }
    }
}
