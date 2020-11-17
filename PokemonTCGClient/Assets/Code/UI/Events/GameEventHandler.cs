using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class GameEventHandler : MonoBehaviour
    {
        public CoinflipEventHandler CoinFlipHandler;

        public void TriggerEvent(TCGCards.Core.GameEvents.Event gameEvent)
        {
            switch (gameEvent.GameEvent)
            {
                case GameEventType.TrainerCardPlayed:
                    break;
                case GameEventType.PokemonAttacks:
                    break;
                case GameEventType.PokemonActivatesAbility:
                    break;
                case GameEventType.PokemonTakesDamage:
                    break;
                case GameEventType.DrawsCard:
                    break;
                case GameEventType.DiscardsCard:
                    break;
                case GameEventType.Flipscoin:
                    CoinFlipHandler.TriggerCoinFlips(((CoinsFlippedEvent)gameEvent).Results);
                    break;
                case GameEventType.AttachesEnergy:
                    break;
                default:
                    break;
            }
        }
    }
}
