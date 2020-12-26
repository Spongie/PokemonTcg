using Entities;
using NetworkingCore.Messages;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;

namespace TCGCards.TrainerEffects.Util
{
    public static class CardUtil
    {
        public static List<Card> GetCardsOfType(List<Card> cards, CardType cardType)
        {
            switch (cardType)
            {
                case CardType.Pokemon:
                    return cards.OfType<PokemonCard>().OfType<Card>().ToList();
                case CardType.BasicPokemon:
                    return cards.OfType<PokemonCard>().Where(pokemon => pokemon.Stage == 0).OfType<Card>().ToList();
                case CardType.Trainer:
                    return cards.OfType<TrainerCard>().OfType<Card>().ToList();
                case CardType.Energy:
                    return cards.OfType<EnergyCard>().OfType<Card>().ToList();
                case CardType.BasicEnergy:
                    return cards.OfType<EnergyCard>().Where(energy => energy.IsBasic).OfType<Card>().ToList();
                case CardType.Any:
                default:
                    return cards;
            }
        }

        public static List<IDeckFilter> GetCardFilters(CardType cardType, EnergyTypes energyType = EnergyTypes.None)
        {
            var filter = new List<IDeckFilter>();

            switch (cardType)
            {
                case CardType.Pokemon:
                    filter.Add(new PokemonFilter());
                    break;
                case CardType.BasicPokemon:
                    filter.Add(new BasicPokemonFilter());
                    break;
                case CardType.Trainer:
                    filter.Add(new TrainerFilter());
                    break;
                case CardType.Energy:
                    filter.Add(new EnergyFilter());
                    break;
                case CardType.BasicEnergy:
                    if (energyType == EnergyTypes.All || energyType == EnergyTypes.None)
                    {
                        filter.Add(new BasicEnergyFilter());
                    }
                    else
                    {
                        filter.Add(new EnergyTypeFilter(energyType));
                    }
                    break;
                default:
                    break;
            }

            return filter;
        }
    }
}
