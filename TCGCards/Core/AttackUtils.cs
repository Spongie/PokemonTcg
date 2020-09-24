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
                target.IsParalyzed = true;
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
                target.IsParalyzed = true;
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
                target.IsBurned = true;
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
                target.IsAsleep = true;
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
                target.IsPoisoned = true;
            }
            else
            {
                log.AddMessage("Coin flipped tails, nothing happened");
            }
        }

        public static void DiscardAttachedEnergy(PokemonCard pokemon, int amount)
        {
            var message = new PickFromListMessage(pokemon.AttachedEnergy, amount);
            var response = pokemon.Owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(pokemon.Owner.Id));

            foreach (var id in response.Cards)
            {
                var energyCard = pokemon.AttachedEnergy.First(x => x.Id.Equals(id));
                pokemon.DiscardEnergyCard(energyCard);
            }
        }
    }
}
