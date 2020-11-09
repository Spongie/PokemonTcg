using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using Entities;

namespace TCGCards.Attacks.Tests
{
    [TestClass()]
    public class ApplyAttackPreventionTests
    {
        [TestMethod()]
        public void ProcessEffects_Flip_Heads()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyAttackPrevention()
            {
                CoinFlip = true
            };
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

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.HEADS);

            attack.ProcessEffects(game, player, opponent);

            Assert.AreEqual(1, player.ActivePokemonCard.AttackStoppers.Count);
        }

        [TestMethod()]
        public void ProcessEffects_Flip_Tails()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyAttackPrevention()
            {
                CoinFlip = true
            };
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

            CoinFlipper.ForcedNextFlips.Enqueue(CoinFlipper.TAILS);

            attack.ProcessEffects(game, player, opponent);

            Assert.AreEqual(0, player.ActivePokemonCard.AttackStoppers.Count);
        }

        [TestMethod()]
        public void ProcessEffects_All()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyAttackPrevention()
            {
                OnlySelf = false
            };
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

            Assert.AreEqual(1, player.ActivePokemonCard.AttackStoppers.Count);
            Assert.IsTrue(player.ActivePokemonCard.AttackStoppers[0].IsAttackIgnored(new PokemonCard()));
        }

        [TestMethod()]
        public void ProcessEffects_OnlySelf_OtherTarget()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyAttackPrevention()
            {
                OnlySelf = true
            };
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

            Assert.AreEqual(1, player.ActivePokemonCard.AttackStoppers.Count);
            Assert.IsFalse(player.ActivePokemonCard.AttackStoppers[0].IsAttackIgnored(new PokemonCard()));
        }

        [TestMethod()]
        public void ProcessEffects_OnlySelf_Self()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyAttackPrevention()
            {
                OnlySelf = true
            };
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

            Assert.AreEqual(1, player.ActivePokemonCard.AttackStoppers.Count);
            Assert.IsTrue(player.ActivePokemonCard.AttackStoppers[0].IsAttackIgnored(pokemon));
            Assert.IsFalse(player.ActivePokemonCard.AttackStoppers[0].IsAttackIgnored(new PokemonCard()));
        }
    }
}