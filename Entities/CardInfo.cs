using System.Collections.Generic;

namespace Entities
{
    public class CardInfo
    {
        public string ClassName { get; set; }
        public string SetId { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as CardInfo;

            if (other == null)
            {
                return false;
            }

            return other.ClassName == ClassName;
        }

        public override int GetHashCode()
        {
            var hashCode = 2082644842;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ClassName);
            hashCode = hashCode * -1521134295 + SetId.GetHashCode();
            return hashCode;
        }
    }
}
