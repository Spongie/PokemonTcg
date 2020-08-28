using NetworkingCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core.Messages;

namespace TCGCards.Core
{
    public class Player
    {
        private readonly int MaxBenchedPokemons = 5;
        private bool endedTurn = false;

        public Player()
        {
            Hand = new List<Card>();
            BenchedPokemon = new List<PokemonCard>();
            PrizeCards = new List<Card>();
            Deck = new Deck();
            DiscardPile = new List<Card>();
        }

        public Player(INetworkPlayer networkPlayer) :this()
        {
            SetNetworkPlayer(networkPlayer);
        }

        public void SetPrizeCards(int priceCards)
        {
            for(int _ = 0; _ < priceCards; _++)
            {
                PrizeCards.Add(Deck.DrawCard());
            }
        }

        public void DiscardCards(IEnumerable<NetworkId> cardIds)
        {
            var cards = Hand.Where(card => cardIds.Contains(card.Id)).ToList();

            DiscardCards(cards);
        }

        public void DiscardCards(IEnumerable<Card> cards)
        {
            DiscardPile.AddRange(cards);
            foreach (var card in cards)
            {
                Hand.Remove(card);
                card.IsRevealed = true;
            }
        }

        public void DiscardCard(Card card)
        {
            DiscardPile.Add(card);
            Hand.Remove(card);
            card.IsRevealed = true;
        }

        public void DrawCardsFromDeck(IEnumerable<NetworkId> selectedCards)
        {
            var cards = Deck.Cards.Where(card => selectedCards.Contains(card.Id)).ToList();

            DrawCardsFromDeck(cards);
        }

        public void DrawCardsFromDeck(IEnumerable<Card> selectedCards)
        {
            Deck.Cards = new Stack<Card>(Deck.Cards.Except(selectedCards));
            Hand.AddRange(selectedCards);
            Deck.Shuffle();
        }

        public void SetBenchedPokemon(PokemonCard pokemon)
        {
            if(BenchedPokemon.Count < MaxBenchedPokemons && pokemon.Stage == 0)
            {
                if(Hand.Contains(pokemon))
                {
                    pokemon.PlayedThisTurn = true;
                    Hand.Remove(pokemon);
                }

                BenchedPokemon.Add(pokemon);
                pokemon.IsRevealed = true;
            }
        }

        public void ForceRetreatActivePokemon(PokemonCard replacementPokemon)
        {
            var oldActivePokemon = ActivePokemonCard;
            ActivePokemonCard = replacementPokemon;
            BenchedPokemon.Remove(replacementPokemon);
            BenchedPokemon.Add(oldActivePokemon);
            oldActivePokemon.ClearStatusEffects();
        }

        public void RetreatActivePokemon(PokemonCard replacementPokemon, IEnumerable<EnergyCard> payedEnergy)
        {
            if(!ActivePokemonCard.CanReatreat())
                return;

            foreach (var energyCard in payedEnergy)
            {
                ActivePokemonCard.DiscardEnergyCard(energyCard);
            }

            var oldActivePokemon = ActivePokemonCard;
            ActivePokemonCard = replacementPokemon;
            BenchedPokemon.Remove(replacementPokemon);
            BenchedPokemon.Add(oldActivePokemon);
            oldActivePokemon.ClearStatusEffects();
        }

        public void DrawPrizeCard(Card prizeCard)
        {
            Hand.Add(prizeCard);
            PrizeCards.Remove(prizeCard);
        }

        public void EndTurn()
        {
            if(endedTurn)
                return;

            endedTurn = true;
            HasPlayedEnergy = false;
            ActivePokemonCard?.EndTurn();

            foreach(var pokemon in BenchedPokemon)
            {
                pokemon.EndTurn();
            }
        }

        public void PlayCard(PokemonCard card)
        {
            if(ActivePokemonCard == null)
                ActivePokemonCard = card;
            else
                BenchedPokemon.Add(card);

            card.IsRevealed = true;
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
            else if(BenchedPokemon.Contains(pokemon))
                BenchedPokemon.Remove(pokemon);

            ActivePokemonCard = pokemon;
            pokemon.IsRevealed = true;
        }

        public void AttachEnergyToPokemon(EnergyCard energyCard, PokemonCard targetPokemonCard, GameField game = null)
        {
            if(HasPlayedEnergy)
                return;

            game?.GameLog.AddMessage($"Attaching {energyCard.GetName()} to {targetPokemonCard.GetName()}");

            HasPlayedEnergy = true;
            energyCard.IsRevealed = true;
            targetPokemonCard.AttachedEnergy.Add(energyCard);

            energyCard.OnAttached(targetPokemonCard, Hand.Contains(energyCard));
            Hand.Remove(energyCard);

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
            if (amount > Deck.Cards.Count)
            {
                IsDead = true;
            }

            for(int i = 0; i < amount; i++)
            {
                if (Deck.Cards.Count > 0)
                    Hand.Add(Deck.DrawCard());
            }
        }

        public void SetNetworkPlayer(INetworkPlayer networkPlayer)
        {
            Id = networkPlayer.Id;
            NetworkPlayer = networkPlayer;
        }

        public IEnumerable<PokemonCard> GetAllPokemonCards()
        {
            if (ActivePokemonCard != null)
            {
                yield return ActivePokemonCard;
            }

            foreach (var pokemon in BenchedPokemon)
            {
                yield return pokemon;
            }
        }

        public List<PokemonCard> BenchedPokemon { get; set; }
        public PokemonCard ActivePokemonCard { get; set; }
        public List<Card> PrizeCards { get; set; }
        public List<Card> DiscardPile { get; set; }
        public Deck Deck { get; set; }
        public List<Card> Hand { get; set; }
        public bool HasPlayedEnergy { get; protected set; }
        public NetworkId Id { get; set; }
        public bool IsDead { get; set; }

        [JsonIgnore]
        public INetworkPlayer NetworkPlayer { get; private set; }

        public bool IsRegistered()
        {
            return Id != null && Deck != null;
        }

        public void SelectPriceCard(int amount)
        {
            var message = new SelectPriceCardsMessage(amount).ToNetworkMessage(NetworkId.Generate());

            var response = NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            foreach (var cardId in response.Cards)
            {
                var card = PrizeCards.First(x => x.Id.Equals(cardId));
                PrizeCards.Remove(card);
                Hand.Add(card);
            }
        }

        public void SelectActiveFromBench()
        {
            var message = new SelectFromYourBench(1).ToNetworkMessage(Id);

            var response = NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            var card = BenchedPokemon.First(x => x.Id.Equals(response.Cards.First()));

            SetActivePokemon(card);
        }

        public void KillActivePokemon()
        {
            var pokemon = ActivePokemonCard;

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
            ActivePokemonCard = null;
        }
    }
}
