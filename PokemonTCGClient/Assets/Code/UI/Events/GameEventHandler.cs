using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCards;
using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class GameEventHandler : MonoBehaviour
    {
        public static GameEventHandler Instance;
        public CoinflipEventHandler CoinFlipHandler;
        public TrainerCardPlayerEventHandler TrainerCardPlayedEventHandler;
        public EnergyCardsAttachedEventHandler EnergyCardsAttachedEventHandler;
        public CardDrawEventHandler CardDrawEventHandler;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    //var gameEvent = new TCGCards.Core.GameEvents.TrainerCardPlayed(null)
            //    //{
            //    //    Card = new TrainerCard { SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/91_hires.png" }
            //    //};
            //
            //    var gameEvent = new CoinsFlippedEvent(new List<bool> { true, true }, null);
            //
            //    TriggerEvent(gameEvent);
            //}

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    var gameEvent = new TCGCards.Core.GameEvents.EnergyCardsAttachedEvent(null)
            //    {
            //        EnergyCard = new EnergyCard { EnergyType = Entities.EnergyTypes.Psychic, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/98_hires.png" },
            //        AttachedTo = (PokemonCard)CardRenderer.pokemon
            //    };

            //    TriggerEvent(gameEvent);
            //}

            if (Input.GetKeyDown(KeyCode.Space))
            {
                var gameEvent = new DrawCardsEvent(null)
                {
                    Cards = new List<Card>
                    {
                        new EnergyCard { IsRevealed = true, EnergyType = Entities.EnergyTypes.Psychic, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/98_hires.png" },
                        new EnergyCard { IsRevealed = true, EnergyType = Entities.EnergyTypes.Psychic, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/98_hires.png" },
                        new EnergyCard { IsRevealed = true, EnergyType = Entities.EnergyTypes.Psychic, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/98_hires.png" }
                    }
                };

                TriggerEvent(gameEvent);
            }
        }

        public void TriggerEvent(TCGCards.Core.GameEvents.Event gameEvent)
        {
            switch (gameEvent.GameEvent)
            {
                case GameEventType.TrainerCardPlayed:
                    TrainerCardPlayedEventHandler.gameObject.SetActive(true);
                    TrainerCardPlayedEventHandler.TriggerCardPlayer(((TrainerCardPlayed)gameEvent).Card);
                    break;
                case GameEventType.PokemonAttacks:
                    break;
                case GameEventType.PokemonActivatesAbility:
                    break;
                case GameEventType.PokemonTakesDamage:
                    break;
                case GameEventType.DrawsCard:
                    CardDrawEventHandler.gameObject.SetActive(true);
                    CardDrawEventHandler.TriggerCardsDrawn(((DrawCardsEvent)gameEvent).Cards);
                    break;
                case GameEventType.DiscardsCard:
                    break;
                case GameEventType.Flipscoin:
                    CoinFlipHandler.gameObject.SetActive(true);
                    CoinFlipHandler.TriggerCoinFlips(((CoinsFlippedEvent)gameEvent).Results);
                    break;
                case GameEventType.AttachesEnergy:
                    var energyEvent = (EnergyCardsAttachedEvent)gameEvent;
                    EnergyCardsAttachedEventHandler.gameObject.SetActive(true);
                    EnergyCardsAttachedEventHandler.TriggerCardPlayer(energyEvent.EnergyCard, energyEvent.AttachedTo);
                    break;
                default:
                    break;
            }
        }

        public void EventCompleted()
        {
            CoinFlipHandler.gameObject.SetActive(false);
            TrainerCardPlayedEventHandler.gameObject.SetActive(false);
            EnergyCardsAttachedEventHandler.gameObject.SetActive(false);
            CardDrawEventHandler.gameObject.SetActive(false);
        }
    }
}
