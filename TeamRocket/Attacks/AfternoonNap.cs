using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.EnergyCards;

namespace TeamRocket.Attacks
{
    public class AfternoonNap : Attack, IDeckSearcher
    {
        public AfternoonNap()
        {
            Name = "Afternoon Nap";
            Description = "Search your deck for a Psychic Energy card and attach it to Slowpoke. Shuffle your deck afterward.";
            DamageText = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var selectedCard = (PsychicEnergy)game.FindCardById(this.TriggerDeckSearch(owner).First());

            owner.AttachEnergyToPokemon(selectedCard, owner.ActivePokemonCard, null);
            owner.ActivePokemonCard.Owner.DrawCardsFromDeck(new[] { selectedCard });
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
            public bool IsCardValid(Card card)
            {
                return card is PsychicEnergy;
            }
        }
    }
}
