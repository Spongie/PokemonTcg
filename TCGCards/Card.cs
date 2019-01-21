using System.Linq;
using NetworkingCore;
using TCGCards.Core;

namespace TCGCards
{
    public abstract class Card
    {
        protected string _Name;

        protected Card(Player owner)
        {
            Id = NetworkId.Generate();
            Owner = owner;
        }

        public abstract string GetName();

        public NetworkId Id { get; set; }

        protected CardSet Set { get; set; }

        public Player Owner { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as Card;

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