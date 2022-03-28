using Entities.Models;
using NetworkingCore;
using System.Collections.Generic;
using TCGCards;

public interface ICardService
{
    Card CreateCardById(NetworkId id);
    List<Card> GetAllCards();
    List<Set> GetAllSets();
    bool UpdateCards(string pokemonCards, string energyCards, string tainerCards, string sets, string formats);
}