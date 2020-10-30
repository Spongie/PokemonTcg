using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class ShuffleHandIntoDeck : DataModel, IEffect
    {
        private bool opponents;

        [DynamicInput("Targets opponent?", InputControl.Boolean)]
        public bool Opponents
        {
            get { return opponents; }
            set
            {
                opponents = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Shuffle hand into deck";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent) => true;

        public void Process(GameField game, Player caster, Player opponent)
        {
            var target = opponents ? opponent : caster;

            target.Deck.ShuffleInCards(opponent.Hand);
            target.Hand.Clear();
        }
    }
}
