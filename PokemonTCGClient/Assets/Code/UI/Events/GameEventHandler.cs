using System;
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
        public PokemonRemovedFromBenchEventHandler PokemonRemovedFromBenchEventHandler;
        public PokemonEvolvedEventHandler PokemonEvolvedEventHandler;
        public AttachedEnergyDiscardedEventHandler AttachedEnergyDiscardedEventHandler;
        public PokemonDiedEventHandler PokemonDiedEventHandler;
        public PokemonBouncedEventHandler PokemonBouncedEventHandler;
        public PokemonHealedEventHandler PokemonHealedEventHandler;
        public StadiumCardPlayedEventHandler StadiumCardPlayedEventHandler;
        public StadiumCardDestroyedEventHandler StadiumCardDestroyedEventHandler;

        private Queue<TCGCards.Core.GameEvents.Event> eventQueue;
        public TCGCards.Core.GameEvents.Event currentEvent;

        public CardRenderer tempTarget;
        private EnergyCard energyCard;
        private PokemonCard pokemon;

        private void Awake()
        {
            eventQueue = new Queue<TCGCards.Core.GameEvents.Event>();
            Instance = this;
        }

        private void Start()
        {
            //pokemon = new PokemonCard { IsRevealed = true, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/29_hires.png" };
            ////energyCard = new EnergyCard { IsRevealed = true, EnergyType = Entities.EnergyTypes.Fire, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/98_hires.png" };
            //tempTarget.SetCard(pokemon, ZoomMode.Center, true);
            //GameController.Instance.AddCard(tempTarget);
            //tempTarget.insertAttachedEnergy(new EnergyCard { EnergyType = Entities.EnergyTypes.Fire, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/98_hires.png" });
            //tempTarget.insertAttachedEnergy(energyCard);
        }

        //PokemonCard activePokemon;
        //PokemonCard benchPokemon;

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
                
            //    activePokemon = new PokemonCard { IsRevealed = true, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/29_hires.png" };
            //    var a = new PokemonBecameActiveEvent()
            //    {
            //        NewActivePokemon = activePokemon
            //    };

            //    eventQueue.Enqueue(a);

            //    GameController.Instance.myId = NetworkingCore.NetworkId.Generate();
            //    benchPokemon = new PokemonCard { IsRevealed = true, SetCode = "base1", ImageUrl = "https://images.pokemontcg.io/base1/29_hires.png" };
            //    var x = new PokemonAddedToBenchEvent()
            //    {
            //        Player = GameController.Instance.myId,
            //        Pokemon = benchPokemon,
            //        Index = 0
            //    };

            //    eventQueue.Enqueue(x);
            //}

            //if (Input.GetKeyDown(KeyCode.C))
            //{
            //    var a2 = new PokemonBecameActiveEvent()
            //    {
            //        NewActivePokemonId = benchPokemon.Id,
            //        ReplacedPokemonId = activePokemon.Id
            //    };

            //    TriggerEvent(a2);
            //}

            if (currentEvent != null || eventQueue.Count == 0)
            {
                return;
            }

            var nextInQueue = eventQueue.Dequeue();
            currentEvent = nextInQueue;
            TriggerEvent(nextInQueue);
        }

        internal bool AnyEventPending()
        {
            return currentEvent != null || eventQueue.Count > 0;
        }

        private void TriggerEvent(TCGCards.Core.GameEvents.Event gameEvent)
        {
            currentEvent = gameEvent;

            switch (gameEvent.GameEvent)
            {
                case GameEventType.TrainerCardPlayed:
                    TrainerCardPlayedEventHandler.gameObject.SetActive(true);
                    TrainerCardPlayedEventHandler.TriggerCardPlayed(((TrainerCardPlayed)gameEvent).Card);
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
                    EventCompleted();
                    break;
                case GameEventType.GameInfo:
                    EventCompleted();
                    break;
                case GameEventType.PokemonBounced:
                    PokemonBouncedEventHandler.gameObject.SetActive(true);
                    PokemonBouncedEventHandler.Trigger((PokemonBouncedEvent)gameEvent);
                    break;
                case GameEventType.PokemonHealed:
                    PokemonHealedEventHandler.gameObject.SetActive(true);
                    PokemonHealedEventHandler.Trigger((PokemonHealedEvent)gameEvent);
                    break;
                case GameEventType.RemovedFromBench:
                    PokemonRemovedFromBenchEventHandler.gameObject.SetActive(true);
                    PokemonRemovedFromBenchEventHandler.Trigger((PokemonRemovedFromBench)gameEvent);
                    break;
                case GameEventType.StadiumCardPlayed:
                    StadiumCardPlayedEventHandler.gameObject.SetActive(true);
                    StadiumCardPlayedEventHandler.Trigger((StadiumCardPlayedEvent)gameEvent);
                    break;
                case GameEventType.StadiumCardDestroyed:
                    StadiumCardDestroyedEventHandler.gameObject.SetActive(true);
                    StadiumCardDestroyedEventHandler.Trigger((StadiumDestroyedEvent)gameEvent);
                    break;
                default:
                    break;
            }

            GameController.Instance.EnableButtons();
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
            PokemonBouncedEventHandler.gameObject.SetActive(false);
            PokemonHealedEventHandler.gameObject.SetActive(false);
            PokemonBouncedEventHandler.gameObject.SetActive(false);
            StadiumCardPlayedEventHandler.gameObject.SetActive(false);
            StadiumCardDestroyedEventHandler.gameObject.SetActive(false);

            string info = currentEvent is GameSyncEvent ? ((GameSyncEvent)currentEvent).Info : "";
            GameController.Instance.OnInfoUpdated(currentEvent.GameField, info);
            currentEvent = null;
        }

        internal void EnqueueEvent(TCGCards.Core.GameEvents.Event gameEvent)
        {
            eventQueue.Enqueue(gameEvent);
        }
    }
}