namespace TCGCards.Core
{
    public readonly struct DiscardCardSettings
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

        public int MinAmount { get; }
        public int MaxAmount { get; }
        public IDeckFilter[] Filters { get; }
        public bool ShuffleIntoDeck { get; }
    }
}
