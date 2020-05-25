using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectFromYourPokemonMessage : AbstractNetworkMessage
    {
        public SelectFromYourPokemonMessage() : this(new EnergyTypes[] { })
        {

        }

        public SelectFromYourPokemonMessage(EnergyTypes[] targetTypes)
        {
            MessageType = MessageTypes.SelectFromYourPokemon;
            TargetTypes = targetTypes;
        }

        public EnergyTypes[] TargetTypes { get; set; }
    }
}
