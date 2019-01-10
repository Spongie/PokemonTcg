using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingCore;
using System;
using System.Linq;
using TCGCards.Core;
using TeamRocket.PokemonCards;

namespace TCGCards.PokemonCards.TeamRocket.Attacks.Tests
{
    [TestClass()]
    public class PsyshockTests
    {
        [TestMethod()]
        public void ProcessEffectsTest()
        {
            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            var game = new GameField();
            game.Players.Add(new Player() { Id = NetworkId.Generate() });
            game.Players.Add(new Player() { Id = NetworkId.Generate() });

            game.ActivePlayer = game.Players.First();
            var attack = new Abra(game.ActivePlayer).Attacks[1];

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

            var game = new GameField();
            game.Players.Add(new Player() { Id = NetworkId.Generate() });
            game.Players.Add(new Player() { Id = NetworkId.Generate() });

            game.ActivePlayer = game.Players.First();

            game.ActivePlayer.ActivePokemonCard = new Abra(game.ActivePlayer);
            game.NonActivePlayer.ActivePokemonCard = new Abra(game.NonActivePlayer);
            var attack = game.ActivePlayer.ActivePokemonCard.Attacks[1];

            game.Attack(attack);

            Assert.IsFalse(game.ActivePlayer.ActivePokemonCard.IsParalyzed);
        }
    }
}