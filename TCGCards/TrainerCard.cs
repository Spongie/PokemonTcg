using System.Collections.ObjectModel;
using System.Linq;
using TCGCards.Core;
using TCGCards.TrainerEffects;

namespace TCGCards
{
    public class TrainerCard : Card
    {
        private ObservableCollection<IEffect> effects = new ObservableCollection<IEffect>();

        public TrainerCard() : base(null)
        {
                
        }

        public TrainerCard(Player owner) : base(owner)
        {
        }

        public virtual void Process(GameField game, Player caster, Player opponent)
        {
            foreach (var effect in Effects)
            {
                effect.Process(game, caster, opponent, null);
            }
        }

        public virtual bool CanCast(GameField game, Player caster, Player opponent)
        {
            return Effects.All(effect => effect.CanCast(game, caster, opponent));
        }

        public override string GetName() => Name;

        private bool addToDiscardWhenCasting = true;

        public bool AddToDiscardWhenCasting
        {
            get { return addToDiscardWhenCasting; }
            set
            {
                addToDiscardWhenCasting = value;
                FirePropertyChanged();
            }
        }


        public ObservableCollection<IEffect> Effects
        {
            get { return effects; }
            set
            {
                effects = value;
                FirePropertyChanged();
            }
        }

    }
}
