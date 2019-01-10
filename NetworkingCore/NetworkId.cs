using System;
using System.Collections.Generic;

namespace NetworkingCore
{
    public class NetworkId
    {
        public Guid Value { get; set; }

        public static NetworkId Generate()
        {
            return new NetworkId { Value = Guid.NewGuid() };
        }

        public static implicit operator Guid(NetworkId value)
        {
            return value.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static NetworkId Empty
        {
            get
            {
                return new NetworkId { Value = Guid.Empty };
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as NetworkId;

            if (other == null)
            {
                return false;
            }

            return other.Value.Equals(Value);
        }

        public override int GetHashCode()
        {
            return -1937169414 + EqualityComparer<Guid>.Default.GetHashCode(Value);
        }
    }
}
