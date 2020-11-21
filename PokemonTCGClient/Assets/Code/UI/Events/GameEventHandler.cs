using System.Collections.Generic;
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
        public CardDiscardedEventHandler CardDiscardedEventHandler;
        public PokemonAttackedEventHandler PokemonAttackedEventHandler;
        public DamageTakenEventHandler DamageTakenEventHandler;
        public AbilityActivatedEventHandler AbilityActivatedEventHandler;
        public PokemonBecameActiveEventHandler PokemonBecameActiveEventHandler;
        public PokemonAddedToBenchEventHandler PokemonAddedToBenchEventHandler;
        public PokemonEvolvedEventHandler PokemonEvolvedEventHandler;
        public AttachedEnergyDiscardedEventHandler AttachedEnergyDiscardedEventHandler;
        public PokemonDiedEventHandler PokemonDiedEventHandler;

        //public CardRenderer tempTarget;
        //private EnergyCard energyCard;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            //energyCard = new EnergyCard { EnergyType = Entities.EnergyTypes.Psychic, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/98_hires.png" };
            //tempTarget.insertAttachedEnergy(new EnergyCard { EnergyType = Entities.EnergyTypes.Fire, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/98_hires.png" });
            //tempTarget.insertAttachedEnergy(energyCard);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //var gameEvent = new TCGCards.Core.GameEvents.AttachedEnergyDiscardedEvent()
                //{
                //    DiscardedCard = energyCard,
                //    FromPokemonId = tempTarget.card.Id
                //};

                //var gameEvent = new CoinsFlippedEvent(new List<bool> { true, true }, null);

               //TriggerEvent(gameEvent);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                var gameEvent = new TCGCards.Core.GameEvents.PokemonDiedEvent()
                {
                    Pokemon = null
                };

                TriggerEvent(gameEvent);
            }

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    var gameEvent = new CardsDiscardedEvent()
            //    {
            //        Cards = new List<Card>
            //        {
            //            new EnergyCard { IsRevealed = true, EnergyType = Entities.EnergyTypes.Psychic, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/98_hires.png" },
            //            new EnergyCard { IsRevealed = true, EnergyType = Entities.EnergyTypes.Psychic, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/98_hires.png" },
            //            new EnergyCard { IsRevealed = true, EnergyType = Entities.EnergyTypes.Psychic, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/98_hires.png" }
            //        }
            //    };

            //    TriggerEvent(gameEvent);
            //}
        }
        //TODO: Add queue for events

        private void TriggerEvent(TCGCards.Core.GameEvents.Event gameEvent)
        {
            switch (gameEvent.GameEvent)
            {
                case GameEventType.TrainerCardPlayed:
                    TrainerCardPlayedEventHandler.gameObject.SetActive(true);
                    TrainerCardPlayedEventHandler.TriggerCardPlayer(((TrainerCardPlayed)gameEvent).Card);
                    break;
                case GameEventType.PokemonAttacks:
                    PokemonAttackedEventHandler.gameObject.SetActive(true);
                    PokemonAttackedEventHandler.TriggerEvent(((PokemonAttackedEvent)gameEvent).Player);
                    break;
                case GameEventType.PokemonActivatesAbility:
                    AbilityActivatedEventHandler.gameObject.SetActive(true);
                    AbilityActivatedEventHandler.Trigger((AbilityActivatedEvent)gameEvent);
                    break;
                case GameEventType.PokemonTakesDamage:
                    DamageTakenEventHandler.gameObject.SetActive(true);
                    DamageTakenEventHandler.Trigger((DamageTakenEvent)gameEvent);
                    break;
                case GameEventType.DrawsCard:
                    CardDrawEventHandler.gameObject.SetActive(true);
                    CardDrawEventHandler.TriggerCardsDrawn(((DrawCardsEvent)gameEvent).Cards);
                    break;
                case GameEventType.DiscardsCard:
                    CardDiscardedEventHandler.gameObject.SetActive(true);
                    CardDiscardedEventHandler.Trigger((CardsDiscardedEvent)gameEvent);
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
                case GameEventType.PokemonBecameActive:
                    PokemonBecameActiveEventHandler.gameObject.SetActive(true);
                    PokemonBecameActiveEventHandler.Trigger((PokemonBecameActiveEvent)gameEvent);
                    break;
                case GameEventType.PokemonAddedToBench:
                    PokemonAddedToBenchEventHandler.gameObject.SetActive(true);
                    PokemonAddedToBenchEventHandler.Trigger((PokemonAddedToBenchEvent)gameEvent);
                    break;
                case GameEventType.PokemonEvolved:
                    PokemonEvolvedEventHandler.gameObject.SetActive(true);
                    PokemonEvolvedEventHandler.Trigger((PokemonEvolvedEvent)gameEvent);
                    break;
                case GameEventType.EnergyCardDiscarded:
                    AttachedEnergyDiscardedEventHandler.gameObject.SetActive(true);
                    AttachedEnergyDiscardedEventHandler.Trigger((AttachedEnergyDiscardedEvent)gameEvent);
                    break;
                case GameEventType.PokemonDied:
                    PokemonDiedEventHandler.gameObject.SetActive(true);
                    PokemonDiedEventHandler.Trigger((PokemonDiedEvent)gameEvent);
                    break;
                case GameEventType.SyncGame:
                    GameController.Instance.OnGameUpdated(((GameSyncEvent)gameEvent).Game, null);
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
            PokemonAttackedEventHandler.gameObject.SetActive(false);
            DamageTakenEventHandler.gameObject.SetActive(false);
            AbilityActivatedEventHandler.gameObject.SetActive(false);
            CardDiscardedEventHandler.gameObject.SetActive(false);
            PokemonBecameActiveEventHandler.gameObject.SetActive(false);
            PokemonAddedToBenchEventHandler.gameObject.SetActive(false);
            PokemonEvolvedEventHandler.gameObject.SetActive(false);
            AttachedEnergyDiscardedEventHandler.gameObject.SetActive(false);
            PokemonDiedEventHandler.gameObject.SetActive(false);
            //TODO: Go to next in queue
        }
    }
}
