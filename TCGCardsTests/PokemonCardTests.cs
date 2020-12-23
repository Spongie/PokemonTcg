using Xunit;
using TCGCards;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.TrainerEffects;
using TCGCards.Core.Abilities;
using TCGCards.Core;
using System.Collections.ObjectModel;
using NetworkingCore;
using Entities;

namespace TCGCards.Tests
{
    public class PokemonCardTests
    {
        [Theory]
        [InlineData(0.5f, 0, 0, true)]
        [InlineData(0.5f, 10, 0, true)]
        [InlineData(0.5f, 20, 10, true)]
        [InlineData(0.5f, 30, 10, true)]
        [InlineData(0.5f, 40, 20, true)]
        [InlineData(0.5f, 0, 0, false)]
        [InlineData(0.5f, 10, 10, false)]
        [InlineData(0.5f, 20, 10, false)]
        [InlineData(0.5f, 30, 20, false)]
        [InlineData(0.5f, 40, 20, false)]
        [InlineData(20, 0, 0, false)]
        [InlineData(20, 10, 0, false)]
        [InlineData(20, 20, 0, false)]
        [InlineData(20, 30, 10, false)]
        [InlineData(20, 40, 20, false)]
        public void TestWithValues(float modifier, int damage, int expected, bool roundDown = false)
        {
            var game = new GameField();

            var player = new Player() { Id = NetworkId.Generate() };
            var opponent = new Player() { Id = NetworkId.Generate() };

            game.AddPlayer(player);
            game.AddPlayer(opponent);

            opponent.ActivePokemonCard = new PokemonCard()
            {
                Owner = opponent
            };

            game.ActivePlayer = opponent;
            game.NonActivePlayer = player;

            var pokemon = new PokemonCard()
            {
                Owner = player
            };

            var effect = new AttachmentEffect()
            {
                Ability = new DamageTakenModifier()
                {
                    Modifer = modifier,
                    RoundDown = roundDown
                },
                TargetingMode = TargetingMode.YourActive
            };

            player.ActivePokemonCard = pokemon;

            effect.Process(game, player, opponent, player.ActivePokemonCard);

            pokemon.DealDamage(damage, game, opponent.ActivePokemonCard, true);

            Assert.Equal(expected, pokemon.DamageCounters);
        }

        [Theory]
        [InlineData(StatusEffect.Burn)]
        [InlineData(StatusEffect.Sleep)]
        [InlineData(StatusEffect.Confuse)]
        [InlineData(StatusEffect.Poison)]
        [InlineData(StatusEffect.Paralyze)]
        public void ApplyStatusEffectTest(StatusEffect status)
        {
            var pokemon = new PokemonCard();
            pokemon.ApplyStatusEffect(status, null);

            switch (status)
            {
                case StatusEffect.Sleep:
                    Assert.True(pokemon.IsAsleep);
                    break;
                case StatusEffect.Poison:
                    Assert.True(pokemon.IsPoisoned);
                    break;
                case StatusEffect.Paralyze:
                    Assert.True(pokemon.IsParalyzed);
                    break;
                case StatusEffect.Burn:
                    Assert.True(pokemon.IsBurned);
                    break;
                case StatusEffect.Confuse:
                    Assert.True(pokemon.IsConfused);
                    break;
                case StatusEffect.None:
                    break;
                default:
                    break;
            }
        }
    }
}