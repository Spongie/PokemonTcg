﻿using System.Collections;
using System.Collections.Generic;
using TeamRocket.PokemonCards;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;

    void Start()
    {
        var card = new Ekans(null);
        for (int i = 0; i < 8; i++)
        {
            var spawnedCard = Instantiate(cardPrefab, transform);
            spawnedCard.GetComponentInChildren<CardRenderer>().SetCard(card);
            spawnedCard.GetComponent<Canvas>().sortingOrder = i;
        }
    }
}
