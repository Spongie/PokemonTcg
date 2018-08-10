namespace TCGCards.Core.Deckfilters
{
    public class EnergyFilter : IDeckFilter
    {
        public bool IsCardValid(ICard card)
        {
            return card is IEnergyCard;
        }
    }
}
