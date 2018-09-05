namespace TCGCards.Core
{
    public interface IDeckFilter
    {
        bool IsCardValid(Card card);
    }
}
