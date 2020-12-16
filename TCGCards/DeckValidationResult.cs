using System.Collections.Generic;

namespace TCGCards
{
    public struct DeckValidationResult
    {
        public bool Result { get; set; }
        public List<string> Messages { get; set; }
    }
}
