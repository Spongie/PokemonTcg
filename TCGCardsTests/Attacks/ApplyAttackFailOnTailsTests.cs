using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using Entities;

namespace TCGCards.Attacks.Tests
{
    [TestClass()]
    public class ApplyAttackFailOnTailsTests
    {
        [TestMethod()]
        public void ProcessEffectsTest_Heads()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyAttackFailOnTails();
            pokemon.Attacks.Add(attack);

            var player = new Player()
            {
                ActivePokemonCard = pokemon
            };
            var opponent = new Player
            {
                ActivePokemonCard = new PokemonCard() { Hp = 100 }
            };

            var game = new GameField();
            game.ActivePlayer = player;
            game.NonActivePlayer = opponent;

            attack.ProcessEffects(game, player, opponent);

            Assert.AreEqual(1, opponent.ActivePokemonCard.AttackStoppers.Count);
        }
    }
}