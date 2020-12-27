using TCGCards.Core;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Entities;

namespace TCGCards.Core.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void SetActivePokemon_CardInHand()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            p.Hand.Add(card);

            p.SetActivePokemon(card);

            Assert.False(p.Hand.Contains(card));
        }

        [Fact]
        public void SetActivePokemon_CardOnBench()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            p.BenchedPokemon.Add(card);

            p.SetActivePokemon(card);

            Assert.False(p.BenchedPokemon.Contains(card));
        }

        [Fact]
        public void SetActivePokemon_SpotTaken()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            var card2 = new PokemonCard(p);

            p.SetActivePokemon(card);
            p.SetActivePokemon(card2);

            Assert.Equal(card.Id, p.ActivePokemonCard.Id);
            Assert.False(p.BenchedPokemon.Count > 0);
        }

        [Fact]
        public void RetreatActivePokemon_NoEnergy()
        {
            var p = new Player();
            var card = new PokemonCard(p) { RetreatCost = 1 };
            var card2 = new PokemonCard(p);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.RetreatActivePokemon(p.BenchedPokemon.ValidPokemonCards.First(), new List<EnergyCard>(), new GameField());

            Assert.Equal(card.Id, p.ActivePokemonCard.Id);
        }

        [Fact]
        public void RetreatActivePokemon_Energy()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            var card2 = new PokemonCard(p);
            var energyCard = new EnergyCard() { Amount = 1 };
            p.Hand.Add(energyCard);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.PlayEnergyCard(energyCard, p.ActivePokemonCard, null);

            p.RetreatActivePokemon(p.BenchedPokemon.ValidPokemonCards.First(), new List<EnergyCard>(), new GameField());

            Assert.Equal(card2.Id, p.ActivePokemonCard.Id);
        }

        [Fact]
        public void RetreatActivePokemon_Energy_Removed()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            var card2 = new PokemonCard(p);
            var energyCard = new EnergyCard() { Amount = 1 };
            p.Hand.Add(energyCard);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.PlayEnergyCard(energyCard, p.ActivePokemonCard);

            p.RetreatActivePokemon(p.BenchedPokemon.ValidPokemonCards.First(), new List<EnergyCard>(p.ActivePokemonCard.AttachedEnergy), new GameField());

            Assert.Equal(card2.Id, p.ActivePokemonCard.Id);
            Assert.False(card.AttachedEnergy.Any());
        }

        [Fact]
        public void RetreatActivePokemon_StatusReset()
        {
            var p = new Player();
            var card = new PokemonCard(p)
            {
                IsBurned = true
            };

            var card2 = new PokemonCard(p);
            var energyCard = new EnergyCard() { Amount = 1 };
            p.Hand.Add(energyCard);

            p.SetActivePokemon(card);
            p.SetBenchedPokemon(card2);

            p.PlayEnergyCard(energyCard, p.ActivePokemonCard);

            p.RetreatActivePokemon(p.BenchedPokemon.ValidPokemonCards.First(), new List<EnergyCard>(p.ActivePokemonCard.AttachedEnergy), new GameField());

            Assert.Equal(card2.Id, p.ActivePokemonCard.Id);
            Assert.False(card.IsBurned);
        }

        [Fact]
        public void PlayCardTest_NoActive()
        {
            var p = new Player();
            var card = new PokemonCard(p);

            p.PlayCard(card);

            Assert.Equal(card, p.ActivePokemonCard);
        }

        [Fact]
        public void PlayCardTest_Active()
        {
            var p = new Player();
            var card = new PokemonCard(p);
            var card2 = new PokemonCard(p);
            p.PlayCard(card);
            p.PlayCard(card2);

            Assert.Equal(card, p.ActivePokemonCard);
            Assert.Equal(card2, p.BenchedPokemon.ValidPokemonCards.First());
        }

        [Fact]
        public void KillActivePokemon_Basic_NothingAttached()
        {
            var player = new Player();
            player.ActivePokemonCard = new PokemonCard(player);

            player.KillActivePokemon();

            Assert.Null(player.ActivePokemonCard);
            Assert.Single(player.DiscardPile);
        }

        [Fact]
        public void KillActivePokemon_Basic_AttachedEnergy()
        {
            var player = new Player();
            player.ActivePokemonCard = new PokemonCard(player);
            player.ActivePokemonCard.AttachedEnergy.Add(new EnergyCard());
            player.ActivePokemonCard.AttachedEnergy.Add(new EnergyCard());

            player.KillActivePokemon();

            Assert.Null(player.ActivePokemonCard);
            Assert.Equal(3, player.DiscardPile.Count);
        }

        [Fact]
        public void KillActivePokemon_StageOne_AttachedEnergy()
        {
            var player = new Player();
            var abra = new PokemonCard(player);
            var kadabra = new PokemonCard(player);

            abra.Evolve(kadabra);

            player.ActivePokemonCard = kadabra;
            player.ActivePokemonCard.AttachedEnergy.Add(new EnergyCard());
            player.ActivePokemonCard.AttachedEnergy.Add(new EnergyCard());

            player.KillActivePokemon();

            Assert.Null(player.ActivePokemonCard);
            Assert.Equal(4, player.DiscardPile.Count);
        }

        [Fact]
        public void KillActivePokemon_StageOne_NoEnergy()
        {
            var player = new Player();
            var abra = new PokemonCard(player);
            var kadabra = new PokemonCard(player);

            abra.Evolve(kadabra);

            player.ActivePokemonCard = kadabra;

            player.KillActivePokemon();

            Assert.Null(player.ActivePokemonCard);
            Assert.Equal(2, player.DiscardPile.Count);
        }

        [Fact]
        public void KillActivePokemonTest_FullyHealedInDiscard()
        {
            var player = new Player();
            var pokemon = new PokemonCard()
            {
                Owner = player,
                Hp = 50
            };

            player.ActivePokemonCard = pokemon;

            pokemon.DamageCounters = 60;
            pokemon.ApplyStatusEffect(StatusEffect.Burn, null);
            pokemon.ApplyStatusEffect(StatusEffect.Confuse, null);
            pokemon.ApplyStatusEffect(StatusEffect.Paralyze, null);
            pokemon.ApplyStatusEffect(StatusEffect.Poison, null);
            pokemon.ApplyStatusEffect(StatusEffect.Sleep, null);

            player.KillActivePokemon();

            Assert.Single(player.DiscardPile);

            Assert.False(pokemon.IsAsleep);
            Assert.False(pokemon.IsPoisoned);
            Assert.False(pokemon.IsParalyzed);
            Assert.False(pokemon.IsBurned);
            Assert.False(pokemon.IsConfused);
            Assert.Equal(0, pokemon.DamageCounters);
        }

        [Fact]
        public void KillBenchedPokemonTest()
        {
            var player = new Player();
            var pokemon = new PokemonCard()
            {
                Owner = player,
                Hp = 50
            };

            player.ActivePokemonCard = new PokemonCard();
            player.BenchedPokemon.Add(pokemon);

            pokemon.DamageCounters = 60;
            pokemon.ApplyStatusEffect(StatusEffect.Burn, null);
            pokemon.ApplyStatusEffect(StatusEffect.Confuse, null);
            pokemon.ApplyStatusEffect(StatusEffect.Paralyze, null);
            pokemon.ApplyStatusEffect(StatusEffect.Poison, null);
            pokemon.ApplyStatusEffect(StatusEffect.Sleep, null);

            player.KillBenchedPokemon(pokemon);

            Assert.Single(player.DiscardPile);
            Assert.Equal(0, player.BenchedPokemon.Count);

            Assert.False(pokemon.IsAsleep);
            Assert.False(pokemon.IsPoisoned);
            Assert.False(pokemon.IsParalyzed);
            Assert.False(pokemon.IsBurned);
            Assert.False(pokemon.IsConfused);
            Assert.Equal(0, pokemon.DamageCounters);
        }

        [Fact()]
        public void ForceRetreatActivePokemon_No_replacement()
        {
            var player = new Player();
            var pokemon = new PokemonCard
            {
                Owner = player
            };
            player.ActivePokemonCard = pokemon;

            player.ForceRetreatActivePokemon(null, new GameField());

            Assert.Null(player.ActivePokemonCard);
            Assert.Equal(pokemon, player.BenchedPokemon.GetFirst());
        }
    }
}