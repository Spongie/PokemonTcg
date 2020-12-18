﻿using Assets.Code.UI.Gameplay;
using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class PokemonBecameActiveEventHandler : MonoBehaviour
    {
        public GameObject CardPrefab;
        public Transform PlayerActivePokemonTransform;
        public CardZone PlayerBenchedPokemonZone;
        public Transform OpponentActivePokemonTransform;
        public CardZone OpponentBenchedPokemonZone;
        public CardRenderer TempActive;
        public CardRenderer TempBench;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Trigger(new PokemonBecameActiveEvent { ReplacedPokemonId = NetworkId.Generate() });
            }
        }

        private void SetActiveOngameStart(PokemonCard pokemonCard)
        {
            NetworkId myId = GameController.Instance.myId;
            var isMySwitch = pokemonCard.Owner.Id.Equals(myId);
            var activeParent = isMySwitch ? PlayerActivePokemonTransform : OpponentActivePokemonTransform;

            var gameObject = Instantiate(CardPrefab, activeParent);
            var renderer = gameObject.GetComponent<CardRenderer>();
            renderer.SetCard(pokemonCard, true);
            GameController.Instance.AddCard(renderer);
            GameEventHandler.Instance.EventCompleted();
        }

        public void Trigger(PokemonBecameActiveEvent activeEvent)
        {
            if (activeEvent.NewActivePokemon != null)
            {
                SetActiveOngameStart(activeEvent.NewActivePokemon);
                return;
            }

            var newActive = GameController.Instance.GetCardRendererById(activeEvent.NewActivePokemonId);
            NetworkId myId = GameController.Instance.myId;
            var isMySwitch = newActive.card.Owner.Id.Equals(myId);
            var activeParent = isMySwitch ? PlayerActivePokemonTransform : OpponentActivePokemonTransform;
            var benchParent = isMySwitch ? PlayerBenchedPokemonZone : OpponentBenchedPokemonZone;

            var oldPosition = newActive.GetComponent<RectTransform>().localPosition;
            newActive.transform.SetParent(activeParent, true);
            newActive.GetComponent<RectTransform>().LeanMove(Vector3.zero, 1.5f).setEaseInCubic();
            
            CardRenderer oldActive = null;

            if (activeEvent.ReplacedPokemonId != null)
            {
                var oldSize = new Vector2(newActive.GetComponent<RectTransform>().rect.width, newActive.GetComponent<RectTransform>().rect.height);
                oldActive = GameController.Instance.GetCardRendererById(activeEvent.ReplacedPokemonId);
                ((PokemonCard)oldActive.card).ClearStatusEffects();
                oldActive.tag = "Ignore";
                oldActive.transform.SetParent(benchParent.transform, true);
                oldActive.SetIsBenched();
                oldActive.EnableStatusIcons();
                oldActive.gameObject.LeanMove(Vector3.zero, 1.5f).setEaseInCubic().setOnComplete(() => 
                {
                    newActive.SetIsActivePokemon();
                    oldActive.tag = "Untagged";
                    GameEventHandler.Instance.EventCompleted();
                });
            }
            else
            {
                GameEventHandler.Instance.EventCompleted();
            }

            Player switchingPlayer = isMySwitch ? GameController.Instance.Player : GameController.Instance.OpponentPlayer;

            switchingPlayer.ActivePokemonCard = (PokemonCard)newActive.card;

            if (switchingPlayer.BenchedPokemon.Contains((PokemonCard)newActive.card))
            {
                switchingPlayer.BenchedPokemon.Remove((PokemonCard)newActive.card);
            }

            if (activeEvent.ReplacedPokemonId != null)
            {
                switchingPlayer.BenchedPokemon.Add((PokemonCard)oldActive.card);
            }
        }
    }
}
