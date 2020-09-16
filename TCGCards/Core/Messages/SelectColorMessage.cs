using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectColorMessage : AbstractNetworkMessage
    {
        public SelectColorMessage()
        {

        }

        public SelectColorMessage(string message)
        {
            Message = message;
            MessageType = MessageTypes.SelectColor;
        }

        public SelectColorMessage(EnergyTypes color)
        {
            Color = color;
        }

        public string Message { get; set; }

        public EnergyTypes Color { get; set; }
    }
}
