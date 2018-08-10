using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectColorMessage : AbstractNetworkMessage
    {
        public SelectColorMessage(string message)
        {
            Message = message;
        }

        public SelectColorMessage(EnergyTypes color)
        {
            Color = color;
        }

        public string Message { get; private set; }

        public EnergyTypes Color { get; private set; }
    }
}
