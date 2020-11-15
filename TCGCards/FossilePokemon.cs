using Entities;
using System.Collections.ObjectModel;
using TCGCards.Core.Abilities;

namespace TCGCards
{
    public class FossilePokemon : PokemonCard
    {
        public FossilePokemon(TrainerCard source)
        {
            Name = source.Name;
            ImageUrl = source.ImageUrl;
            Type = EnergyTypes.Colorless;
            Hp = 10;
            PrizeCards = 0;
            Attacks = new ObservableCollection<Attack>();
            Ability = new DiscardSelfAbility(this);
        }

        public override bool CanReatreat()
        {
            return false;
        }

        public override void ApplyStatusEffect(StatusEffect statusEffect)
        {
            //Can't happen
        }
    }
}
