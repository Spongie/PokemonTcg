namespace TCGCards.Core.Deckfilters
{
    public class EnergyFilter : IDeckFilter
    {
        public bool IsCardValid(Card card)
        {
            return card is EnergyCard;
        }
    }
}
