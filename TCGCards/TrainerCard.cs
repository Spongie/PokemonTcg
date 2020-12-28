using System.Collections.ObjectModel;
using System.Linq;
using TCGCards.Core;
using TCGCards.TrainerEffects;

namespace TCGCards
{
    public class TrainerCard : Card
    {
        private ObservableCollection<IEffect> effects = new ObservableCollection<IEffect>();
        private bool addToDiscardWhenCasting = true;
        private int maxCardsInHand = 99;
        private int maxBenchedPokemon = 99;
        private Ability ability;
        private int maxBasicPokemonInHand = 99;

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
            return Effects.All(effect => effect.CanCast(game, caster, opponent))
                && caster.Hand.Count < MaxCardsInHand
                && caster.BenchedPokemon.Count < MaxBenchedPokemon
                && caster.Hand.OfType<PokemonCard>().Count(p => p.Stage == 0) < MaxBasicInHand;
        }

        public override string GetName() => Name;

        public bool IsStadium()
        {
            return ability != null;
        }

        public bool AddToDiscardWhenCasting
        {
            get { return addToDiscardWhenCasting; }
            set
            {
                addToDiscardWhenCasting = value;
                FirePropertyChanged();
            }
        }

        public int MaxCardsInHand
        {
            get { return maxCardsInHand; }
            set
            {
                maxCardsInHand = value;
                FirePropertyChanged();
            }
        }

        public int MaxBenchedPokemon
        {
            get { return maxBenchedPokemon; }
            set
            {
                maxBenchedPokemon = value;
                FirePropertyChanged();
            }
        }

        public int MaxBasicInHand
        {
            get { return maxBasicPokemonInHand; }
            set
            {
                maxBasicPokemonInHand = value;
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

        public Ability Ability
        {
            get { return ability; }
            set
            {
                ability = value;
                FirePropertyChanged();
            }
        }

    }
}
