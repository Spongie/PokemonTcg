using System;
using System.Linq;
using TCGCards.Core;

namespace TCGCards
{
    public abstract class ICard
    {
        protected string _Name;

        protected ICard(Player owner)
        {
            Id = Guid.NewGuid();
            Owner = owner;
        }

        public abstract string GetName();

        public Guid Id { get; protected set; }

        protected CardSet Set { get; set; }


        public Player Owner { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as ICard;

            if(item == null)
            {
                return false;
            }

            return item.Id.Equals(Id);
        }

        public virtual string GetLogicalName()
        {
            var type = GetType();

            var name = type.FullName.Split('.').Last();
            var nameSpace = type.Namespace.Split('.').Last();
            var assembly = type.Assembly.GetName().Name;

            return $"Cards\\{assembly}\\{nameSpace}\\{name}";
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}