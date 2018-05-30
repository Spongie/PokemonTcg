using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.EnergyCards;

namespace TeamRocket.Abilities
{
    public class AfternoonNap : AbstractDeckSearcherAbility
    {
        public AfternoonNap(IPokemonCard owner) : base(owner)
        {
            TriggerType = TriggerType.Activation;
        }

        public override void Activate(Player owner, Player opponent)
        {
            var selectedCard = (PsychicEnergy)TriggerDeckSearch(owner).First();

            Owner.AttachedEnergy.Add(selectedCard);
            Owner.Owner.DrawCardsFromDeck(new[] { selectedCard });
        }

        public override void SetTarget(IPokemonCard target)
        {
        }

        protected override List<IDeckFilter> GetDeckFilters()
        {
            return new List<IDeckFilter> { new AfternoonNapDeckFilter() };
        }

        protected override int GetNumberOfCards()
        {
            return 1;
        }

        private class AfternoonNapDeckFilter : IDeckFilter
        {
            public bool IsCardValid(ICard card)
            {
                return card is PsychicEnergy;
            }
        }
    }
}
