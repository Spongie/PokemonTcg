using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class BenchSelectedMessage : ActiveSelectedMessage
    {
        public BenchSelectedMessage(IPokemonCard active, Player owner) : base(active, owner)
        {
            messageType = MessageTypes.SelectedBench;
        }
    }
}
