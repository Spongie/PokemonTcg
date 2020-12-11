using Entities;
using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectFromYourPokemonMessage : AbstractNetworkMessage
    {
        public SelectFromYourPokemonMessage() : this(string.Empty, new EnergyTypes[] { })
        {

        }

        public SelectFromYourPokemonMessage(string info) : this (info, new EnergyTypes[] { })
        {

        }

        public SelectFromYourPokemonMessage(params EnergyTypes[] targetTypes) : this(string.Empty, targetTypes)
        {

        }

        public SelectFromYourPokemonMessage(string info, params EnergyTypes[] targetTypes)
        {
            MessageType = MessageTypes.SelectFromYourPokemon;
            TargetTypes = targetTypes;
            Info = info;
        }

        public EnergyTypes[] TargetTypes { get; set; }
        public string Info { get; set; }
        public IDeckFilter Filter { get; set; }
    }
}
