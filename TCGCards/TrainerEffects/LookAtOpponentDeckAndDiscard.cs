using System.Collections.Generic;
using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class LookAtOpponentDeckAndDiscard : DataModel, IEffect
    {
        private CardType onlyDiscardType;
        private bool drawsIfWrong;
        private CoinFlipConditional coinFlipConditional = new CoinFlipConditional();

        public string EffectType
        {
            get
            {
                return "Look at top card and discard";
            }
        }

        [DynamicInput("Condition", InputControl.Dynamic)]
        public CoinFlipConditional CoinFlipConditional
        {
            get { return coinFlipConditional; }
            set
            {
                coinFlipConditional = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Discard type", InputControl.Dropdown, typeof(CardType))]
        public CardType OnlyDiscardType
        {
            get { return onlyDiscardType; }
            set
            {
                onlyDiscardType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Draw if wrong", InputControl.Boolean)]
        public bool DrawsIfWrong
        {
            get { return drawsIfWrong; }
            set
            {
                drawsIfWrong = value;
                FirePropertyChanged();
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (!CoinFlipConditional.IsOk(game, caster))
            {
                return;
            }

            var topCard = opponent.Deck.Cards.Pop();

            caster.RevealCard(topCard);

            if (OnlyDiscardType == CardType.Any)
            {
                opponent.DiscardPile.Add(topCard);
                opponent.TriggerDiscardEvent(new List<Card> { topCard });
            }
            else if (OnlyDiscardType == CardType.Pokemon && topCard is PokemonCard)
            {
                opponent.DiscardPile.Add(topCard);
                opponent.TriggerDiscardEvent(new List<Card> { topCard });
            }
            else if (OnlyDiscardType == CardType.Trainer && topCard is TrainerCard)
            {
                opponent.DiscardPile.Add(topCard);
                opponent.TriggerDiscardEvent(new List<Card> { topCard });
            }
            else if (OnlyDiscardType == CardType.Energy && topCard is EnergyCard)
            {
                opponent.DiscardPile.Add(topCard);
                opponent.TriggerDiscardEvent(new List<Card> { topCard });
            }
            else if (OnlyDiscardType == CardType.BasicEnergy && topCard is EnergyCard && ((EnergyCard)topCard).IsBasic)
            {
                opponent.DiscardPile.Add(topCard);
                opponent.TriggerDiscardEvent(new List<Card> { topCard });
            }
            else if (OnlyDiscardType == CardType.BasicPokemon && topCard is PokemonCard && ((PokemonCard)topCard).Stage == 0)
            {
                opponent.DiscardPile.Add(topCard);
                opponent.TriggerDiscardEvent(new List<Card> { topCard });
            }
            else if (DrawsIfWrong)
            {
                opponent.Deck.Cards.Push(topCard);
                opponent.DrawCards(1);
            }
            else
            {
                opponent.Deck.Cards.Push(topCard);
            }
        }
    }
}
