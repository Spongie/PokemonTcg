using System.Collections.Generic;

namespace TCGCards.Core
{
    public interface IDeckSearcher
    {
        int GetNumberOfCards();
        List<IDeckFilter> GetDeckFilters();
    }
}
