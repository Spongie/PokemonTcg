using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.EnergyCards;

namespace TeamRocket.Abilities
{
    public class AfternoonNap : Ability, IDeckSearcher
    {
        public AfternoonNap(IPokemonCard owner) : base(owner)
        {
            TriggerType = TriggerType.Activation;
        }

        public override void Activate(Player owner, Player opponent)
        {
            var selectedCard = (PsychicEnergy)this.TriggerDeckSearch(owner).First();

            Owner.AttachedEnergy.Add(selectedCard);
            Owner.Owner.DrawCardsFromDeck(new[] { selectedCard });
        }

        public override void SetTarget(ICard target)
        {
        }

        public List<IDeckFilter> GetDeckFilters()
        {
            return new List<IDeckFilter> { new AfternoonNapDeckFilter() };
        }

        public int GetNumberOfCards()
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
