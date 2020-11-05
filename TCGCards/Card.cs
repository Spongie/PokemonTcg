using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Entities.Models;
using NetworkingCore;
using TCGCards.Core;

namespace TCGCards
{
    public abstract class Card : DataModel
    {
        private string setCode;
        private string name;
        private string imageUrl;
        private bool completed;

        protected Card(Player owner)
        {
            Id = NetworkId.Generate();
            Owner = owner;
        }

        public abstract string GetName();

        public NetworkId Id { get; set; }

        public string SetCode
        {
            get { return setCode; }
            set
            {
                setCode = value;
                FirePropertyChanged();
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged();
            }
        }

        public string ImageUrl
        {
            get { return imageUrl; }
            set
            {
                imageUrl = value;
                FirePropertyChanged();
            }
        }

        public bool Completed
        {
            get { return completed; }
            set
            {
                completed = value;
                FirePropertyChanged();
            }
        }

        public Player Owner { get; set; }

        public bool IsRevealed { get; set; } = true;

        public bool IsTestCard { get; set; }

        public bool IgnoreInBuilder { get; set; }

        public override bool Equals(object other)
        {
            var otherCard = other as Card;

            if(otherCard == null)
            {
                return false;
            }

            return otherCard.Id.Equals(Id);
        }

        public string GetImageName()
        {
            return ImageUrl.Split('/').Last();
        }

        public virtual string GetLogicalName()
        {
            var type = GetType();

            var name = type.FullName.Split('.').Last();
            var nameSpace = type.Namespace.Split('.').Last();
            var assembly = type.Assembly.GetName().Name;

            return Path.Combine("Cards", assembly, nameSpace, name);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static Card CreateFromTypeInfo(TypeInfo type)
        {
            var constructor = type.DeclaredConstructors.First();
            var parameters = new List<object>();

            for (int i = 0; i < constructor.GetParameters().Length; i++)
            {
                parameters.Add(null);
            }

            var card = (Card)constructor.Invoke(parameters.ToArray());
            
            if (card.IsTestCard)
            {
                return null;
            }

            return card;
        }
    }
}