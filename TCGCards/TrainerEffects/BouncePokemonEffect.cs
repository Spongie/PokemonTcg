using CardEditor.Views;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.GameEvents;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class BouncePokemonEffect : DataModel, IEffect
    {
        private TargetingMode targetingMode; 
        private bool shuffleIntoDeck;
        private bool returnAttachedToHand;
        private bool onlyBasic;
        private bool isMay;
        private bool coinFlip;
        private int coins = 1;
        private int headsForSuccess = 1;

        [DynamicInput("Coin Flip", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Coins")]
        public int Coins
        {
            get { return coins; }
            set
            {
                coins = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Heads for success")]
        public int HeadsForSuccess
        {
            get { return headsForSuccess; }
            set
            {
                headsForSuccess = value;
                FirePropertyChanged();
            }
        }



        [DynamicInput("Only return basic version", InputControl.Boolean)]
        public bool OnlyBasic
        {
            get { return onlyBasic; }
            set
            {
                onlyBasic = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Shuffle target into deck? (Otherwise hand)", InputControl.Boolean)]
        public bool ShuffleIntoDeck
        {
            get { return shuffleIntoDeck; }
            set
            {
                shuffleIntoDeck = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Add attached cards to hand? (Only if sending to hand)", InputControl.Boolean)]
        public bool ReturnAttachedToHand
        {
            get { return returnAttachedToHand; }
            set
            {
                returnAttachedToHand = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Ask Yes/No?", InputControl.Boolean)]
        public bool IsMay
        {
            get { return isMay; }
            set
            {
                isMay = value;
                FirePropertyChanged();
            }
        }


        public string EffectType
        {
            get
            {
                return "Bounce pokemon";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            if (TargetingMode == TargetingMode.OpponentActive || TargetingMode == TargetingMode.OpponentBench || TargetingMode == TargetingMode.OpponentPokemon)
            {
                return opponent.BenchedPokemon.Count > 0;
            }

            return caster.BenchedPokemon.Count > 0;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (TargetingMode == TargetingMode.OpponentActive && opponent.ActivePokemonCard.IsDead())
            {
                return;
            }

            string specificTargetName;

            switch (targetingMode)
            {
                case TargetingMode.YourActive:
                    specificTargetName = caster.ActivePokemonCard.Name;
                    break;
                case TargetingMode.OpponentActive:
                    specificTargetName = opponent.ActivePokemonCard.Name;
                    break;
                case TargetingMode.Self:
                    specificTargetName = pokemonSource.Name;
                    break;
                default:
                    specificTargetName = "Pokémon";
                    break;
            }

            if (isMay && !CardUtil.AskYesNo(caster, $"Return {specificTargetName} to it's owners hand?"))
            {
                return;
            }

            if (CoinFlip && game.FlipCoins(Coins) < HeadsForSuccess)
            {
                return;
            }

            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);

            if (target == null)
            {
                return;
            }

            if (onlyBasic)
            {
                ReturnOnlyBasic(target, shuffleIntoDeck, game);
            }
            else if (shuffleIntoDeck)
            {
                ShuffleCardsIntoDeck(target, game);
            }
            else
            {
                ReturnCardsToHand(target, game);
            }
        }

        private void ReturnOnlyBasic(PokemonCard target, bool shuffleIntoDeck, GameField game)
        {
            var energyCards = new List<EnergyCard>(target.AttachedEnergy);
            target.AttachedEnergy.Clear();

            foreach (var card in energyCards)
            {
                if (shuffleIntoDeck)
                {
                    target.Owner.Deck.Cards.Push(card);
                    game.SendEventToPlayers(new DrawCardsEvent { Player = target.Owner.Id, Cards = new List<Card> { card } });
                }
                else if (ReturnAttachedToHand)
                {
                    target.Owner.Hand.Add(card);
                    game.SendEventToPlayers(new DrawCardsEvent { Player = target.Owner.Id, Cards = new List<Card> { card } });
                }
                else
                {
                    target.Owner.DiscardPile.Add(card);
                    game.SendEventToPlayers(new AttachedEnergyDiscardedEvent() { DiscardedCard = card, FromPokemonId = target.Id });
                }
            }

            var pokemon = target;

            game.SendEventToPlayers(new PokemonBouncedEvent()
            {
                PokemonId = pokemon.Id
            });

            if (pokemon.Stage == 0)
            {
                DiscardOrBounceBasic(target, shuffleIntoDeck, pokemon, game);
            }
            else if (pokemon.Stage == 1)
            {
                DiscardOrBounceBasic(target, shuffleIntoDeck, pokemon.EvolvedFrom, game);
                DiscardOrBounceBasic(target, shuffleIntoDeck, pokemon, game);
                pokemon.EvolvedFrom = null;
            }
            else
            {
                target.Owner.DiscardPile.Add(pokemon.EvolvedFrom.EvolvedFrom);
                game.SendEventToPlayers(new CardsDiscardedEvent { Player = target.Owner.Id, Cards = new List<Card> { pokemon.EvolvedFrom.EvolvedFrom } });
                DiscardOrBounceBasic(target, shuffleIntoDeck, pokemon.EvolvedFrom.EvolvedFrom, game);
                pokemon.EvolvedFrom.EvolvedFrom = null;
                DiscardOrBounceBasic(target, shuffleIntoDeck, pokemon.EvolvedFrom, game);
                pokemon.EvolvedFrom = null;
                DiscardOrBounceBasic(target, shuffleIntoDeck, pokemon, game);
            }

            if (target == target.Owner.ActivePokemonCard)
            {
                target.Owner.ActivePokemonCard = null;
                target.Owner.SelectActiveFromBench(game);
            }
        }

        private void DiscardOrBounceBasic(PokemonCard target, bool shuffleIntoDeck, PokemonCard pokemon, GameField game)
        {
            pokemon.EvolvedFrom = null;
            pokemon.DamageCounters = 0;
            pokemon.ClearStatusEffects();
            target.Owner.BenchedPokemon.Remove(pokemon);

            if (pokemon.Stage > 0)
            {
                if (shuffleIntoDeck)
                {
                    target.Owner.Deck.ShuffleInCard(pokemon);
                }
                else
                {
                    target.Owner.DiscardPile.Add(pokemon);
                }
                game.SendEventToPlayers(new CardsDiscardedEvent { Player = target.Owner.Id, Cards = new List<Card> { pokemon} });
            }
            else if (shuffleIntoDeck)
            {
                target.Owner.Deck.ShuffleInCard(pokemon);
                game.SendEventToPlayers(new DrawCardsEvent { Player = target.Owner.Id, Cards = new List<Card> { pokemon } });
            }
            else
            {
                target.Owner.Hand.Add(pokemon);
                game.SendEventToPlayers(new DrawCardsEvent { Player = target.Owner.Id, Cards = new List<Card> { pokemon } });
            }
        }

        private void ReturnCardsToHand(PokemonCard target, GameField game)
        {
            var cardsDiscarded = new List<Card>();
            var cardsDrawn = new List<Card>();

            foreach (var card in target.AttachedEnergy)
            {
                if (ReturnAttachedToHand)
                {
                    target.Owner.Hand.Add(card);
                    cardsDrawn.Add(card);
                }
                else
                {
                    target.Owner.DiscardPile.Add(card);
                    cardsDiscarded.Add(card);
                }
            }
            target.AttachedEnergy.Clear();

            var evolution2 = target;
            
            while (evolution2.EvolvedFrom != null)
            {
                evolution2.EvolvedFrom.DamageCounters = 0;
                evolution2.EvolvedFrom.ClearStatusEffects();

                if (ReturnAttachedToHand)
                {
                    target.Owner.Hand.Add(evolution2.EvolvedFrom);
                    cardsDrawn.Add(evolution2.EvolvedFrom);
                }
                else
                {
                    target.Owner.DiscardPile.Add(evolution2.EvolvedFrom);
                    cardsDiscarded.Add(evolution2.EvolvedFrom);
                }

                evolution2 = evolution2.EvolvedFrom;
                evolution2.EvolvedFrom = null;
            }

            target.DamageCounters = 0;
            target.ClearStatusEffects();
            target.Owner.Hand.Add(target);
            cardsDrawn.Add(target);

            game?.SendEventToPlayers(new CardsDiscardedEvent() { Player = target.Owner.Id, Cards = new List<Card>(cardsDrawn) });
            
            if (target == target.Owner.ActivePokemonCard)
            {
                target.Owner.ActivePokemonCard = null;
                game?.SendEventToPlayers(new PokemonBouncedEvent() { PokemonId = target.Id, ToHand = true });
                target.Owner.SelectActiveFromBench(game);
            }
            else
            {
                target.Owner.BenchedPokemon.Remove(target);
                game?.SendEventToPlayers(new PokemonBouncedEvent() { PokemonId = target.Id, ToHand = true });
            }

            game?.SendEventToPlayers(new DrawCardsEvent() { Player = target.Owner.Id, Cards = new List<Card>(cardsDrawn) });
        }

        private static void ShuffleCardsIntoDeck(PokemonCard target, GameField game)
        {
            foreach (var card in target.AttachedEnergy)
            {
                target.Owner.Deck.Cards.Push(card);
            }
            target.AttachedEnergy.Clear();

            var evolution = target;

            while (evolution.EvolvedFrom != null)
            {
                evolution.EvolvedFrom.DamageCounters = 0;
                evolution.EvolvedFrom.ClearStatusEffects();

                target.Owner.Deck.Cards.Push(evolution.EvolvedFrom);

                evolution = evolution.EvolvedFrom;
                evolution.EvolvedFrom = null;
            }

            target.DamageCounters = 0;
            target.ClearStatusEffects();
            target.Owner.Deck.Cards.Push(target);
            target.Owner.Deck.Shuffle();

            if (target == target.Owner.ActivePokemonCard)
            {
                target.Owner.ActivePokemonCard = null;
                game?.SendEventToPlayers(new PokemonBouncedEvent() { PokemonId = target.Id });
                target.Owner.SelectActiveFromBench(game);
            }
            else
            {
                game?.SendEventToPlayers(new PokemonBouncedEvent() { PokemonId = target.Id });
                target.Owner.BenchedPokemon.Remove(target);
                game?.SendEventToPlayers(new PokemonRemovedFromBench { PokemonId = target.Id });
            }
        }
    }
}
