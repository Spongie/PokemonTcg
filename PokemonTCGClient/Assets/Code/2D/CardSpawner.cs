using System.Collections;
using System.Collections.Generic;
using TeamRocket.PokemonCards;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;

    void Start()
    {
        var card = new Ekans(null);
        Instantiate(cardPrefab, transform).GetComponent<CardRenderer>().SetCard(card);
    }
}
