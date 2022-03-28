using TCGCards.Core;

public interface IInfoService
{
    VersionNumber GetCardsVersion();
    byte[] GetClientBytes();
    string GetEnergyJson();
    string GetFormatsJson();
    string GetPokemonJson();
    string GetSetsJson();
    string GetTrainerJson();
    VersionNumber GetVersion();
}