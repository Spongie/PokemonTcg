using NetworkingCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards.Core
{
    public class Player
    {
        private bool endedTurn = false;
        public event EventHandler<PlayerCardDraw> OnCardsDrawn;
        public event EventHandler<PlayerCardDraw> OnCardsDiscarded;

        public Player()
        {
            Hand = new List<Card>();
            BenchedPokemon = new Bench();
            PrizeCards = new List<Card>();
            Deck = new Deck();
            DiscardPile = new List<Card>();
        }

        internal void RevealCard(Card card)
        {
            var message = new RevealCardsMessage(new List<Card> { card }).ToNetworkMessage(Id);
            NetworkPlayer.Send(message);
        }

        internal void RevealCards(List<Card> cards)
        {
            foreach (var card in cards)
            {
                card.RevealTo(Id);
            }

            var message = new RevealCardsMessage(cards).ToNetworkMessage(Id);
            NetworkPlayer.Send(message);
        }

        public Player(INetworkPlayer networkPlayer) :this()
        {
            SetNetworkPlayer(networkPlayer);
        }

        public void SetPrizeCards(int prizeCards)
        {
            for(int _ = 0; _ < prizeCards; _++)
            {
                var card = Deck.DrawCard();
                PrizeCards.Add(card);
            }
        }

        public void DiscardCards(List<NetworkId> cardIds)
        {
            var cards = Hand.Where(card => cardIds.Contains(card.Id)).ToList();

            DiscardCards(cards);
        }

        public void DiscardCards(List<Card> cards)
        {
            DiscardPile.AddRange(cards);
            foreach (var card in cards)
            {
                Hand.Remove(card);
                card.RevealToAll();
            }

            OnCardsDiscarded?.Invoke(this, new PlayerCardDraw() { Cards = new List<Card>(cards), Amount = cards.Count(), Player = this });
        }

        public void TriggerDiscardEvent(List<Card> cardsDiscarded)
        {
            OnCardsDiscarded?.Invoke(this, new PlayerCardDraw() { Cards = new List<Card>(cardsDiscarded), Amount = cardsDiscarded.Count(), Player = this });
        }

        public void DiscardCard(Card card)
        {
            DiscardPile.Add(card);
            Hand.Remove(card);
            card.RevealToAll();

            OnCardsDiscarded?.Invoke(this, new PlayerCardDraw() { Cards = new List<Card> { card }, Amount = 1, Player = this });
        }

        public void DrawCardsFromDeck(List<NetworkId> selectedCards)
        {
            var cards = Deck.Cards.Where(card => selectedCards.Contains(card.Id)).ToList();

            DrawCardsFromDeck(cards);
        }

        public void DrawCardsFromDeck(List<Card> selectedCards)
        {
            Deck.Cards = new Stack<Card>(Deck.Cards.Except(selectedCards));
            Hand.AddRange(selectedCards);
            Deck.Shuffle();

            OnCardsDrawn?.Invoke(this, new PlayerCardDraw()
            {
                Amount = selectedCards.Count(),
                Cards = new List<Card>(selectedCards),
                Player = this
            });
        }

        public void SetBenchedPokemon(PokemonCard pokemon)
        {
            if (Hand.Contains(pokemon))
            {
                pokemon.PlayedThisTurn = true;
                Hand.Remove(pokemon);
            }

            BenchedPokemon.Add(pokemon);
            pokemon.RevealToAll();
        }

        public void ForceRetreatActivePokemon(PokemonCard replacementPokemon, GameField game)
        {
            var oldActivePokemon = ActivePokemonCard;
            ActivePokemonCard = replacementPokemon;
            BenchedPokemon.ReplaceWith(replacementPokemon, oldActivePokemon);
            oldActivePokemon.ClearStatusEffects();

            game?.SendEventToPlayers(new PokemonBecameActiveEvent
            {
                NewActivePokemonId = replacementPokemon.Id,
                ReplacedPokemonId = oldActivePokemon.Id
            });
        }

        public void RetreatActivePokemon(PokemonCard replacementPokemon, List<EnergyCard> payedEnergy, GameField game)
        {
            var retreatStoppers = GetAllPokemonCards().SelectMany(pokemon => pokemon.TemporaryAbilities.OfType<RetreatStopper>());

            if(!ActivePokemonCard.CanReatreat() || retreatStoppers.Any())
            {
                return;
            }

            foreach (var energyCard in payedEnergy)
            {
                ActivePokemonCard.DiscardEnergyCard(energyCard, game);
            }

            var oldActivePokemon = ActivePokemonCard;
            ActivePokemonCard = replacementPokemon;

            BenchedPokemon.ReplaceWith(replacementPokemon, oldActivePokemon);

            oldActivePokemon.ClearStatusEffects();

            game?.SendEventToPlayers(new PokemonBecameActiveEvent
            {
                NewActivePokemonId = replacementPokemon.Id,
                ReplacedPokemonId = oldActivePokemon.Id
            });
        }

        public void DrawPrizeCard(Card prizeCard)
        {
            Hand.Add(prizeCard);
            PrizeCards.Remove(prizeCard);
        }

        public void EndTurn(GameField game)
        {
            if(endedTurn)
            {
                return;
            }

            endedTurn = true;
            HasPlayedEnergy = false;
            ActivePokemonCard?.EndTurn(game);

            foreach(var pokemon in BenchedPokemon.ValidPokemonCards)
            {
                pokemon?.EndTurn(game);
            }
        }

        public void PlayCard(PokemonCard pokemon)
        {
            if (ActivePokemonCard == null)
            {
                ActivePokemonCard = pokemon;
            }
            else
            {
                BenchedPokemon.Add(pokemon);
            }

            pokemon.RevealToAll();
        }
        
        public void SetActivePokemon(PokemonCard pokemon)
        {
            if (ActivePokemonCard != null)
            {
                return;
            }

            if(Hand.Contains(pokemon))
            {
                pokemon.PlayedThisTurn = true;
                Hand.Remove(pokemon);
            }

            BenchedPokemon.Remove(pokemon);

            ActivePokemonCard = pokemon;
            pokemon.RevealToAll();
        }

        public void SwapActivePokemon(PokemonCard pokemon, GameField game)
        {
            BenchedPokemon.ReplaceWith(pokemon, ActivePokemonCard);

            game?.SendEventToPlayers(new PokemonBecameActiveEvent
            {
                NewActivePokemonId = pokemon.Id,
                ReplacedPokemonId = ActivePokemonCard.Id
            });

            ActivePokemonCard = pokemon;
            pokemon.RevealToAll();
        }

        public void PlayEnergyCard(EnergyCard energyCard, PokemonCard targetPokemonCard, GameField game = null)
        {
            if(HasPlayedEnergy || !Hand.Contains(energyCard) || Hand.Contains(targetPokemonCard))
                return;

            game?.GameLog.AddMessage($"Attaching {energyCard.GetName()} to {targetPokemonCard.GetName()}");

            HasPlayedEnergy = true;
            energyCard.RevealToAll();
            targetPokemonCard.AttachedEnergy.Add(energyCard);

            bool fromHand = false;
            if (Hand.Contains(energyCard)) 
            {
                fromHand = true;
                Hand.Remove(energyCard);
            }

            game?.SendEventToPlayers(new EnergyCardsAttachedEvent()
            {
                AttachedTo = targetPokemonCard,
                EnergyCard = energyCard
            });

            energyCard.OnAttached(targetPokemonCard, fromHand, game);
            
            game?.PushGameLogUpdatesToPlayers();
        }

        internal void ResetTurn()
        {
            endedTurn = false;
        }

        public void SetDeck(Deck deck)
        {
            Deck = deck;

            deck.Shuffle();
        }

        public void DrawCards(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            if (amount > Deck.Cards.Count)
            {
                IsDead = true;
            }

            var drawnCards = new List<Card>();

            for(int i = 0; i < amount; i++)
            {
                if (Deck.Cards.Count > 0)
                {
                    var card = Deck.DrawCard();
                    drawnCards.Add(card);
                    Hand.Add(card);
                }
            }

            OnCardsDrawn?.Invoke(this, new PlayerCardDraw()
            {
                Amount = drawnCards.Count,
                Cards = new List<Card>(drawnCards),
                Player = this
            });
        }

        public void SetNetworkPlayer(INetworkPlayer networkPlayer)
        {
            if (networkPlayer.Id != null)
            {
                Id = networkPlayer.Id;
            }

            NetworkPlayer = networkPlayer;
        }

        public List<PokemonCard> GetAllPokemonCards()
        {
            var pokemonCards = new List<PokemonCard>();

            if (ActivePokemonCard != null)
            {
                pokemonCards.Add(ActivePokemonCard);
            }

            foreach (var pokemon in BenchedPokemon.ValidPokemonCards)
            {
                if (pokemon != null)
                {
                    pokemonCards.Add(pokemon);
                }
            }

            return pokemonCards;
        }

        public Bench BenchedPokemon { get; set; }
        public PokemonCard ActivePokemonCard { get; set; }
        public List<Card> PrizeCards { get; set; }
        public List<Card> DiscardPile { get; set; }
        public Deck Deck { get; set; }
        public List<Card> Hand { get; set; }
        public bool HasPlayedEnergy { get; protected set; }
        public NetworkId Id { get; set; }
        public bool IsDead { get; set; }
        public int TurnsTaken { get; set; }

        [JsonIgnore]
        public INetworkPlayer NetworkPlayer { get; private set; }

        public bool IsRegistered()
        {
            return Id != null && Deck != null;
        }

        public void SelectPrizeCard(int amount, GameField game)
        {
            if (amount <= 0)
            {
                return;
            }

            var message = new SelectPrizeCardsMessage(amount).ToNetworkMessage(NetworkId.Generate());

            var response = NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);
            var cardsDrawn = new List<Card>();

            foreach (var cardId in response.Cards)
            {
                var card = PrizeCards.First(x => x.Id.Equals(cardId));
                PrizeCards.Remove(card);
                Hand.Add(card);
                cardsDrawn.Add(card);
            }

            game.SendEventToPlayers(new DrawCardsEvent() { Cards = new List<Card>(cardsDrawn), Amount = cardsDrawn.Count, Player = Id });
        }

        public void SelectActiveFromBench(GameField game)
        {
            game.GetOpponentOf(this).NetworkPlayer?.Send(new InfoMessage("Opponent is selecting a new active Pokémon").ToNetworkMessage(Id));

            var message = new SelectFromYourBenchMessage(1).ToNetworkMessage(Id);

            var response = NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            var card = (PokemonCard)game.Cards[response.Cards.First()];

            game?.SendEventToPlayers(new PokemonBecameActiveEvent
            {
                NewActivePokemonId = card.Id
            });

            SetActivePokemon(card);
        }

        public void KillActivePokemon()
        {
            KillPokemon(ActivePokemonCard);
            ActivePokemonCard = null;
        }

        private void KillPokemon(PokemonCard pokemon)
        {
            DiscardPile.AddRange(pokemon.AttachedEnergy);
            pokemon.AttachedEnergy.Clear();

            if (pokemon.EvolvedFrom != null)
            {
                DiscardPile.Add(pokemon.EvolvedFrom);

                if (pokemon.EvolvedFrom.EvolvedFrom != null)
                {
                    DiscardPile.Add(pokemon.EvolvedFrom.EvolvedFrom);
                    pokemon.EvolvedFrom.EvolvedFrom = null;
                }

                pokemon.EvolvedFrom = null;
            }

            DiscardPile.Add(pokemon);

            pokemon.DamageCounters = 0;
            pokemon.ClearStatusEffects();
        }

        public void KillBenchedPokemon(PokemonCard pokemon)
        {
            KillPokemon(pokemon);
            BenchedPokemon.Remove(pokemon);
        }
    }
}
