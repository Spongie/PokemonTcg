using Entities;
using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectColorMessage : AbstractNetworkMessage
    {
        public SelectColorMessage() :this(EnergyTypes.All, string.Empty)
        {
            
        }

        public SelectColorMessage(string message) : this(EnergyTypes.All, message)
        {
            
        }

        public SelectColorMessage(EnergyTypes color) :this(color, string.Empty)
        {
            
        }

        public SelectColorMessage(EnergyTypes color, string info) 
        {
            Color = color;
            Message = info;
            MessageType = MessageTypes.SelectColor;
        }

        public string Message { get; set; }

        public EnergyTypes Color { get; set; }
        public bool OnlyColorsInGame { get; set; }
    }
}
