using CardEditor.Views;
using Entities.Models;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class BouncePokemonEffect : DataModel, IEffect
    {
        private TargetingMode targetingMode; 
        private bool shuffleIntoDeck;
        private bool returnAttachedToHand;
        private bool onlyBasic;

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
                return opponent.BenchedPokemon.Any();
            }

            return caster.BenchedPokemon.Any();
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var target = CardUtil.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);

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
            foreach (var card in target.AttachedEnergy)
            {
                if (shuffleIntoDeck)
                {
                    target.Owner.Deck.Cards.Push(card);
                }
                else if (ReturnAttachedToHand)
                {
                    target.Owner.Hand.Add(card);
                }
                else
                {
                    target.Owner.DiscardPile.Add(card);
                }
            }
            target.AttachedEnergy.Clear();

            if (target.Stage == 0)
            {
                if (shuffleIntoDeck)
                {
                    target.Owner.Deck.Cards.Push(target);
                }
                else
                {
                    target.Owner.DiscardPile.Add(target);
                }

                return;
            }

            var pokemon = target;

            if (pokemon.Stage == 0)
            {
                DiscardOrBounceBasic(target, shuffleIntoDeck, pokemon);
            }
            else if (pokemon.Stage == 1)
            {
                DiscardOrBounceBasic(target, shuffleIntoDeck, pokemon.EvolvedFrom);
                DiscardOrBounceBasic(target, shuffleIntoDeck, pokemon);
                pokemon.EvolvedFrom = null;
            }
            else
            {
                target.Owner.DiscardPile.Add(pokemon.EvolvedFrom.EvolvedFrom);
                DiscardOrBounceBasic(target, shuffleIntoDeck, pokemon.EvolvedFrom.EvolvedFrom);
                pokemon.EvolvedFrom.EvolvedFrom = null;
                DiscardOrBounceBasic(target, shuffleIntoDeck, pokemon.EvolvedFrom);
                pokemon.EvolvedFrom = null;
                DiscardOrBounceBasic(target, shuffleIntoDeck, pokemon);
            }

            if (target == target.Owner.ActivePokemonCard)
            {
                target.Owner.ActivePokemonCard = null;
                target.Owner.SelectActiveFromBench(game);
            }
            else
            {
                target.Owner.BenchedPokemon.Remove(target);
            }
        }

        private void DiscardOrBounceBasic(PokemonCard target, bool shuffleIntoDeck, PokemonCard pokemon)
        {
            if (pokemon.Stage > 0)
            {
                target.Owner.DiscardPile.Add(pokemon);
            }
            else if (shuffleIntoDeck)
            {
                target.Owner.Deck.Cards.Push(pokemon);
            }
            else if (returnAttachedToHand)
            {
                target.Owner.Hand.Add(pokemon);
            }
            else
            {
                target.Owner.DiscardPile.Add(pokemon);
            }
        }

        private void ReturnCardsToHand(PokemonCard target, GameField game)
        {
            foreach (var card in target.AttachedEnergy)
            {
                if (ReturnAttachedToHand)
                {
                    target.Owner.Hand.Add(card);
                }
                else
                {
                    target.Owner.DiscardPile.Add(card);
                }
            }
            target.AttachedEnergy.Clear();

            var evolution2 = target;

            while (evolution2.EvolvedFrom != null)
            {
                if (ReturnAttachedToHand)
                {
                    target.Owner.Hand.Add(evolution2.EvolvedFrom);
                }
                else
                {
                    target.Owner.DiscardPile.Add(evolution2.EvolvedFrom);
                }

                evolution2 = evolution2.EvolvedFrom;
                evolution2.EvolvedFrom = null;
            }

            target.Owner.Hand.Add(target);

            if (target == target.Owner.ActivePokemonCard)
            {
                target.Owner.ActivePokemonCard = null;
                target.Owner.SelectActiveFromBench(game);
            }
            else
            {
                target.Owner.BenchedPokemon.Remove(target);
            }
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
                target.Owner.Deck.Cards.Push(evolution.EvolvedFrom);

                evolution = evolution.EvolvedFrom;
                evolution.EvolvedFrom = null;
            }

            target.Owner.Deck.Cards.Push(target);
            target.Owner.Deck.Shuffle();

            if (target == target.Owner.ActivePokemonCard)
            {
                target.Owner.ActivePokemonCard = null;
                target.Owner.SelectActiveFromBench(game);
            }
            else
            {
                target.Owner.BenchedPokemon.Remove(target);
            }
        }
    }
}
