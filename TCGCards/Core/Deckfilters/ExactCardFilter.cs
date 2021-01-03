namespace TCGCards.Core.Deckfilters
{
    public struct ExactCardFilter : IDeckFilter
    {
        public bool IsCardValid(Card card)
        {
            if (Invert)
            {
                return card.Name != Name;
            }

            return card.Name == Name;
        }

        public string Name { get; set; }
        public bool Invert { get; set; }
    }
}
