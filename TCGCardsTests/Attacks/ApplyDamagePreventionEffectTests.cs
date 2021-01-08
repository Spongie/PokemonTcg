using Xunit;
using TCGCards.Core;
using Entities;

namespace TCGCards.Attacks.Tests
{
    public class ApplyDamagePreventionEffectTests
    {
        [Fact]
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

            var game = new GameField().WithFlips(CoinFlipper.HEADS);
            game.AddPlayer(player);
            game.AddPlayer(opponent);
            game.ActivePlayerIndex = 0;

            attack.ProcessEffects(game, player, opponent);

            Assert.Single(player.ActivePokemonCard.DamageStoppers);
        }

        [Fact]
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

            var game = new GameField().WithFlips(CoinFlipper.TAILS);
            game.AddPlayer(player);
            game.AddPlayer(opponent);
            game.ActivePlayerIndex = 0;

            attack.ProcessEffects(game, player, opponent);

            Assert.Empty(player.ActivePokemonCard.DamageStoppers);
        }

        [Fact]
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

            var game = new GameField().WithFlips(CoinFlipper.HEADS);
            game.AddPlayer(player);
            game.AddPlayer(opponent);
            game.ActivePlayerIndex = 0;

            attack.ProcessEffects(game, player, opponent);

            Assert.Empty(player.ActivePokemonCard.DamageStoppers);
            Assert.Single(game.DamageStoppers);
        }

        [Fact]
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

            var game = new GameField().WithFlips(CoinFlipper.HEADS);
            game.AddPlayer(player);
            game.AddPlayer(opponent);
            game.ActivePlayerIndex = 0;

            attack.ProcessEffects(game, player, opponent);

            Assert.Empty(player.ActivePokemonCard.DamageStoppers);
            Assert.Single(game.DamageStoppers);
            Assert.True(game.DamageStoppers[0].IsDamageIgnored(10));
        }

        [Fact]
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

            var game = new GameField().WithFlips(CoinFlipper.HEADS);
            game.AddPlayer(player);
            game.AddPlayer(opponent);
            game.ActivePlayerIndex = 0;

            attack.ProcessEffects(game, player, opponent);

            Assert.Empty(player.ActivePokemonCard.DamageStoppers);
            Assert.Single(game.DamageStoppers);
            Assert.False(game.DamageStoppers[0].IsDamageIgnored(410));
        }
    }
}