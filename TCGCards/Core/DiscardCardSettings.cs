namespace TCGCards.Core
{
    public struct DiscardCardSettings
    {
        public DiscardCardSettings(int amount, IDeckFilter[] filters, bool shuffleIntoDeck) :this(amount, amount, filters, shuffleIntoDeck)
        {

        }

        public DiscardCardSettings(int minAmount, int maxAmount, IDeckFilter[] filters, bool shuffleIntoDeck) : this()
        {
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            Filters = filters;
            ShuffleIntoDeck = shuffleIntoDeck;
        }

        public int MinAmount { get; set; }
        public int MaxAmount { get; set; }
        public IDeckFilter[] Filters { get; set; }
        public bool ShuffleIntoDeck { get; set; }
    }
}
