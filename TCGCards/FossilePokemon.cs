using Entities;
using NetworkingCore;
using System.Collections.ObjectModel;
using TCGCards.Core;
using TCGCards.Core.Abilities;

namespace TCGCards
{
    public class FossilePokemon : PokemonCard
    {
        public FossilePokemon()
        {

        }

        public FossilePokemon(TrainerCard source)
        {
            Id = NetworkId.Generate();
            Name = source.Name;
            PokemonName = Name;
            Owner = source.Owner;
            SetCode = source.SetCode;
            RevealedToAll = true;
            ImageUrl = source.ImageUrl;
            Type = EnergyTypes.Colorless;
            Hp = 10;
            PrizeCards = 0;
            Attacks = new ObservableCollection<Attack>();
            Ability = new DiscardSelfAbility(this);
            Source = source;
        }

        public TrainerCard Source { get; set; }

        public override bool CanReatreat()
        {
            return false;
        }

        public override void ApplyStatusEffect(StatusEffect statusEffect, GameField game)
        {
            //Can't happen
        }
    }
}
