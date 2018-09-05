using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class BenchSelectedMessage : ActiveSelectedMessage
    {
        public BenchSelectedMessage(PokemonCard active, Player owner) : base(active, owner)
        {
            messageType = MessageTypes.SelectedBench;
        }
    }
}
