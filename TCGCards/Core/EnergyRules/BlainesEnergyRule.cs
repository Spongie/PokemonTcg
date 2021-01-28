using Entities;
using NetworkingCore;

namespace TCGCards.Core.EnergyRules
{
    public struct BlainesEnergyRule : IEnergyRule
    {
        public bool CanPlayEnergyCard(EnergyCard card, PokemonCard target)
        {
            if (AcceptedRule && !target.Id.Equals(TargetId))
            {
                return false;
            }

            if (target.Name.Contains("Blaine") && card.EnergyType == EnergyTypes.Fire)
            {
                if (TargetId != null)
                {
                    return target.Id.Equals(TargetId);
                }

                return CardsAttachedThisTurn < 2;
            }

            if (AcceptedRule)
            {
                return false;
            }

            return CardsAttachedThisTurn < 1;
        }

        public void CardPlayed(EnergyCard card, PokemonCard target)
        {
            if (target.Name.Contains("Blaine") && card.EnergyType == EnergyTypes.Fire)
            {
                AcceptedRule = true;
                TargetId = target.Id;
            }

            CardsAttachedThisTurn++;
        }

        public void Reset()
        {
            CardsAttachedThisTurn = 0;
        }

        public NetworkId TargetId { get; set; }
        public int CardsAttachedThisTurn { get; set; }
        public bool AcceptedRule { get; set; }
    }
}
