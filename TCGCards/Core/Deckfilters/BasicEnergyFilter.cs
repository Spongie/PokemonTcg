namespace TCGCards.Core.Deckfilters
{
    public class BasicEnergyFilter : IDeckFilter
    {
        public bool IsCardValid(Card card)
        {
            return card is EnergyCard && ((EnergyCard)card).IsBasic;
        }
    }
}
