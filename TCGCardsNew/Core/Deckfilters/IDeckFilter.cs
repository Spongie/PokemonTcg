namespace TCGCards.Core
{
    public interface IDeckFilter
    {
        bool IsCardValid(ICard card);
    }
}
