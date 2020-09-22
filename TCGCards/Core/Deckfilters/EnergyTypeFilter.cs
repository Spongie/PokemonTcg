using Entities;

namespace TCGCards.Core.Deckfilters
{
    public class EnergyTypeFilter : IDeckFilter
    {
        private readonly EnergyTypes energyType;

        public EnergyTypeFilter(EnergyTypes energyType)
        {
            this.energyType = energyType;
        }

        public bool IsCardValid(Card card)
        {
            return card is EnergyCard && ((EnergyCard)card).EnergyType == energyType;
        }
    }
}
