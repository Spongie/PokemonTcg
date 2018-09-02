using System.Collections.Generic;

namespace Entities
{
    public interface IFormat
    {
        string GetName();
        long GetFormatId();
        List<long> GetSetIds();
        List<string> GetBannedCardsClassNames();
    }
}
