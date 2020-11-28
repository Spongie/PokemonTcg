using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TCGCards.Core;
using Entities;
using TCGCards.Core.Abilities;

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

            Assert.AreEqual(1, player.ActivePokemonCard.TemporaryAbilities.Count);
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

            Assert.AreEqual(0, player.ActivePokemonCard.TemporaryAbilities.Count);
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

            Assert.AreEqual(1, player.ActivePokemonCard.TemporaryAbilities.Count);
            Assert.IsTrue(player.ActivePokemonCard.TemporaryAbilities.OfType<IAttackStoppingAbility>().First().IsStopped(game, opponent.ActivePokemonCard, new PokemonCard()));
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

            Assert.AreEqual(1, player.ActivePokemonCard.TemporaryAbilities.Count);
            Assert.IsFalse(player.ActivePokemonCard.TemporaryAbilities.OfType<IAttackStoppingAbility>().First().IsStopped(game,  opponent.ActivePokemonCard, new PokemonCard()));
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

            Assert.AreEqual(1, player.ActivePokemonCard.TemporaryAbilities.Count);
            Assert.IsFalse(player.ActivePokemonCard.TemporaryAbilities.OfType<IAttackStoppingAbility>().First().IsStopped(game, player.ActivePokemonCard, opponent.ActivePokemonCard));
            Assert.IsTrue(player.ActivePokemonCard.TemporaryAbilities.OfType<IAttackStoppingAbility>().First().IsStopped(game, opponent.ActivePokemonCard, player.ActivePokemonCard));
        }
    }
}