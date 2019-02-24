using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards;
using TCGCards.Core;
using TeamRocket.PokemonCards;
using TeamRocket.TrainerCards;

namespace TeamRocketTests.Abilities
{
    [TestClass]
    public class HayFeverTests
    {
        [TestMethod]
        public void Trainer_Not_Played()
        {
            var game = new GameField();
            game.InitTest();

            game.ActivePlayer.ActivePokemonCard = new DarkVileplume(game.ActivePlayer);

            game.ActivePlayer.Hand.Add(new HereComesTeamRocket());

            game.PlayerTrainerCard((TrainerCard)game.ActivePlayer.Hand.First());

            Assert.AreEqual(0, game.ActivePlayer.DiscardPile.Count);
        }
    }
}
