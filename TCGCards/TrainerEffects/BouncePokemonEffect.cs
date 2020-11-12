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

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            var target = CardUtil.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent);

            if (shuffleIntoDeck)
            {
                foreach (var card in target.AttachedEnergy)
                {
                    target.Owner.Deck.Cards.Push(card);
                }
                target.AttachedEnergy.Clear();
                target.Owner.Deck.Cards.Push(target);
                target.Owner.Deck.Shuffle();

                if (target == target.Owner.ActivePokemonCard)
                {
                    target.Owner.ActivePokemonCard = null;
                    target.Owner.SelectActiveFromBench();
                }

                return;
            }

            foreach (var card in target.AttachedEnergy)
            {
                target.Owner.Hand.Add(card);
            }
            target.AttachedEnergy.Clear();
            target.Owner.Hand.Add(target);

            if (target == target.Owner.ActivePokemonCard)
            {
                target.Owner.ActivePokemonCard = null;
                target.Owner.SelectActiveFromBench();
            }
        }
    }
}
