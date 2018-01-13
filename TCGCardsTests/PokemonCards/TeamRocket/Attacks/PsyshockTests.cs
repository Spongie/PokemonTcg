using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.PokemonCards.TeamRocket.Attacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCards.Core;

namespace TCGCards.PokemonCards.TeamRocket.Attacks.Tests
{
    [TestClass()]
    public class PsyshockTests
    {
        [TestMethod()]
        public void ProcessEffectsTest()
        {
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            var attack = new Psyshock();

            var game = new GameField();
            game.Players.Add(new Player() { Id = Guid.NewGuid() });
            game.Players.Add(new Player() { Id = Guid.NewGuid() });

            game.ActivePlayer = game.Players.First();

            game.ActivePlayer.ActivePokemonCard = new Abra(game.ActivePlayer);
            game.NonActivePlayer.ActivePokemonCard = new Abra(game.NonActivePlayer);

            game.Attack(attack);

            //Checking active player because attack ends turn
            Assert.IsTrue(game.ActivePlayer.ActivePokemonCard.IsParalyzed);
        }

        [TestMethod()]
        public void ProcessEffectsTest_Tails()
        {
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);

            var attack = new Psyshock();

            var game = new GameField();
            game.Players.Add(new Player() { Id = Guid.NewGuid() });
            game.Players.Add(new Player() { Id = Guid.NewGuid() });

            game.ActivePlayer = game.Players.First();

            game.ActivePlayer.ActivePokemonCard = new Abra(game.ActivePlayer);
            game.NonActivePlayer.ActivePokemonCard = new Abra(game.NonActivePlayer);

            game.Attack(attack);

            Assert.IsFalse(game.ActivePlayer.ActivePokemonCard.IsParalyzed);
        }

        [TestMethod()]
        public void GetDamage()
        {
            var attack = new Psyshock();

            Assert.AreEqual(10, attack.GetDamage(null, null));
        }
    }
}