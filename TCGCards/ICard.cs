using System;

namespace TCGCards
{
    public abstract class ICard
    {
        protected readonly Guid _Id;
        protected string _Name;
        private CardSet set;

        protected ICard()
        {
            _Id = Guid.NewGuid();
        }

        public abstract string GetName();

        public Guid Id
        {
            get { return _Id; }
        }

        protected CardSet Set
        {
            get
            {
                return set;
            }

            set
            {
                set = value;
            }
        }
    }
}