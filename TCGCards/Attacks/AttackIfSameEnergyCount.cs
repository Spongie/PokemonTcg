using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class AttackIfSameEnergyCount : Attack
    {
        public override bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            if (owner.ActivePokemonCard.AttachedEnergy.Count != opponent.ActivePokemonCard.AttachedEnergy.Count)
            {
                return false;
            }

            return base.CanBeUsed(game, owner, opponent);
        }
    }
}
