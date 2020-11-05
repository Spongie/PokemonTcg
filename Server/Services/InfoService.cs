using System.IO;
using TCGCards.Core;

namespace Server.Services
{
    public class InfoService : IService
    {
        public void InitTypes()
        {
            
        }

        public VersionNumber GetVersion() => new VersionNumber(File.ReadAllText("client.version"));
        public VersionNumber GetCardsVersion() => new VersionNumber(File.ReadAllText("cards.version"));

        public byte[] GetClientBytes() => File.ReadAllBytes("Client.zip");

        public string GetPokemonJson() => File.ReadAllText("pokemon.json");
        public string GetEnergyJson() => File.ReadAllText("energy.json");
        public string GetTrainerJson() => File.ReadAllText("trainers.json");
        public string GetSetsJson() => File.ReadAllText("sets.json");
    }
}
