using CardEditor.Views;
using Entities;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class MillAndDamageFromEnergy : Attack
    {
        private int cardsToMill;
        private EnergyTypes energyType;

        [DynamicInput("Extra damage for this", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Cards to discard from deck")]
        public int CardsToMill
        {
            get { return cardsToMill; }
            set
            {
                cardsToMill = value;
                FirePropertyChanged();
            }
        }
        
        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var cards = new List<Card>();

            for (int i = 0; i < CardsToMill; i++)
            {
                if (owner.Deck.Cards.Count == 0)
                {
                    break;
                }

                var card = owner.Deck.Cards.Pop();

                cards.Add(card);
            }

            owner.DiscardPile.AddRange(cards);
            owner.TriggerDiscardEvent(cards);

            return Damage * cards.OfType<EnergyCard>().Count(card => EnergyType == EnergyTypes.All || EnergyType == EnergyTypes.None || card.EnergyType == EnergyType);
        }
    }
}
