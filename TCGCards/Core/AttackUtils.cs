using Entities;
using System.Linq;
using TCGCards.Core.Messages;

namespace TCGCards.Core
{
    public static class AttackUtils
    {
        public static void FlipCoinIfHeadsParalyzed(GameLog log, PokemonCard target)
        {
            if (CoinFlipper.FlipCoin())
            {
                log.AddMessage("Coin flipped heads defending pokemon is now Paralyzed");
                target.ApplyStatusEffect(StatusEffect.Paralyze);
            }
            else
            {
                log.AddMessage("Coin flipped tails, nothing happened");
            }
        }

        public static void FlipCoinIfHeadsConfused(GameLog log, PokemonCard target)
        {
            if (CoinFlipper.FlipCoin())
            {
                log.AddMessage("Coin flipped heads defending pokemon is now Confused");
                target.ApplyStatusEffect(StatusEffect.Confuse);
            }
            else
            {
                log.AddMessage("Coin flipped tails, nothing happened");
            }
        }

        public static void FlipCoinIfHeadsBurned(GameLog log, PokemonCard target)
        {
            if (CoinFlipper.FlipCoin())
            {
                log.AddMessage("Coin flipped heads defending pokemon is now Burned");
                target.ApplyStatusEffect(StatusEffect.Burn);
            }
            else
            {
                log.AddMessage("Coin flipped tails, nothing happened");
            }
        }

        public static void FlipCoinIfHeadsAsleep(GameLog log, PokemonCard target)
        {
            if (CoinFlipper.FlipCoin())
            {
                log.AddMessage("Coin flipped heads defending pokemon is now Asleep");
                target.ApplyStatusEffect(StatusEffect.Sleep);
            }
            else
            {
                log.AddMessage("Coin flipped tails, nothing happened");
            }
        }

        public static void FlipCoinIfHeadsPoisoned(GameLog log, PokemonCard target)
        {
            if (CoinFlipper.FlipCoin())
            {
                log.AddMessage("Coin flipped heads defending pokemon is now Poisoned");
                target.ApplyStatusEffect(StatusEffect.Poison);
            }
            else
            {
                log.AddMessage("Coin flipped tails, nothing happened");
            }
        }

        public static void DiscardAttachedEnergy(PokemonCard pokemon, int amount, GameField game)
        {
            var message = new PickFromListMessage(pokemon.AttachedEnergy, amount);
            var response = pokemon.Owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(pokemon.Owner.Id));

            foreach (var id in response.Cards)
            {
                var energyCard = pokemon.AttachedEnergy.First(x => x.Id.Equals(id));
                pokemon.DiscardEnergyCard(energyCard, game);
            }
        }
    }
}
