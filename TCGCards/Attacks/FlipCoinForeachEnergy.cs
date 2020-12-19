using CardEditor.Views;
using Entities;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.Attacks
{
    public class FlipCoinForeachEnergy : Attack
    {
        private bool discardForEachHead;
        private EnergyTypes onlyThisType = EnergyTypes.All;

        [DynamicInput("Discard for each heads?", InputControl.Boolean)]
        public bool DiscardForeachHead
        {
            get { return discardForEachHead; }
            set
            {
                discardForEachHead = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only this type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes OnlyThisType
        {
            get { return onlyThisType; }
            set
            {
                onlyThisType = value;
                FirePropertyChanged();
            }
        }

        private int extraBaseDamage;

        [DynamicInput("Extra Damage after flips")]
        public int ExtraBaseDamage
        {
            get { return extraBaseDamage; }
            set
            {
                extraBaseDamage = value;
                FirePropertyChanged();
            }
        }


        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var heads = game.FlipCoins(owner.ActivePokemonCard.AttachedEnergy.Count(e => OnlyThisType == EnergyTypes.All || e.EnergyType == OnlyThisType));

            if (heads > 0)
            {
                var message = new PickFromListMessage(owner.ActivePokemonCard.AttachedEnergy.OfType<Card>().ToList(), heads).ToNetworkMessage(owner.Id);
                var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);
                opponent.ActivePokemonCard.DiscardEnergyCard((EnergyCard)game.Cards[response.Cards.First()], game);
            }

            return ExtraBaseDamage + (heads * Damage);
        }
    }
}
