using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards.Core;
using Entities;

namespace TCGCards.Attacks.Tests
{
    [TestClass()]
    public class ApplyDamagePreventionEffectTests
    {
        [TestMethod()]
        public void ProcessEffectsTest_Flip_Heads()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyDamagePreventionEffect()
            {
                CoinFlip = true,
                OnlyProtectSelf = true
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

            Assert.AreEqual(1, player.ActivePokemonCard.DamageStoppers.Count);
        }

        [TestMethod()]
        public void ProcessEffectsTest_Flip_Tails()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyDamagePreventionEffect()
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

            Assert.AreEqual(0, player.ActivePokemonCard.DamageStoppers.Count);
        }

        [TestMethod()]
        public void ProcessEffectsTest_ProtectsAll()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyDamagePreventionEffect()
            {
                CoinFlip = true,
                OnlyProtectSelf = false
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

            Assert.AreEqual(0, player.ActivePokemonCard.DamageStoppers.Count);
            Assert.AreEqual(1, game.DamageStoppers.Count);
        }

        [TestMethod()]
        public void ProcessEffectsTest_PreventionLimit_LowDamage()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyDamagePreventionEffect()
            {
                CoinFlip = true,
                OnlyProtectSelf = false,
                MaxDamage = 20
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

            Assert.AreEqual(0, player.ActivePokemonCard.DamageStoppers.Count);
            Assert.AreEqual(1, game.DamageStoppers.Count);
            Assert.IsTrue(game.DamageStoppers[0].IsDamageIgnored(10));
        }

        [TestMethod()]
        public void ProcessEffectsTest_PreventionLimit_HighDamage()
        {
            var pokemon = new PokemonCard() { Hp = 100 };
            var attack = new ApplyDamagePreventionEffect()
            {
                CoinFlip = true,
                OnlyProtectSelf = false,
                MaxDamage = 20
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

            Assert.AreEqual(0, player.ActivePokemonCard.DamageStoppers.Count);
            Assert.AreEqual(1, game.DamageStoppers.Count);
            Assert.IsFalse(game.DamageStoppers[0].IsDamageIgnored(410));
        }
    }
}