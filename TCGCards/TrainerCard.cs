using System.Collections.ObjectModel;
using System.Linq;
using TCGCards.Core;
using TCGCards.TrainerEffects;

namespace TCGCards
{
    public class TrainerCard : Card
    {
        private ObservableCollection<ITrainerEffect> effects = new ObservableCollection<ITrainerEffect>();

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
                effect.Process(game, caster, opponent);
            }
        }

        public virtual bool CanCast(GameField game, Player caster, Player opponent)
        {
            return Effects.All(effect => effect.CanCast(game, caster, opponent));
        }

        public override string GetName() => Name;

        public ObservableCollection<ITrainerEffect> Effects
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
