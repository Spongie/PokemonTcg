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

        public override bool Equals(object obj)
        {
            var item = obj as ICard;

            if(item == null)
            {
                return false;
            }

            return item.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}