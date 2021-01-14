using Assets.Code._2D;
using Entities;
using System;
using System.Collections.Generic;
using TCGCards.Core.Messages;
using TMPro;
using UnityEngine;

namespace Assets.Code.UI.Game
{
    public class SelectColorPanel : MonoBehaviour
    {
        public TextMeshProUGUI infoText;

        public void Init(SelectColorMessage colorMessage)
        {
            if (!string.IsNullOrEmpty(colorMessage.Message))
            {
                infoText.text = colorMessage.Message;
            }
            else
            {
                infoText.text = "Select a Type";
            }
            HashSet<EnergyTypes> colors;

            if (colorMessage.OnlyColorsInGame)
            {
                colors = GetEnergyTypesInGame();
            }
            else
            {
                colors = new HashSet<EnergyTypes>();

                foreach (EnergyTypes value in Enum.GetValues(typeof(EnergyTypes)))
                {
                    colors.Add(value);
                }
            }

            foreach (var symbol in GetComponentsInChildren<SelectEnergySymbolHandler>())
            {
                symbol.gameObject.SetActive(colors.Contains(symbol.EnergyType));
            }
        }

        private HashSet<EnergyTypes> GetEnergyTypesInGame()
        {
            var set = new HashSet<EnergyTypes>();

            set.Add(GameController.Instance.Player.ActivePokemonCard.Type);
            set.Add(GameController.Instance.OpponentPlayer.ActivePokemonCard.Type);

            foreach (var pokemon in GameController.Instance.Player.BenchedPokemon.ValidPokemonCards)
            {
                set.Add(pokemon.Type);
            }
            foreach (var pokemon in GameController.Instance.OpponentPlayer.BenchedPokemon.ValidPokemonCards)
            {
                set.Add(pokemon.Type);
            }

            return set;
        }
    }
}
