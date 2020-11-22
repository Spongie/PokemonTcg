using Assets.Code._2D;
using Entities;
using System.Collections.Generic;
using TCGCards;
using TCGCards.Core.Messages;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Game
{
    public class SelectColorPanel : MonoBehaviour
    {
        private EnergyResourceManager energyResources;
        public GameObject buttonHolder;
        private bool onlyColorsInGame;

        public void Init(string message, bool onlyInGame)
        {
            GetComponent<Text>().text = message;
            onlyColorsInGame = onlyInGame;
        }

        private HashSet<EnergyTypes> GetEnergyTypesInGame()
        {
            var set = new HashSet<EnergyTypes>();

            set.Add(GameController.Instance.Player.ActivePokemonCard.Type);
            set.Add(GameController.Instance.OpponentPlayer.ActivePokemonCard.Type);

            foreach (var pokemon in GameController.Instance.Player.BenchedPokemon)
            {
                set.Add(pokemon.Type);
            }
            foreach (var pokemon in GameController.Instance.OpponentPlayer.BenchedPokemon)
            {
                set.Add(pokemon.Type);
            }

            return set;
        }

        private void Start()
        {
            energyResources = GameObject.FindGameObjectWithTag("_global_").GetComponent<EnergyResourceManager>();
            HashSet<EnergyTypes> typesInGame = GetEnergyTypesInGame();

            foreach (var key in energyResources.Icons.Keys)
            {
                if (onlyColorsInGame && !typesInGame.Contains(key))
                {
                    continue;
                }

                var child = new GameObject();
                child.transform.SetParent(buttonHolder.transform);
                Button button = child.AddComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    onClick(key);
                });
                child.AddComponent<Image>().sprite = energyResources.Icons[key];
                child.AddComponent<LayoutElement>();
            }
        }

        private void onClick(EnergyTypes energyType)
        {
            var message = new SelectColorMessage(energyType).ToNetworkMessage(NetworkManager.Instance.Me.Id);
            message.ResponseTo = NetworkManager.Instance.RespondingTo;

            NetworkManager.Instance.Me.Send(message);

            NetworkManager.Instance.RespondingTo = null;
            GameController.Instance.OnCardPicked();
            gameObject.SetActive(false);
        }
    }
}
