using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using TeamRocket.Abilities;
using TeamRocket.PokemonCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class LongDistanceHypnosisTests
    {
        [TestMethod]
        public void Activate_Heads()
        {
            var player = new Player();
            var opponent = new Player();
            player.ActivePokemonCard = new DarkGloom(player);
            opponent.ActivePokemonCard = new Oddish(opponent);

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            var ability = new LongDistanceHypnosis(player.ActivePokemonCard);
            ability.Trigger(player, opponent, 0);

            Assert.IsTrue(opponent.ActivePokemonCard.IsAsleep);
            Assert.IsFalse(player.ActivePokemonCard.IsAsleep);
        }

        [TestMethod]
        public void Activate_Tails()
        {
            var player = new Player();
            var opponent = new Player();
            player.ActivePokemonCard = new DarkGloom(player);
            opponent.ActivePokemonCard = new Oddish(opponent);

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);

            var ability = new LongDistanceHypnosis(player.ActivePokemonCard);
            ability.Trigger(player, opponent, 0);

            Assert.IsFalse(opponent.ActivePokemonCard.IsAsleep);
            Assert.IsTrue(player.ActivePokemonCard.IsAsleep);
        }
    }
}
