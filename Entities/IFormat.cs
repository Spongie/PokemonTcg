using System.Collections.Generic;

namespace Entities
{
    public interface IFormat
    {
        string GetName();
        long GetFormatId();
        List<string> GetSetIds();
        List<CardInfo> GetBannedCards();
    }
}
