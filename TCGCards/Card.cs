﻿using System.IO;
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

        public CardSet Set { get; set; }

        public Player Owner { get; set; }

        public bool IsRevealed { get; set; }

        public bool IsTestCard { get; set; }

        public override bool Equals(object other)
        {
            var otherCard = other as Card;

            if(otherCard == null)
            {
                return false;
            }

            return otherCard.Id.Equals(Id);
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
    }
}