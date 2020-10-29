namespace TCGCards.Core.Deckfilters
{
    public class TrainerFilter : IDeckFilter
    {
        public bool IsCardValid(Card card)
        {
            return card is TrainerCard;
        }
    }
}
