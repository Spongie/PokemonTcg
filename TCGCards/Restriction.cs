using Entities.Models;
using NetworkingCore;

namespace TCGCards
{
    public class Restriction : DataModel
    {
        private int maxCount;
        private NetworkId restrictedId;
        private string name;

        public NetworkId RestrictedId
        {
            get { return restrictedId; }
            set
            {
                restrictedId = value;
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


        public int MaxCount
        {
            get { return maxCount; }
            set
            {
                maxCount = value;
                FirePropertyChanged();
            }
        }

    }
}
